using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[GenerateAuthoringComponent]
public struct BulletSettingsComponent : IComponentData
{
    //bullets
    public float MaxBulletAge;
    public int MaxBulletCount;
    public float BulletBaseSpeed;
}