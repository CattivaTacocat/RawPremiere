using Godot;
using System;
using DeadDog.Nodeo.Structures;
using DeadDog.Nodeo.Tools;
using DeadDog.Ordexp;
using DeadDog.RawPremiere.Standards;
using RawPremiere.Components;
using RawPremiere.Components.Elements;

public partial class HybridTransformEntity : Node2D
{
    #region 组件
    [Notify,Export] public HybridCoordinateDegComp HybridCoordinateComp { get => _hybridCoordinateComp.Get(); set => _hybridCoordinateComp.Set(value); }
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
        HybridCoordinateComp.ParamsChanged += OnTransformChanged;
        HybridCoordinateComp.IsPolarChanged += OnTransformChanged;
        ScaleComp.ScaleChanged += OnTransformChanged;
        CompleteRotationComp.RotationChanged += OnTransformChanged;
        CompleteRotationComp.PivotChanged += OnPivotChanged;
        GlobalTransformComp.IsGlobalChanged += OnTransformChanged;
        ChildTransformChanged += OnPivotChanged;
        HybridCoordinateCompChanged += RespondAll;
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
        HybridCoordinateComp.ParamsChanged -= OnTransformChanged;
        HybridCoordinateComp.IsPolarChanged -= OnTransformChanged;
        ScaleComp.ScaleChanged -= OnTransformChanged;
        CompleteRotationComp.RotationChanged -= OnTransformChanged;
        CompleteRotationComp.PivotChanged -= OnPivotChanged;
        GlobalTransformComp.IsGlobalChanged -= OnTransformChanged;
        ChildTransformChanged -= OnPivotChanged;
        HybridCoordinateCompChanged -= RespondAll;
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
            HybridCoordinateComp.Position * GlobalUnit.UNIT_LENGTH,
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
