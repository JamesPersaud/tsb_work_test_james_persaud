using Unity.Entities;

[GenerateAuthoringComponent]
public struct AsteroidComponent : IComponentData
{
    public enum AsteroidSize : int
    {
        BIG = 0,
        MEDIUM = 1,
        SMALL = 2
    }

    public AsteroidSize Size;
}