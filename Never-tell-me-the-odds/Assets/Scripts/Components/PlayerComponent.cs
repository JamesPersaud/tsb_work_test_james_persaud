using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerComponent : IComponentData
{    
    //The angle that the player is facing
    public float Heading;    
} 