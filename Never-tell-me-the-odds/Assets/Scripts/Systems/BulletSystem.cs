using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class BulletSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        BulletSettingsComponent bullletSettings = GetSingleton<BulletSettingsComponent>();
        int count = 0;

        //kill any bullets that are too old
        Entities.WithAll<BulletComponent>().ForEach((
            Entity entity, ref BulletComponent bullet) =>
        {
            if (bullet.FiredByPlayer)
            {
                count++;
            }

            bullet.Age += Time.DeltaTime;
            if (bullet.Age >= bullletSettings.MaxBulletAge)
            {
                EntityManager.AddComponent<DestroyMeComponent>(entity);
            }
        });

        //spawn new bullets
        Entities.WithAll<FireBulletEventComponent>().ForEach((
            Entity entity, ref FireBulletEventComponent fireEvent) =>
        {  
            if (!fireEvent.FiredByPlayer || count < bullletSettings.MaxBulletCount)
            {
                SpawnBullet(fireEvent.Position, fireEvent.Velocity, true);
                count += (fireEvent.FiredByPlayer) ? 1 : 0;
            }

            EntityManager.AddComponent<DestroyMeComponent>(entity);
        });
    }

    private void SpawnBullet(float3 position, float3 velocity, bool firedByPlayer )
    {
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();
        Entity bullet = EntityManager.Instantiate(prefabLoader.BulletPrefabEntity);
        EntityManager.SetComponentData(bullet, new Translation { Value = position });
        EntityManager.SetComponentData(bullet, new VelocityComponent { Velocity = velocity });
        EntityManager.SetComponentData(bullet, new BulletComponent { Age = 0, FiredByPlayer = firedByPlayer });
    }
}
