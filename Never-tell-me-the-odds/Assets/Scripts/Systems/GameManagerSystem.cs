using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class GameManagerSystem : ComponentSystem
{
    //make a singleton?
    //implement game state?

    protected override void OnStartRunning()
    {
        SettingsComponent settings = GetSingleton<SettingsComponent>();
        float3 startPosition = new Unity.Mathematics.float3(
                    settings.PlayerStartPosition.x, settings.PlayerStartPosition.y, settings.PlayerStartPosition.z);
        
        Entities.ForEach((ref PrefabLoaderComponent prefabLoader) =>
        {
            //Instantiate the player
            Entity player = EntityManager.Instantiate(prefabLoader.PlayerPrefabEntity);
            EntityManager.SetComponentData(player, new Translation { Value = startPosition });
            EntityManager.SetComponentData(player, new PlayerComponent { Heading = settings.PlayerStartFacing });

            //Instantiate the starting asteroids
            for(int i =0; i < settings.NumStartingAsteroids; i++)
            {                
                float3 spawnPosition = GetRandomSpawnPosition(settings);
                float spawnSpeed = UnityEngine.Random.Range(settings.AsteroidSpeedMin,settings.AsteroidSpeedMax);
                float3 spawnVelocity = new float3(0, spawnSpeed, 0);
                spawnVelocity = math.rotate(quaternion.Euler(0, 0, UnityEngine.Random.Range(0, 2 * math.PI)), spawnVelocity);

                Entity asteroid = EntityManager.Instantiate(prefabLoader.AsteroidPrefabEntity);
                EntityManager.SetComponentData(asteroid, new Translation { Value = spawnPosition });
                EntityManager.SetComponentData(asteroid, new VelocityComponent { Velocity = spawnVelocity });
            }
        });        
    }     

    private float3 GetRandomSpawnPosition(SettingsComponent settings)
    {
        float3 position = float3.zero;

        while(position.x > settings.NospawnBoundsMin.x && position.x < settings.NospawnBoundsMax.x &&
            position.y > settings.NospawnBoundsMin.y && position.y < settings.NospawnBoundsMax.y)
        {
            position.x = UnityEngine.Random.Range(-settings.ScreenWidth / 2, settings.ScreenWidth / 2);
            position.x = UnityEngine.Random.Range(-settings.ScreenHeight / 2, settings.ScreenHeight/ 2);
        }

        return position;
    }

    protected override void OnUpdate()
    {
        
    }
}
