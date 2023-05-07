using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using Unity.Mathematics;

public class MovementSystem : ComponentSystem
{
    protected override void OnUpdate()
    {
        GameSettingsComponent gameSettings = GetSingleton<GameSettingsComponent>();        

        Entities.WithAll<VelocityComponent, AccelerationComponent>().ForEach((
            Entity movementEntity,
            ref AccelerationComponent acceleration,
            ref VelocityComponent velocity,
            ref Translation translation) =>
        {
            float3 finalVelocity = velocity.Velocity;
            float3 finalPosition = translation.Value;

            if (math.length(acceleration.Acceleration) != 0)
            {
                finalVelocity += acceleration.Acceleration * Time.DeltaTime;
            }

            finalPosition += velocity.Velocity * Time.DeltaTime;                                                    

            float halfwidth = gameSettings.ScreenWidth / 2;
            float halfHeight = gameSettings.ScreenHeight / 2;

            //the ufo just flies off the end of the screen
            if (!EntityManager.HasComponent<UfoComponent>(movementEntity))
            {
                if (finalPosition.x < -halfwidth) { finalPosition.x = halfwidth; }
                else if (finalPosition.x > halfwidth) { finalPosition.x = -halfwidth; }
            }

            if (finalPosition.y < -halfHeight) { finalPosition.y = halfHeight; }
            else if (finalPosition.y > halfwidth) { finalPosition.y = -halfHeight; }

            translation.Value = finalPosition;
            velocity.Velocity = finalVelocity;

        });
    }
}
