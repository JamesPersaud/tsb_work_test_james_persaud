using System;
using UnityEngine;
using Unity.Entities;
using Unity.Jobs;
using Unity.Mathematics;

/// <summary>
/// Before the movement system runs, this system needs to handle input that can affect the player entity
/// 
/// Thrust input
///     Gives the player acceleration
/// Turning left/right
///     Changes the player's heading
/// Hyperspace
///     Sets the player up for a hyperspace jump
/// Fire
///     Queue a new bullet for the bullet system if there aren't already too many 
///     
/// 
///     
/// 
/// </summary>
public class PlayerInputSystem : ComponentSystem
{               
    protected override void OnUpdate()
    {
        InputSettingsComponent inputSettings = GetSingleton<InputSettingsComponent>();
        PlayerSettingsComponent playerSettings = GetSingleton<PlayerSettingsComponent>();
        PlayerInputComponent input = GetSingleton<PlayerInputComponent>();

        PlayerInputComponent previousInputSate = input;

        input.Thrust = Input.GetKey(inputSettings.ThrustButton);
        input.Left = Input.GetKey(inputSettings.TurnLeftButton);
        input.Right = Input.GetKey(inputSettings.TurnRightButton);
        input.Hyperspace = Input.GetKey(inputSettings.HyperspaceButton);
        input.Fire = Input.GetKey(inputSettings.FireButton);        

        //turning
        input.Turning = 0;
        if(input.Left || input.Right)
        {
            input.TurnKeyHoldDuration += Time.DeltaTime;
            if(input.TurnKeyHoldDuration >= inputSettings.TurnButtonRepeatSpeed)
            {
                //trying to steer both ways at once results in nothing
                if(input.Left)
                {
                    input.Turning = inputSettings.TurnIncrement;
                }
                else if(input.Right)
                {
                    input.Turning = -inputSettings.TurnIncrement;
                }

                input.TurnKeyHoldDuration -= inputSettings.TurnButtonRepeatSpeed;
            }
        }
        else
        {
            input.TurnKeyHoldDuration = 0;
        }

        //thrusting
        input.Thrusting = (input.Thrust) ? playerSettings.ThrustAccelerationFactor : 0;

        //firing     
        input.Firing = input.Fire && !previousInputSate.Fire;      

        SetSingleton<PlayerInputComponent>(input);                    
    }
}
