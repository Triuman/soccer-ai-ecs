using Unity.Entities;

[GenerateAuthoringComponent]
public struct AssetContainer : IComponentData
{
    public Entity PlayerPrefab;
    public Entity BallPrefab;
    public Entity RectPrefab;
    public Entity CirclePrefab;
}
