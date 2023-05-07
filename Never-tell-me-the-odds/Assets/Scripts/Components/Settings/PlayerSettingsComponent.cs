using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PlayerSettingsComponent : IComponentData
{   
    //player movement
    public float PlayerFriction;
    public float MaxPlayerSpeed;
    public float3 PlayerStartPosition;
    public float PlayerStartFacing;
    public float ThrustAccelerationFactor;
}