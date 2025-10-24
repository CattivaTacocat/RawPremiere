using Godot;
using System;
using Lombok.NET;
using RawPremiere.Temp.NCSTemp;

public partial class PlayerEntity : Node2D
{
    #region 组件
    private MovementComp _movement = new(Vector2.Zero, 200);

    public MovementComp Movement
    {
        get => _movement;
        set
        {
            if (_movement.Equals(value)) return;
            _movement = value;
            OnMove?.Invoke(value);
        }
    }
    
    private HealthComp _health = new HealthComp(100,100);

    public HealthComp Health
    {
        get => _health;
        set
        {
            if (_health.Equals(value)) return;
            if (_health.CurrentHealth < value.CurrentHealth)
                OnHealing?.Invoke(value);
            else
                OnHurt?.Invoke(value);
            _health = value;
        }
    }
    #endregion
    #region 事件
    public event Action<MovementComp> OnMove;
    public event Action<HealthComp> OnHurt;
    public event Action<HealthComp> OnHealing;
    #endregion
}
