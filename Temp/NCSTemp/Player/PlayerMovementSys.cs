using Godot;
using System;
using RawPremiere.Temp.NCSTemp;

public partial class PlayerMovementSys : Node
{
    #region 实体
    [Export] public PlayerEntity Player { get; private set; }
    #endregion
    #region 操作
    public override void _Process(double delta)
    {
        var dirX = Input.GetAxis("ui_left", "ui_right");
        var dirY = Input.GetAxis("ui_up", "ui_down");
        Player.Movement = Player.Movement with { Direction = new(dirX, dirY) };
    }
    #endregion
}
