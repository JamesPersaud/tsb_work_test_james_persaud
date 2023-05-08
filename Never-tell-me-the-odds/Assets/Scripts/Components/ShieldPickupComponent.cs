using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct ShieldPickupComponent : IComponentData
{
    public float TimeRemaining;
}