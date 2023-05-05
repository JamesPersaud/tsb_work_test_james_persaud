using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class BulletSystem : ComponentSystem
{

    protected override void OnUpdate()
    {
        SettingsComponent settings = GetSingleton<SettingsComponent>();
        int count = 0;

        //kill any bullets that are too old
        Entities.WithAll<BulletComponent>().ForEach((
            Entity entity, ref BulletComponent bullet) =>
        {
            count++;

            bullet.Age += Time.DeltaTime;
            if (bullet.Age >= settings.MaxBulletAge)
            {
                EntityManager.DestroyEntity(entity);
            }
        });

        //spawn new bullets if the fire button has been pressed and there is room for more
        Entities.WithAll<FireBulletEventComponent>().ForEach((
            Entity entity, ref FireBulletEventComponent fireEvent) =>
        {
            if (count < settings.MaxBulletCount)
            {
                SpawnBullet(fireEvent.Position, fireEvent.Velocity);
                count++;
            }

            EntityManager.DestroyEntity(entity);
        });
    }

    private void SpawnBullet(float3 position, float3 velocity )
    {
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();
        Entity bullet = EntityManager.Instantiate(prefabLoader.BulletPrefabEntity);
        EntityManager.SetComponentData(bullet, new Translation { Value = position });
        EntityManager.SetComponentData(bullet, new VelocityComponent { Velocity = velocity });
    }
}
