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

        SpawnPlayer();
        SpawnAsteroids(settings.NumStartingAsteroids, true, float3.zero, AsteroidComponent.AsteroidSize.BIG);             
    }      

    private void SpawnPlayer()
    {
        SettingsComponent settings = GetSingleton<SettingsComponent>();
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();

        float3 startPosition = new Unity.Mathematics.float3(
                    settings.PlayerStartPosition.x, settings.PlayerStartPosition.y, settings.PlayerStartPosition.z);        

        //Instantiate the player
        Entity player = EntityManager.Instantiate(prefabLoader.PlayerPrefabEntity);
        EntityManager.SetComponentData(player, new Translation { Value = startPosition });
        EntityManager.SetComponentData(player, new PlayerComponent { Heading = settings.PlayerStartFacing });
    }

    private void SpawnAsteroids(int numToSpawn, bool randomPosition, float3 position, AsteroidComponent.AsteroidSize size)
    {
        SettingsComponent settings = GetSingleton<SettingsComponent>();
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();

        for (int i =0; i<numToSpawn;i++)
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
        }
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
