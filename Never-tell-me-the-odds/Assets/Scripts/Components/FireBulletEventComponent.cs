using Unity.Entities;
using Unity.Mathematics;

public struct FireBulletEventComponent : IComponentData
{
    public float3 Position;
    public float3 Velocity;
}