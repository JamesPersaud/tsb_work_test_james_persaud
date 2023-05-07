using Unity.Entities;
using Unity.Mathematics;

[GenerateAuthoringComponent]
public struct UfoComponent : IComponentData
{
    public enum UfoType
    {
        BIG_AND_DUMB,
        SMALL_AND_VIOLENT
    }

    public UfoType Type;
    public float TimeToFire;
}