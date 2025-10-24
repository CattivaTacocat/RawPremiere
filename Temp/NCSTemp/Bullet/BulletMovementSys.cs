using Godot;
using System;

public partial class BulletMovementSys : Node
{
    #region 实体
    [Export] public BulletEntity Bullet { get; private set; }
    #endregion
    #region 操作
    public void InitVelocity(Vector2 dir, float speed)
    {
        Bullet.Movement = new(dir, speed);
    }
    #endregion
}
