using Unity.Entities;

[GenerateAuthoringComponent]
public struct AsteroidComponent : IComponentData
{
    public enum AsteroidSize
    {
        BIG,
        MEDIUM,
        SMALL
    }

    public AsteroidSize Size;
}