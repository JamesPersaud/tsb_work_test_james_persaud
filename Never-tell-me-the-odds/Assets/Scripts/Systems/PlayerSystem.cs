using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class PlayerSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        PlayerSettingsComponent playerSettings = GetSingleton<PlayerSettingsComponent>();
        PlayerInputComponent input = GetSingleton<PlayerInputComponent>();

        Entities.WithAll<PlayerComponent>().ForEach((        
            ref PlayerComponent player,
            ref Rotation rotation,
            ref VelocityComponent velocity,
            ref AccelerationComponent acceleration,
            ref Translation translation) =>
        {
            //set and rotate the player to its current heading
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

            //if firing then create a new fire bullet event
            if(input.Firing)
            {
                float3 firingDirectionLocal = new float3(0, 1, 0);
                firingDirectionLocal = math.rotate(rotation.Value, firingDirectionLocal);

                BulletSettingsComponent bulletSettings = GetSingleton<BulletSettingsComponent>();
                float3 bulletVelocity = (firingDirectionLocal * bulletSettings.BulletBaseSpeed) + velocity.Velocity;
                float3 firingPoint = firingDirectionLocal * 0.5f + translation.Value;

                Entity eventEntity = EntityManager.CreateEntity(typeof(FireBulletEventComponent));
                EntityManager.AddComponentData(eventEntity, new FireBulletEventComponent { 
                    Position = firingPoint, Velocity = bulletVelocity, FiredByPlayer = true
                });
            }
            
            //friction modifies acceleration in the inverse direction to the current velocity
            if(math.length(velocity.Velocity) >0)
            {
                float3 frictionVector = math.normalize(-velocity.Velocity) * playerSettings.PlayerFriction;
                acceleration.Acceleration += frictionVector;
            }

            //velocity bounds
            if(math.length(velocity.Velocity) <0)
            {
                velocity.Velocity = float3.zero;
            }

            if (math.length(velocity.Velocity) > playerSettings.MaxPlayerSpeed)
            {
                velocity.Velocity = math.normalize(velocity.Velocity) * playerSettings.MaxPlayerSpeed;
            }
        });
    }
}
