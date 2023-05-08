using System;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;
using Unity.Transforms;

/// <summary>
/// This is poorly organized, there should be a more generic system for handling powerups and their effects
/// removing the duplication of code between the two types implemented
/// </summary>
public class PowerUpSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        int activeShieldEffects = 0;
        int activeShieldPickups = 0;

        int activeDoubleShotPickups = 0;
        int activeDoubleShotEffects = 0;

        //double shot pickups
        Entities.WithAll<DoubleShotPickupComponent>().ForEach((
        Entity pickupEntity, ref DoubleShotPickupComponent shotPickup, ref Translation pickupTranslation) =>
        {
            activeDoubleShotPickups++;
            float3 position = pickupTranslation.Value;

            //collide with player            
            Entities.WithAll<PlayerComponent>().ForEach((
                ref Translation playerTranslation) =>
            {
                float distance = math.distance(position, playerTranslation.Value);
                if (distance < 1)
                {
                    SpawnShotEffect();
                    EntityManager.AddComponent(pickupEntity, typeof(DestroyMeComponent));
                }
            });

            //expire
            shotPickup.TimeRemaining -= Time.DeltaTime;
            if (shotPickup.TimeRemaining <= 0)
            {
                EntityManager.AddComponent<DestroyMeComponent>(pickupEntity);
            }
        });

        //double shot effects      
        Entities.WithAll<DoubleShotEffectComponent>().ForEach((
        Entity effectEntity, ref DoubleShotEffectComponent shieldEffect) =>
        {
            activeDoubleShotEffects++;       

            shieldEffect.TimeRemaining -= Time.DeltaTime;
            if (shieldEffect.TimeRemaining <= 0)
            {
                EntityManager.AddComponent<DestroyMeComponent>(effectEntity);
            }
        });

        //double shot pickup spawning
        if (activeDoubleShotEffects == 0 && activeDoubleShotPickups == 0)
        {
            int spawnEventCount = 0;

            //spawn a new shot pickup if due
            Entities.WithAll<SpawnDoubleShotPickupEventComponent>().ForEach((
                Entity eventEntity, ref SpawnDoubleShotPickupEventComponent spawnPickup) =>
            {
                spawnEventCount++;

                spawnPickup.TimeToSpawn -= Time.DeltaTime;
                if (spawnPickup.TimeToSpawn <= 0)
                {
                    EntityManager.AddComponent<DestroyMeComponent>(eventEntity);
                    SpawnNewShotPickupAtRandom();
                }
            });

            if (spawnEventCount == 0)
            {
                RaiseNewShotPickupSpawnEvent();
            }
        }

        //shield pickups
        Entities.WithAll<ShieldPickupComponent>().ForEach((
        Entity pickupEntity, ref ShieldPickupComponent shieldPickup, ref Translation pickupTranslation) =>
        {
            activeShieldPickups++;
            float3 position = pickupTranslation.Value;

            //collide with player            
            Entities.WithAll<PlayerComponent>().ForEach((
                ref Translation playerTranslation) =>
            {
                float distance = math.distance(position, playerTranslation.Value);
                if(distance < 1)
                {
                    SpawnShieldEffect();
                    EntityManager.AddComponent(pickupEntity, typeof(DestroyMeComponent));
                }                
            });

            //expire
            shieldPickup.TimeRemaining -= Time.DeltaTime;
            if(shieldPickup.TimeRemaining <= 0)
            {
                EntityManager.AddComponent<DestroyMeComponent>(pickupEntity);
            }          
        });

        //shield effects
        Entities.WithAll<ShieldEffectComponent>().ForEach((
        Entity effectEntity, ref ShieldEffectComponent shieldEffect) =>
        {
            activeShieldEffects++;
            
            //follow the player            
            Entities.WithAll<PlayerComponent>().ForEach((
                ref Translation playerTranslation) =>
            {
                EntityManager.SetComponentData(effectEntity, new Translation { Value = playerTranslation.Value });
            });

            shieldEffect.TimeRemaining -= Time.DeltaTime;
            if(shieldEffect.TimeRemaining <= 0)
            {
                EntityManager.AddComponent<DestroyMeComponent>(effectEntity);
            }
        });

        //a new shield pickup needs to be spawned
        if (activeShieldEffects == 0 && activeShieldPickups == 0)
        {
            int spawnEventCount = 0;

            //spawn a new shield pickup if due
            Entities.WithAll<SpawnShieldEventComponent>().ForEach((
                Entity eventEntity, ref SpawnShieldEventComponent spawnShield) =>
            {
                spawnEventCount++;

                spawnShield.TimeToSpawn -= Time.DeltaTime;
                if (spawnShield.TimeToSpawn <= 0)
                {
                    EntityManager.AddComponent<DestroyMeComponent>(eventEntity);
                    SpawnNewShieldPickupAtRandom();
                }
            });
         
            if (spawnEventCount == 0)
            {
                RaiseNewShieldPickupSpawnEvent();
            }
        }
    }

    private void SpawnShotEffect()
    {
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();
        PowerupSettingsComponent powerupSettings = GetSingleton<PowerupSettingsComponent>();

        Entity eventEntity = EntityManager.CreateEntity(typeof(DoubleShotEffectComponent ));
        EntityManager.SetComponentData(eventEntity, new DoubleShotEffectComponent { TimeRemaining = powerupSettings.DoubleShotEffectDuration });
    }

    private void RaiseNewShotPickupSpawnEvent()
    {
        PowerupSettingsComponent powerupSettings = GetSingleton<PowerupSettingsComponent>();
        float timeToSpawn = UnityEngine.Random.Range(powerupSettings.MinTimeToSpawnDoubleShotPickup, powerupSettings.MaxTimeToSpawnDoubleShotPickup);

        Entity eventEntity = EntityManager.CreateEntity(typeof(SpawnDoubleShotPickupEventComponent));
        EntityManager.AddComponentData(eventEntity, new SpawnDoubleShotPickupEventComponent
        {
            TimeToSpawn = timeToSpawn
        });
    }

    private void SpawnNewShotPickupAtRandom()
    {
        PowerupSettingsComponent powerupSettings = GetSingleton<PowerupSettingsComponent>();
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();
        GameSettingsComponent gameSettings = GetSingleton<GameSettingsComponent>();

        float3 position = float3.zero;
        position.x = UnityEngine.Random.Range(-gameSettings.ScreenWidth / 2 + 1, gameSettings.ScreenWidth / 2 - 1);
        position.y = UnityEngine.Random.Range(-gameSettings.ScreenHeight / 2 + 1, gameSettings.ScreenHeight / 2 - 1);

        Entity pickupEntity = EntityManager.Instantiate(prefabLoader.DoubleShotPickupPrefabEntity);
        EntityManager.SetComponentData(pickupEntity, new DoubleShotPickupComponent { TimeRemaining = powerupSettings.DoubleShotPickupDuration });
        EntityManager.SetComponentData(pickupEntity, new Translation { Value = position });
    }

    private void SpawnShieldEffect()
    {
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();
        PowerupSettingsComponent powerupSettings = GetSingleton<PowerupSettingsComponent>();

        Entity effectEntity = EntityManager.Instantiate(prefabLoader.ShieldEffectPrefabEntity);
        EntityManager.SetComponentData(effectEntity, new ShieldEffectComponent { TimeRemaining = powerupSettings.ShieldEffectDuration});
    }

    private void SpawnNewShieldPickupAtRandom()
    {
        PowerupSettingsComponent powerupSettings = GetSingleton<PowerupSettingsComponent>();
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();
        GameSettingsComponent gameSettings = GetSingleton<GameSettingsComponent>();

        float3 position = float3.zero;
        position.x = UnityEngine.Random.Range( -gameSettings.ScreenWidth /2 + 1, gameSettings.ScreenWidth / 2 - 1);
        position.y = UnityEngine.Random.Range(-gameSettings.ScreenHeight / 2 + 1, gameSettings.ScreenHeight / 2 - 1);

        Entity pickupEntity = EntityManager.Instantiate(prefabLoader.ShieldPickupPrefabEntity);
        EntityManager.SetComponentData(pickupEntity, new ShieldPickupComponent { TimeRemaining = powerupSettings.ShieldPickupDuration});
        EntityManager.SetComponentData(pickupEntity, new Translation { Value = position });
    }

    private void RaiseNewShieldPickupSpawnEvent()
    {
        PowerupSettingsComponent powerupSettings = GetSingleton<PowerupSettingsComponent>();
        float timeToSpawn = UnityEngine.Random.Range(powerupSettings.MinTimeToSpawnShieldPickup, powerupSettings.MaxTimeToSpawnShieldPickup);

        Entity eventEntity = EntityManager.CreateEntity(typeof(SpawnShieldEventComponent));
        EntityManager.AddComponentData(eventEntity, new SpawnShieldEventComponent
        {
            TimeToSpawn = timeToSpawn
        });
    }
}
