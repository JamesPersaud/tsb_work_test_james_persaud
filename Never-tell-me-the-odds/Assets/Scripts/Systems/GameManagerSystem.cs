using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class GameManagerSystem : ComponentSystem
{
    //make a singleton?
    //implement game state?

    protected override void OnStartRunning()
    {             
        SpawnPlayer();
        SpawnInitialAsteroids();
    }

    protected override void OnUpdate()
    {
        EntityUpkeep();
    }

    private void SpawnPlayer()
    {
        PlayerSettingsComponent playerSettings = GetSingleton<PlayerSettingsComponent>();
        PrefabLoaderComponent prefabLoader = GetSingleton<PrefabLoaderComponent>();

        float3 startPosition = playerSettings.PlayerStartPosition;

        //Instantiate the player
        Entity player = EntityManager.Instantiate(prefabLoader.PlayerPrefabEntity);
        EntityManager.SetComponentData(player, new Translation { Value = startPosition });
        EntityManager.SetComponentData(player, new PlayerComponent { Heading = playerSettings.PlayerStartFacing });
    }

    private void SpawnInitialAsteroids()
    {
        AsteroidSettingsComponent asteroidSettings = GetSingleton<AsteroidSettingsComponent>();

        Entity eventEntity = EntityManager.CreateEntity(typeof(AsteroidSpawnEventComponent));
        EntityManager.AddComponentData(eventEntity, new AsteroidSpawnEventComponent { 
            NumToSpawn = asteroidSettings.NumStartingAsteroids, RandomPositions = true, 
            Position = float3.zero, Size = AsteroidComponent.AsteroidSize.BIG
        });        
    }    

    private void EntityUpkeep()
    {
        Entities.WithAll<DestroyMeComponent>().ForEach((
        Entity entity) =>
        {
            EntityManager.DestroyEntity(entity);
        });
    }
}
