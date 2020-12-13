using Unity.Entities;

public struct SelectedShapeComponent : IComponentData
{
    public SelectedShape Value;
}

public enum SelectedShape {
    Circle,
    Rect
}