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

            //is there a smart wrap function?
            float halfwidth = gameSettings.ScreenWidth / 2;
            float halfHeight = gameSettings.ScreenHeight / 2;

            if (finalPosition.x < -halfwidth) { finalPosition.x = halfwidth; }
            else if (finalPosition.x > halfwidth) {finalPosition.x = -halfwidth; }

            if (finalPosition.y < -halfHeight) { finalPosition.y = halfHeight; }
            else if (finalPosition.y > halfwidth) { finalPosition.y = -halfHeight; }

            translation.Value = finalPosition;
            velocity.Velocity = finalVelocity;

        });
    }
}
