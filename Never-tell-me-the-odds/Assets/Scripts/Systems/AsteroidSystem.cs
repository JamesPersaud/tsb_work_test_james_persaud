using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;
using Unity.Collections;
using System.Collections.Generic;

/// <summary>
/// This system deals with asteroid collisions - with the bullets, the player - and anything else that may be out there
/// in space!
/// </summary>
public class AsteroidSystem : ComponentSystem
{
    public struct BulletCollisionCheckData
    {
        public float3 Position;
        public Entity BulletEntity;
    }

    protected override void OnUpdate()
    {
        ResolveSpawnEvents();
        CheckCollisions();        
    }

    private void CheckCollisions()
    {
        SettingsComponent settings = GetSingleton<SettingsComponent>();
        //The managed collection here is a particularly bad sin but it's simply just a lot less code to write
        List<BulletCollisionCheckData> bullets = new List<BulletCollisionCheckData>();

        //collect all the bullets
        Entities.WithAll<BulletComponent>().ForEach((
        Entity bulletEntity, ref Translation bulletTranslation) =>
        {
            bullets.Add(new BulletCollisionCheckData { Position = bulletTranslation.Value, BulletEntity = bulletEntity });
        });

        Entities.WithAll<AsteroidComponent>().ForEach((
            Entity asteroidEntity, ref AsteroidComponent asteroid, ref Translation asteroidTranslation) =>
        {
            AsteroidComponent.AsteroidSize asteroidSize = asteroid.Size;
            float3 asteroidPosition = asteroidTranslation.Value;

            //collide with bullets
            foreach (BulletCollisionCheckData data in bullets)
            {
                float asteroidRadius = settings.GetSizeScaleByAsteroidSize(asteroidSize) / 2;
                float distance = math.distance(asteroidPosition, data.Position);
            
                if (distance < asteroidRadius) //collision detected!
                {
                    if (asteroidSize != AsteroidComponent.AsteroidSize.SMALL)
                    {
                        AsteroidComponent.AsteroidSize nextSize =
                        (asteroidSize == AsteroidComponent.AsteroidSize.BIG) ?
                        AsteroidComponent.AsteroidSize.MEDIUM : AsteroidComponent.AsteroidSize.SMALL;

                        Entity eventEntity = EntityManager.CreateEntity(typeof(AsteroidSpawnEventComponent));
                        EntityManager.AddComponentData(eventEntity, new AsteroidSpawnEventComponent
                        {
                            NumToSpawn = settings.NumFragments,
                            RandomPositions = false,
                            Position = asteroidPosition,
                            Size = nextSize
                        });
                    }

                    EntityManager.AddComponent<DestroyMeComponent>(asteroidEntity);
                    EntityManager.AddComponent<DestroyMeComponent>(data.BulletEntity);
                }
            }
        });       
    }

    private void ResolveSpawnEvents()
    {
        Entities.WithAll<AsteroidSpawnEventComponent>().ForEach((
            Entity spawnEventEntity, ref AsteroidSpawnEventComponent spawnEvent) =>
        {
            SpawnAsteroids(spawnEvent.NumToSpawn, spawnEvent.RandomPositions, spawnEvent.Position, spawnEvent.Size);
            EntityManager.AddComponent<DestroyMeComponent>(spawnEventEntity);
        });
    }

    private void SpawnAsteroids(int numToSpawn, bool randomPosition, float3 position, AsteroidComponent.AsteroidSize size)
    {
        SettingsComponent settings = GetSingleton<SettingsComponent>();
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();

        for (int i = 0; i < numToSpawn; i++)
        {
            if (randomPosition)
            {
                position = GetRandomSpawnPosition(settings);
            }

            float spawnSpeed = UnityEngine.Random.Range(settings.AsteroidSpeedMin, settings.AsteroidSpeedMax);
            spawnSpeed *= settings.GetSpeedScaleByAsteroidSize(size);
            float3 spawnVelocity = new float3(0, spawnSpeed, 0);
            spawnVelocity = math.rotate(quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 2 * math.PI)), spawnVelocity);

            float scale = settings.GetSizeScaleByAsteroidSize(size);
            float4x4 trs = float4x4.TRS(position, float4.zero, new float3(scale, scale, 1));

            Entity asteroid = EntityManager.Instantiate(prefabLoader.AsteroidPrefabEntity);
            EntityManager.SetComponentData(asteroid, new Translation { Value = position });
            EntityManager.SetComponentData(asteroid, new VelocityComponent { Velocity = spawnVelocity });
            EntityManager.SetComponentData(asteroid, new AsteroidComponent { Size = size });
            EntityManager.SetComponentData(asteroid, new LocalToWorld { Value = trs });
            EntityManager.SetComponentData(asteroid, new NonUniformScale { Value = scale });
        }
    }

    private float3 GetRandomSpawnPosition(SettingsComponent settings)
    {
        float3 position = float3.zero;

        while (position.x > settings.NospawnBoundsMin.x && position.x < settings.NospawnBoundsMax.x &&
            position.y > settings.NospawnBoundsMin.y && position.y < settings.NospawnBoundsMax.y)
        {
            position.x = UnityEngine.Random.Range(-settings.ScreenWidth / 2, settings.ScreenWidth / 2);
            position.x = UnityEngine.Random.Range(-settings.ScreenHeight / 2, settings.ScreenHeight / 2);
        }

        return position;
    }
}
