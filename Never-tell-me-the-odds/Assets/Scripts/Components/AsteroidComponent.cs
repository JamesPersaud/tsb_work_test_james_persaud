using Unity.Entities;

[GenerateAuthoringComponent]
public struct AsteroidComponent : IComponentData
{
    public enum AsteroidSize
    {
        SMALL, MEDIUM, BIG
    }

    public AsteroidSize Size;
}