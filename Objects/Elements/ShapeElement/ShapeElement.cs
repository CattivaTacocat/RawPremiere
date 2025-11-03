using Godot;
using System;
using RawPremiere.Components;
using RawPremiere.Components.Elements;

public partial class ShapeElement : Node2D
{
    #region 组件
    [ExportGroup("组件")]
    [Export] public ElementInfoComp ElementInfo { get; private set; }
    [Notify,Export] public GlobalTransformComp GlobalTransformComp { get => _globalTransformComp.Get(); private set => _globalTransformComp.Set(value); }
    [Export] public HybridCoordinateDegComp HybridCoordinateComp { get; private set; }
    [Export] public ScaleComp ScaleComp { get; private set; }
    [Export] public CompleteRotationComp CompleteRotation { get; private set; }
    [Export] public ShapeComp Shape { get; private set; }
    #endregion
}
