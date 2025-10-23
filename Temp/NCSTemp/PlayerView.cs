using Godot;
using System;
using RawPremiere.Temp.NCSTemp;

public partial class PlayerView : Node2D
{
    #region 辅助字段
    private Vector2 _dir;
    private float _speed;
    #endregion
    #region 系统
    [Export] public PlayerSys Sys { get; private set; }
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitSys();
    }

    private void InitSys()
    {
        Sys.OnMove += RespondMove;
    }
    #endregion
    #region 销毁
    public override void _ExitTree()
    {
        DestroySys();
    }
    
    private void DestroySys()
    {
        Sys.OnMove -= RespondMove;
        Sys.QueueFree();
    }
    #endregion
    #region 响应
    private void RespondMove(MovementComp movement)
    {
        _dir = movement.Direction;
        _speed = movement.Speed;
    }
    #endregion
    #region 视图
    public override void _Process(double delta)
    {
        Position += _dir * _speed * (float)delta;
    }
    #endregion
}
