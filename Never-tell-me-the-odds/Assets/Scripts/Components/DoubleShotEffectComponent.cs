using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct DoubleShotEffectComponent : IComponentData
{
    public float TimeRemaining;
}