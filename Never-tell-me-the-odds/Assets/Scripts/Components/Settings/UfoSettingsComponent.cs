using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct UfoSettingsComponent : IComponentData
{
    //UFOs
    public float UfoSpeedByTypeBig;
    public float UfoSpeedByTypeSmall;
    public float UfoScaleByTypeBig;
    public float UfoScaleByTypeSmall;
    
    public float UfoSpawnTimeMin;
    public float UfoSpawnTimeMax;
    public float UfoSpawnSmallUfoChance;

    public float UfoFireTimeMin;
    public float UfoFireTimeMax;
    public float UfoFireRateByTypeBig;
    public float UfoFireRateByTypeSmall;
    public float UfoAimedFireAccuracy;
}