using Godot;

namespace RawPremiere.Components.Elements;

public partial class CompleteRotationComp : Node
{
    #region 属性
    [Notify,Export] public float Rotation { get => _rotation.Get(); set=>_rotation.Set(value); }
    [Notify,Export] public Vector2 Skew { get => _skew.Get(); set=>_skew.Set(value); }
    [Notify,Export] public Vector2 Pivot { get => _pivot.Get(); set=>_pivot.Set(value); }
    #endregion
}