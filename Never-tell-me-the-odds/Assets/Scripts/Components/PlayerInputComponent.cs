using Unity.Entities;

[GenerateAuthoringComponent]
public struct PlayerInputComponent : IComponentData
{
    //key states
    public bool Thrust;
    public bool Left;
    public bool Right;
    public bool Hyperspace;
    public bool Fire;

    //turning
    public float Turning;
    public float TurnKeyHoldDuration;

    //thrusting
    public float Thrusting;
}