using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct AccelerationComponent : IComponentData
{
    public float3 Acceleration;
}