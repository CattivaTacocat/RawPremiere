using Godot;
using System;
using RawPremiere.Codes.Calculators;
using RawPremiere.Codes.Factories;
using RawPremiere.Components.Elements;
using RawPremiere.Components.Enums;

public partial class ShapeEntity : Polygon2D
{
    #region 组件
    [Notify,Export] public ShapeComp ShapeComp { get => _shapeComp.Get(); set => _shapeComp.Set(value); }
    #endregion
    #region 节点
    [Export] public CollisionPolygon2D Collision { get; private set; }
    [Export] public Area2D Area { get; private set; }
    [Export] public StaticBody2D StaticBody { get; private set; }
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitEvents();
        RespondAll();
    }

    private void InitEvents()
    {
        ShapeComp.ShapeChanged += OnShapeChanged;
        ShapeComp.PointsChanged += UpdateShape;
        ShapeComp.HollowChanged += UpdateShape;
        ShapeComp.SectorStartChanged += UpdateShape;
        ShapeComp.SectorAmountChanged += UpdateShape;
        ShapeComp.CollisionTypeChanged += OnCollisionTypeChanged;
        ShapeCompChanged += RespondAll;
    }
    #endregion
    #region 销毁
    public override void _ExitTree()
    {
        DestroyEvents();
    }
    
    private void DestroyEvents()
    {
        ShapeComp.ShapeChanged -= OnShapeChanged;
        ShapeComp.PointsChanged -= UpdateShape;
        ShapeComp.HollowChanged -= UpdateShape;
        ShapeComp.SectorStartChanged -= UpdateShape;
        ShapeComp.SectorAmountChanged -= UpdateShape;
        ShapeComp.CollisionTypeChanged -= OnCollisionTypeChanged;
        ShapeCompChanged -= RespondAll;
    }
    #endregion
    #region 响应
    private void RespondAll()
    {
        OnShapeChanged();
        UpdateShape();
        OnCollisionTypeChanged();
    }

    private void OnShapeChanged()
    {
        ShapeComp.Points = ShapeFactory.CreateShape(ShapeComp.Shape).ToArray();
    }

    private void OnCollisionTypeChanged()
    {
        Collision.SetDeferred("disabled", ShapeComp.CollisionType == CollisionTypeEnum.Neither);
        if (ShapeComp.CollisionType == CollisionTypeEnum.Wall)
            Collision.Reparent(StaticBody);
        else Collision.Reparent(Area);
    }
    #endregion
    #region 处理
    private void UpdateShape()
    {
        var points = ShapeComp.Points;
        if (ShapeComp.Shape != ShapeEnum.Arrow &&
            ShapeComp.Shape != ShapeEnum.Mucro &&
            ShapeComp.Shape != ShapeEnum.Unknown &&
            ShapeComp.Shape != ShapeEnum.Custom)
            points = ShapeCalculator.ClipHollow(points, ShapeComp.Hollow);
        points = ShapeCalculator.ClipSector(points, ShapeComp.SectorStart, ShapeComp.SectorAmount);
        SetPoints(points);
    }

    private void SetPoints(Vector2[] points)
    {
        Polygon = points;
        Collision.Polygon = points;
    }
    #endregion
}
