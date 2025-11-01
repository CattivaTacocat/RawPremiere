using Godot;
using RawPremiere.Components.Enums;
using RawPremiere.Objects.Appearance;
using RawPremiere.Objects.Appearance.DeathEffects;
using RawPremiere.Objects.Appearance.TrailEffects;

namespace RawPremiere.Components.Elements;

public partial class PlayerComp : Node
{
    #region 创建
    
    #endregion
    #region 属性
    [Notify,Export] public PlayerStateEnum State { get => _state.Get(); set => _state.Set(value); }
    [Notify,Export] public float Speed { get => _speed.Get(); set => _speed.Set(value); }
    [Notify,Export] public Vector2 Force { get => _force.Get(); set => _force.Set(value); }
    [Notify,Export] public int Health { get => _health.Get(); set => _health.Set(value); }
    [Notify,Export] public int Opportunities { get => _opportunities.Get(); set => _opportunities.Set(value); }
    [Notify,Export] public SimplePlayerSkin Skin { get => _skin.Get(); set => _skin.Set(value); }
    [Notify,Export] public SimpleTrailEffect TrailEffect { get => _trailEffect.Get(); set => _trailEffect.Set(value); }
    [Notify,Export] public SimpleDeathEffect DeathEffect { get => _deathEffect.Get(); set => _deathEffect.Set(value); }
    #endregion
}