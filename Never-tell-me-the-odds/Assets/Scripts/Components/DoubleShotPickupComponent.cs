using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct DoubleShotPickupComponent : IComponentData
{
    public float TimeRemaining;
}