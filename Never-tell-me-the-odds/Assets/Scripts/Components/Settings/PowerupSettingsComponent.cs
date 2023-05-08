using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct PowerupSettingsComponent : IComponentData
{
    public float MinTimeToSpawnShieldPickup;
    public float MaxTimeToSpawnShieldPickup;
    public float ShieldPickupDuration;
    public float ShieldEffectDuration;

    public float MinTimeToSpawnDoubleShotPickup;
    public float MaxTimeToSpawnDoubleShotPickup;
    public float DoubleShotPickupDuration;
    public float DoubleShotEffectDuration;
}