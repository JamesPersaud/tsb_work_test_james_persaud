using Unity.Entities;
using Unity.Mathematics;

public struct AsteroidSpawnEventComponent : IComponentData
{
    public int NumToSpawn;
    public bool RandomPositions;
    public float3 Position;
    public AsteroidComponent.AsteroidSize Size;
}