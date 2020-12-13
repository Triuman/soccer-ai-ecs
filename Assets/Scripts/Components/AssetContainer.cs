using Unity.Entities;
using UnityEngine;

[GenerateAuthoringComponent]
public struct AssetContainer : IComponentData
{
    public Entity PlayerPrefab;
    public Entity BallPrefab;
    public Material RectMaterial;
}
