using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class PlayerSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        SettingsComponent settings = GetSingleton<SettingsComponent>();
        PlayerInputComponent input = GetSingleton<PlayerInputComponent>();

        Entities.WithAll<PlayerComponent>().ForEach((        
            ref PlayerComponent player,
            ref Rotation rotation,
            ref VelocityComponent velocity,
            ref AccelerationComponent acceleration) =>
        {
            player.Heading += input.Turning;

            if(player.Heading <0)
            {
                player.Heading += 360;
            }
            else if(player.Heading >360)
            {
                player.Heading -= 360;
            }
            rotation.Value = quaternion.Euler(0, 0, player.Heading * math.PI / 180f );

            //if thrusting then set acceleration to the heading direction
            acceleration.Acceleration = float3.zero;

            if (input.Thrusting != 0)
            {          
                acceleration.Acceleration = new float3(0,1,0);
                acceleration.Acceleration = math.rotate(rotation.Value, acceleration.Acceleration);
                acceleration.Acceleration *= input.Thrusting;
            }
            
            //friction modifies acceleration in the inverse direction to the current velocity
            if(math.length(velocity.Velocity) >0)
            {
                float3 frictionVector = math.normalize(-velocity.Velocity) * settings.PlayerFriction;
                acceleration.Acceleration += frictionVector;
            }

            if(math.length(velocity.Velocity) <0)
            {
                velocity.Velocity = float3.zero;               
            }

            if (math.length(velocity.Velocity) > settings.MaxPlayerSpeed)
            {
                velocity.Velocity = math.normalize(velocity.Velocity) * settings.MaxPlayerSpeed;
            }
        });
    }
}
