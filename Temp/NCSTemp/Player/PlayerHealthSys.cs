using Godot;
using System;
using System.Linq;

public partial class PlayerHealthSys : Node
{
    #region 实体
    [Export] public PlayerEntity Player { get; private set; }
    #endregion
    #region 节点
    [Export] public Area2D Collision { get; private set; }
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitEvents();
    }

    private void InitEvents()
    {
        Collision.AreaEntered += Touch;
    }
    #endregion
    #region 操作
    public void Touch(Area2D area)
    {
        if (area.IsInGroup("hurt"))
        {
            Hurt(area as HurtComp);
        }
        else if (area.IsInGroup("healing"))
        {
            Healing();
        }
    }

    private void Hurt(HurtComp hurt)
    {
        var atk = (int)hurt.Attack.GetActualAtk();
        Player.Health = Player.Health with { CurrentHealth = Player.Health.CurrentHealth - atk };
    }

    private void Healing()
    {
        
    }
    #endregion
}
