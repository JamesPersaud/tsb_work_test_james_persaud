using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;

public class UfoSystem : ComponentSystem
{   
    protected override void OnUpdate()
    {
        GameSettingsComponent gameSettings = GetSingleton<GameSettingsComponent>();
        UfoSettingsComponent ufoSettings = GetSingleton<UfoSettingsComponent>();

        NativeList<Entity> bullets = new NativeList<Entity>(100, Allocator.Temp);
        Entities.WithAll<BulletComponent>().ForEach((
        Entity bulletEntity) =>
        {
            bullets.Add(bulletEntity);
        });

        int ufoCount = 0;
        //manage UFOs in play
        Entities.WithAll<UfoComponent>().ForEach((
            Entity ufoEntity, ref Translation ufoTranslation, ref UfoComponent ufo) =>
        {
            ufoCount++;

            ufo.TimeToFire -= Time.DeltaTime;
            if(ufo.TimeToFire <= 0)
            {
                float speed = GetSingleton<BulletSettingsComponent>().BulletBaseSpeed;
                float scale = GetUfoScaleByType(ufoSettings, ufo.Type);

                float3 firingVelocity = new float3(0, 1, 0);
                float3 firingPosition = new float3(0, 1, 0);

                if (ufo.Type == UfoComponent.UfoType.BIG_AND_DUMB)
                {
                    //fire at random
                    firingPosition = math.rotate(quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 2 * math.PI)), firingVelocity);                    
                    firingVelocity = firingPosition * speed;              
                }
                else
                {
                    //fire towards player
                    firingPosition = math.rotate(quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 2 * math.PI)), firingVelocity);
                    firingVelocity = firingPosition * speed;
                }

                firingPosition = firingPosition * scale + ufoTranslation.Value;

                Entity eventEntity = EntityManager.CreateEntity(typeof(FireBulletEventComponent));
                EntityManager.AddComponentData(eventEntity, new FireBulletEventComponent
                {
                    Position = firingPosition,
                    Velocity = firingVelocity,
                    FiredByPlayer = false
                });

                ufo.TimeToFire = UnityEngine.Random.Range(ufoSettings.UfoFireTimeMin, ufoSettings.UfoFireTimeMax);
                ufo.TimeToFire *= GetUfoFireRateByType(ufoSettings, ufo.Type);
            }

            //collision with bullets
            foreach(Entity bulletEntity in bullets)
            {
                Translation bulletTranslation = EntityManager.GetComponentData<Translation>(bulletEntity);
                float ufoRadius = GetUfoScaleByType(ufoSettings, ufo.Type) / 2;
                float distance = math.distance(ufoTranslation.Value, bulletTranslation.Value);

                if (distance < ufoRadius) // collision! One hit kills a ufo
                {
                    EntityManager.AddComponent<DestroyMeComponent>(ufoEntity);
                    EntityManager.AddComponent<DestroyMeComponent>(bulletEntity);
                }
            }            

            //is the ufo out of play?
            if (ufoTranslation.Value.x < -gameSettings.ScreenWidth /2 || 
                ufoTranslation.Value.x > gameSettings.ScreenWidth / 2)
            {             
                EntityManager.AddComponent<DestroyMeComponent>(ufoEntity);                           
            }
        });

        if (ufoCount == 0)
        {
            int spawnUfoEventCount = 0;
            //spawn a new UFO if due
            Entities.WithAll<SpawnUfoEventComponent>().ForEach((
                Entity eventEntity, ref SpawnUfoEventComponent spawnUfo) =>
            {
                spawnUfoEventCount++;

                spawnUfo.TimeToSpawn -= Time.DeltaTime;
                if(spawnUfo.TimeToSpawn <=0)
                {
                    EntityManager.AddComponent<DestroyMeComponent>(eventEntity);
                    SpawnNewUfoAtRandom(spawnUfo.Type);
                }
            });
            // if no UFOs in play, spawn a new one
            if(spawnUfoEventCount == 0)
            {               
                RaiseNewUfoSpawnEvent();
            }
        }

        bullets.Dispose();
    }

    private void SpawnNewUfoAtRandom(UfoComponent.UfoType type)
    {
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();
        GameSettingsComponent gameSettings = GetSingleton<GameSettingsComponent>();
        UfoSettingsComponent ufoSettings = GetSingleton<UfoSettingsComponent>();

        float scale = GetUfoScaleByType(ufoSettings, type);
        float speed = GetUfoSpeedByType(ufoSettings, type);

        float3 position = float3.zero;
        float3 velocity = float3.zero;
        
        int whichSide = UnityEngine.Random.Range(0,2);
        position.x = (whichSide == 0) ? -gameSettings.ScreenWidth / 2 + 1 : gameSettings.ScreenWidth / 2 -1;
        position.y = UnityEngine.Random.Range(-gameSettings.ScreenHeight/2 +1, gameSettings.ScreenHeight / 2 - 1);
        velocity.x = (whichSide == 0) ? speed : -speed;

        float4x4 trs = float4x4.TRS(position, float4.zero, new float3(scale, scale, 1));

        Entity asteroid = EntityManager.Instantiate(prefabLoader.UfoPrefabEntity);
        EntityManager.SetComponentData(asteroid, new Translation { Value = position });
        EntityManager.SetComponentData(asteroid, new VelocityComponent { Velocity = velocity });
        EntityManager.SetComponentData(asteroid, new UfoComponent { Type = type });
        EntityManager.SetComponentData(asteroid, new LocalToWorld { Value = trs });
        EntityManager.SetComponentData(asteroid, new NonUniformScale { Value = scale });
    }

    private void RaiseNewUfoSpawnEvent()
    {
        UfoSettingsComponent ufoSettings = GetSingleton<UfoSettingsComponent>();
        float timeToSpawn = UnityEngine.Random.Range(ufoSettings.UfoSpawnTimeMin, ufoSettings.UfoSpawnTimeMax);
        float randomType = UnityEngine.Random.Range(0f, 1f);
        UfoComponent.UfoType type = (randomType < ufoSettings.UfoSpawnSmallUfoChance) ?
            UfoComponent.UfoType.SMALL_AND_VIOLENT : UfoComponent.UfoType.BIG_AND_DUMB;

        Entity eventEntity = EntityManager.CreateEntity(typeof(SpawnUfoEventComponent));
        EntityManager.AddComponentData<SpawnUfoEventComponent>(eventEntity, new SpawnUfoEventComponent
        {
            TimeToSpawn = timeToSpawn,
            Type = type
        });
    }
    private float GetUfoScaleByType(UfoSettingsComponent ufoSettings, UfoComponent.UfoType type)
    {
        if(type == UfoComponent.UfoType.BIG_AND_DUMB)
        {
            return ufoSettings.UfoScaleByTypeBig;
        }
        else
        {
            return ufoSettings.UfoScaleByTypeSmall;
        }
    }

    private float GetUfoSpeedByType(UfoSettingsComponent ufoSettings, UfoComponent.UfoType type)
    {
        if (type == UfoComponent.UfoType.BIG_AND_DUMB)
        {
            return ufoSettings.UfoSpeedByTypeBig;
        }
        else
        {
            return ufoSettings.UfoSpeedByTypeSmall;
        }
    }

    private float GetUfoFireRateByType(UfoSettingsComponent ufoSettings, UfoComponent.UfoType type)
    {
        if (type == UfoComponent.UfoType.BIG_AND_DUMB)
        {
            return ufoSettings.UfoFireRateByTypeBig;
        }
        else
        {
            return ufoSettings.UfoFireRateByTypeSmall;
        }
    }
}
