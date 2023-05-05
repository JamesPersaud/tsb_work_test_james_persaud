using Unity.Entities;

[GenerateAuthoringComponent]
public struct PrefabLoaderComponent : IComponentData
{
    public Entity PlayerPrefabEntity;
    public Entity AsteroidPrefabEntity;
    public Entity BulletPrefabEntity;
}