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
        SettingsComponent settings = GetSingleton<SettingsComponent>();
        PlayerInputComponent input = GetSingleton<PlayerInputComponent>();

        PlayerInputComponent previousInputSate = input;

        input.Thrust = Input.GetKey(settings.ThrustButton);
        input.Left = Input.GetKey(settings.TurnLeftButton);
        input.Right = Input.GetKey(settings.TurnRightButton);
        input.Hyperspace = Input.GetKey(settings.HyperspaceButton);
        input.Fire = Input.GetKey(settings.FireButton);        

        //turning
        input.Turning = 0;
        if(input.Left || input.Right)
        {
            input.TurnKeyHoldDuration += Time.DeltaTime;
            if(input.TurnKeyHoldDuration >= settings.TurnButtonRepeatSpeed)
            {
                //trying to steer both ways at once results in nothing
                if(input.Left)
                {
                    input.Turning = settings.TurnIncrement;
                }
                else if(input.Right)
                {
                    input.Turning = -settings.TurnIncrement;
                }

                input.TurnKeyHoldDuration -= settings.TurnButtonRepeatSpeed;
            }
        }
        else
        {
            input.TurnKeyHoldDuration = 0;
        }

        //thrusting
        input.Thrusting = (input.Thrust) ? settings.ThrustAccelerationFactor : 0;

        //firing     
        input.Firing = input.Fire && !previousInputSate.Fire;      

        SetSingleton<PlayerInputComponent>(input);                    
    }
}
