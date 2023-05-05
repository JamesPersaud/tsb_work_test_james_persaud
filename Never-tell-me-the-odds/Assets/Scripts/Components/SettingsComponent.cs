using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct SettingsComponent : IComponentData
{
    public const float THE_ODDS = 0.0002688172f; //approximately 3720 to one.

    /*
     *  "Never tell me the odds"
     *          -- Han Solo
     * 
     *  This is going to be as faithful an implementation of the original mechanics as possible given
     *  the notes at https://www.retrogamedeconstructionzone.com/2019/10/asteroids-by-numbers.html and the
     *  available time (10-20 hours)
     * 
     *  The standard unit of measure of distance in asteroids is player ship lengths
     *  The original screen is about 25/36
     *  The player's max speed is 17/s
     *  Bullets travel at 17 + ship's velocity (this is how mine laying works)
     *  
     *  According to the docs, Asteroids effective resolution is 1024x768 and it allegedly corresponds to 25/36 ship lengths.
     *  Are the x and y backwards here?
     *  
     *  All talk of pixels is an approximation since the original game is vector based (which is very cool)
     *  Adopting 32 pixels per unit means we can have a 32x32 ship sprite (plenty of pixels, not that this is a raster based game)
     *  and the screen will be 32/24 ship lengths - which is close enough to the original
     *  (so the orthographic size of the camera should be screen.height / 32 / 2)
     *  
     *  in classic asteroids the player turns in 5 degree increments
     *  for responsiveness we want any press of a turn button to result in at least a 5 degree turn
     *  when holding down a turn button we need the concept of a timer for repeats so we can control the turning speed
     *  
     *  If a full turn can be done in 1 second, we need to allow 72 repeats per second so the cool-down is approx 0.014s
     *  which seems fast but prevents very silly things from happening at super high frame rates.
     *  
     */

    //This class primarily exists to have a place to set global constants and data for the spawning of entities using
    //a classic unity game object

    //screen size
    public float ScreenWidth;
    public float ScreenHeight;

    //player movement
    public float PlayerFriction;
    public float MaxPlayerSpeed;
    public float3 PlayerStartPosition;
    public float PlayerStartFacing;
    public float ThrustAccelerationFactor;

    //input handling
    public KeyCode TurnLeftButton;
    public KeyCode TurnRightButton;
    public KeyCode ThrustButton;
    public KeyCode FireButton;
    public KeyCode HyperspaceButton;
    public float TurnIncrement;
    public float TurnButtonRepeatSpeed;

    
}