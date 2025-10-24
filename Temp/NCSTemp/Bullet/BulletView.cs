using Godot;
using System;

public partial class BulletView : Sprite2D
{
    #region 实体
    [Export] public BulletEntity BulletEntity;
    #endregion
    #region 视图
    public override void _Process(double delta)
    {
        var dir = BulletEntity.Movement.Direction;
        var speed = BulletEntity.Movement.Speed;
        Position += dir * speed * (float)delta;
    }
    #endregion
}
