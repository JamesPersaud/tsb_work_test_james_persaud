using Unity.Entities;

[GenerateAuthoringComponent]
public struct BulletComponent : IComponentData
{
    public float Age;
    public bool FiredByPlayer;
}