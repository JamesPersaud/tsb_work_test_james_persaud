using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct InputSettingsComponent : IComponentData
{   
    //input handling
    public KeyCode TurnLeftButton;
    public KeyCode TurnRightButton;
    public KeyCode ThrustButton;
    public KeyCode FireButton;
    public KeyCode HyperspaceButton;
    public float TurnIncrement;
    public float TurnButtonRepeatSpeed;
}