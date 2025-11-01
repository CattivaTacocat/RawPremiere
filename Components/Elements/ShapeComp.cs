using Godot;
using RawPremiere.Components.Enums;

namespace RawPremiere.Components.Elements;

public partial class ShapeComp : Node
{
    #region 创建
    public ShapeComp()
    {
        _shape.Set(ShapeEnum.Rectangle);
        _points.Set([]);
        _hollow.Set(0);
        _sectorStart.Set(0);
        _sectorAmount.Set(0);
        _collisionType.Set(CollisionTypeEnum.Platform);
        _priority.Set(0);
    }
    #endregion
    #region 属性
    [Notify,Export] public ShapeEnum Shape { get => _shape.Get(); set => _shape.Set(value); }
    [Notify,Export] public Vector2[] Points { get => _points.Get(); set => _points.Set(value); }
    [Notify,Export] public float Hollow { get => _hollow.Get(); set => _hollow.Set(value); }
    [Notify,Export] public float SectorStart { get => _sectorStart.Get(); set => _sectorStart.Set(value); }
    [Notify,Export] public float SectorAmount { get => _sectorAmount.Get(); set => _sectorAmount.Set(value); }
    [Notify,Export] public CollisionTypeEnum CollisionType { get => _collisionType.Get(); set => _collisionType.Set(value); }
    [Notify,Export] public int Priority { get => _priority.Get(); set => _priority.Set(value); }
    #endregion
}