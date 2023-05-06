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
        SpawnInitialAsteroids();
    }

    protected override void OnUpdate()
    {
        EntityUpkeep();
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

    private void SpawnInitialAsteroids()
    {
        SettingsComponent settings = GetSingleton<SettingsComponent>();

        Entity eventEntity = EntityManager.CreateEntity(typeof(AsteroidSpawnEventComponent));
        EntityManager.AddComponentData(eventEntity, new AsteroidSpawnEventComponent { 
            NumToSpawn = settings.NumStartingAsteroids, RandomPositions = true, 
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
