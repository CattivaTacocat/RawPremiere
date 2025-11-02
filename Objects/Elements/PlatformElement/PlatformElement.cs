using Godot;
using System;
using RawPremiere.Components.Elements;

public partial class PlatformElement : Node2D
{
    #region 组件
    [Export] public ElementInfoComp ElementInfo { get; private set; }
    [Export] public CartesianCoordinateComp CartesianCoordinate { get; private set; }
    [Export] public ScaleComp ScaleComp { get; private set; }
    [Export] public CompleteRotationComp CompleteRotation { get; private set; }
    [Export] public ShapeComp Shape { get; private set; }
    #endregion
}
