using Unity.Entities;

[GenerateAuthoringComponent]
public struct PrefabContainer : IComponentData
{
    public Entity PlayerPrefab;
    public Entity BallPrefab;
}
