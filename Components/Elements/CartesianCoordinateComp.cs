using Godot;

namespace RawPremiere.Components.Elements;

public partial class CartesianCoordinateComp : Node
{
    #region 属性
    [Notify,Export] public Vector2 Position { get => _position.Get(); set => _position.Set(value); }
    #endregion
}