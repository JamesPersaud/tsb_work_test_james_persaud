using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct AsteroidSettingsComponent : IComponentData
{      
    //asteroids
    public int NumStartingAsteroids;
    //number of fragments to split into when shot 
    public int NumFragments;

    public float2 NospawnBoundsMin;
    public float2 NospawnBoundsMax;

    public float AsteroidSpeedMin;
    public float AsteroidSpeedMax;

    public float AsteroidScaleBySizeBig;
    public float AsteroidScaleBySizeMedium;
    public float AsteroidScaleBySizeSmall;

    public float AsteroidSpeedBySizeBig;
    public float AsteroidSpeedBySizeMedium;
    public float AsteroidSpeedBySizeSmall;
}