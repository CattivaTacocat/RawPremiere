using Godot;
using System;
using RawPremiere.Temp.NCSTemp;

public partial class PlayerSys : Node
{
    #region 组件
    private MovementComp _movement = new();
    public MovementComp Movement
    {
        get => _movement;
        set
        {
            if (_movement.Equals(value)) return;
            _movement = value;
            OnMove?.Invoke(_movement);
        }
    }
    public HealthComp Health { get; set; } = new();
    #endregion
    #region 事件
    public event Action<MovementComp> OnMove;
    public event Action<HealthComp> OnHurt;
    #endregion
    #region 操作
    public override void _Process(double delta)
    {
        var dirX = Input.GetAxis("ui_left", "ui_right");
        var dirY = Input.GetAxis("ui_up", "ui_down");
        Movement = Movement with { Direction = new(dirX, dirY) };
    }
    #endregion
}
