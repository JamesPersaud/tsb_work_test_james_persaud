using Unity.Entities;
using Unity.Mathematics;

public struct SpawnUfoEventComponent : IComponentData
{ 
    public UfoComponent.UfoType Type;
    public float TimeToSpawn;
}