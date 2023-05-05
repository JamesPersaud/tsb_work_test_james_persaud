using Unity.Entities;

[GenerateAuthoringComponent]
public struct PrefabLoaderComponent : IComponentData
{
    public Entity PlayerPrefabEntity;
}