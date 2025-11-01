using DeadDog.Nodeo.Structures;
using Godot;

namespace RawPremiere.Components.Elements;

public partial class PolarCoordinateComp : Node
{
    #region 属性
    [Notify] public Polar2 Position { get => _position.Get(); set => _position.Set(value); }
    #endregion
}