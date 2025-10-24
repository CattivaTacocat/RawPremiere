using Godot;
using System;
using RawPremiere.Temp.NCSTemp;

public partial class BulletEntity : Node2D
{
    #region 系统
    [Export] public BulletMovementSys MovementSys { get; private set; }
    #endregion
    #region 组件
    public MovementComp Movement = new(Vector2.Left, 200);
    [Export] public HurtComp Hurt;
    #endregion
    #region 创建
    public override void _Ready()
    {
        Hurt.Attack.CurrentAtk = 10;
        Hurt.Attack.CriticalRatio = 0.4f;
        Hurt.Attack.CriticalMultiplier = 1.5f;
    }
    #endregion
    #region 视图
    public override void _Process(double delta)
    {
        var dir = Movement.Direction;
        var speed = Movement.Speed;
        Position += dir * speed * (float)delta;
    }
    #endregion
}
