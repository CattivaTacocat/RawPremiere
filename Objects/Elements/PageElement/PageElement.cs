using Godot;
using System;
using DeadDog.Ordexp;
using DeadDog.RawPremiere.Standards;
using RawPremiere.Components.Elements;
using RawPremiere.Objects.Elements;
using Environment = Godot.Environment;

public partial class PageElement : CanvasLayer , IElement
{
    #region 组件
    [Notify,Export] public ElementInfoComp ElementInfoComp { get => _elementInfoComp.Get(); set => _elementInfoComp.Set(value); }
    [Notify,Export] public EnvironmentComp EnvironmentComp { get => _environmentComp.Get(); set => _environmentComp.Set(value); }
    [Notify,Export] public PageComp PageComp { get => _pageComp.Get(); set => _pageComp.Set(value); }
    [Notify,Export] public CartesianCoordinateComp CartesianCoordinateComp { get => _cartesianCoordinateComp.Get(); set => _cartesianCoordinateComp.Set(value); }
    [Notify,Export] public ScaleComp ScaleComp { get => _scaleComp.Get(); set => _scaleComp.Set(value); }
    [Notify,Export] public SimpleRotationComp SimpleRotationComp { get => _simpleRotationComp.Get(); set => _simpleRotationComp.Set(value); }
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitEvents();
    }

    private void InitEvents()
    {
        CartesianCoordinateComp.PositionChanged += OnPositionChanged;
        ScaleComp.ScaleChanged += OnScaleChanged;
        SimpleRotationComp.RotationChanged += OnRotationChanged;
    }
    #endregion
    #region 销毁
    public override void _ExitTree()
    {
        DestroyEvents();
    }

    private void DestroyEvents()
    {
        CartesianCoordinateComp.PositionChanged -= OnPositionChanged;
        ScaleComp.ScaleChanged -= OnScaleChanged;
        SimpleRotationComp.RotationChanged -= OnRotationChanged;
    }
    #endregion
    #region 响应
    private void OnPositionChanged()
    {
        Offset = CartesianCoordinateComp.Position
                 * GlobalUnit.UNIT_LENGTH;
    }
    
    private void OnScaleChanged()
    {
        Scale = ScaleComp.Scale;
    }
    
    private void OnRotationChanged()
    {
        Rotation = SimpleRotationComp.Rotation
            .DegToRad();
    }
    #endregion
}
