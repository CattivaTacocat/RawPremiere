using Godot;
using System;
using RawPremiere.Temp.NCSTemp;

public partial class PlayerView : Node2D
{
    #region 辅助字段
    private Vector2 _dir;
    private float _speed;
    #endregion
    #region 实体
    [Export] public PlayerEntity Player { get; private set; }
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitSys();
    }

    private void InitSys()
    {
        Player.OnMove += RespondMove;
        Player.OnHurt += RespondHurt;
        Player.OnHealing += RespondHealing;
    }
    #endregion
    #region 销毁
    public override void _ExitTree()
    {
        DestroySys();
    }
    
    private void DestroySys()
    {
        Player.OnMove -= RespondMove;
    }
    #endregion
    #region 响应
    private void RespondMove(MovementComp movement)
    {
        _dir = movement.Direction;
        _speed = movement.Speed;
    }

    private void RespondHurt(HealthComp health)
    {
        GD.Print($"血条变成{health}");
        GD.Print($"播放受伤动画");
    }

    private void RespondHealing(HealthComp health)
    {
        GD.Print($"血条变成{health}");
        GD.Print($"播放治愈动画");
    }
    #endregion
    #region 视图
    public override void _Process(double delta)
    {
        Position += _dir * _speed * (float)delta;
    }
    #endregion
}
