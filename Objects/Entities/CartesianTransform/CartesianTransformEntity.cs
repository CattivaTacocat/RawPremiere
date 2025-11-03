using Godot;
using System;
using DeadDog.Nodeo.Tools;
using DeadDog.Ordexp;
using DeadDog.RawPremiere.Standards;
using RawPremiere.Components.Elements;

public partial class CartesianTransformEntity : Node2D
{
    #region 组件
    [Notify,Export] public CartesianCoordinateComp CartesianCoordinateComp { get => _cartesianCoordinateComp.Get(); set => _cartesianCoordinateComp.Set(value); }
    [Notify,Export] public ScaleComp ScaleComp { get => _scaleComp.Get(); set => _scaleComp.Set(value); }
    [Notify,Export] public CompleteRotationComp CompleteRotationComp { get => _completeRotationComp.Get(); set => _completeRotationComp.Set(value); }
    [Notify,Export] public GlobalTransformComp GlobalTransformComp { get => _globalTransformComp.Get(); set => _globalTransformComp.Set(value); }
    #endregion
    #region 节点
    [Notify,Export] public Node2D ChildTransform { get => _childTransform.Get(); set => _childTransform.Set(value); }
    #endregion
    #region 创建
    public override void _Ready()
    { 
        InitEvents();
        RespondAll();
    }
    
    private void InitEvents()
    {
        CartesianCoordinateComp.PositionChanged += OnTransformChanged;
        ScaleComp.ScaleChanged += OnTransformChanged;
        CompleteRotationComp.RotationChanged += OnTransformChanged;
        CompleteRotationComp.PivotChanged += OnPivotChanged;
        CartesianCoordinateCompChanged += RespondAll;
        ScaleCompChanged += RespondAll;
        CompleteRotationCompChanged += RespondAll;
    }
    #endregion
    #region 销毁
    public override void _ExitTree()
    {
        DestroyEvents();
    }
    
    private void DestroyEvents()
    {
        CartesianCoordinateComp.PositionChanged -= OnTransformChanged;
        ScaleComp.ScaleChanged -= OnTransformChanged;
        CompleteRotationComp.RotationChanged -= OnTransformChanged;
        CompleteRotationComp.PivotChanged -= OnPivotChanged;
        CartesianCoordinateCompChanged -= RespondAll;
        ScaleCompChanged -= RespondAll;
        CompleteRotationCompChanged -= RespondAll;
    }
    #endregion
    #region 响应
    private void RespondAll()
    {
        OnTransformChanged();
        OnPivotChanged();
    }

    private void OnTransformChanged()
    {
        var t = Transform2D.Identity.Transform2DAs3DDeg(
            CompleteRotationComp.Rotation,
            CartesianCoordinateComp.Position * GlobalUnit.UNIT_LENGTH,
            ScaleComp.Scale
        );
        if (GlobalTransformComp.IsGlobal) GlobalTransform = t;
        else Transform = t;
    }
    
    private void OnPivotChanged()
    {
        if (IsInstanceValid(ChildTransform))
            ChildTransform.Position = CompleteRotationComp.Pivot * -GlobalUnit.UNIT_HALF_LENGTH;
    }
    #endregion
    #region 操作
    public void Update() => RespondAll();
    #endregion
}
