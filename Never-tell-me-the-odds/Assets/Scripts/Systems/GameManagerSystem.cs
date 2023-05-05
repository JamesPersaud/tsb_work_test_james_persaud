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

        //Instantiate the player
        Entities.ForEach((ref PrefabLoaderComponent prefabLoader) =>
        {            
            Entity player = EntityManager.Instantiate(prefabLoader.PlayerPrefabEntity);
            EntityManager.SetComponentData(player, new Translation { Value = startPosition });
            EntityManager.SetComponentData(player, new PlayerComponent { Heading = settings.PlayerStartFacing });
        });
    }  

    protected override void OnUpdate()
    {
        
    }
}
