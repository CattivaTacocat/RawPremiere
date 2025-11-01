using System.Collections.Generic;
using Godot;
using RawPremiere.Objects.Commands;

namespace RawPremiere.Components.Elements;

public partial class PageComp : Node
{
    #region 创建
    public PageComp()
    {
        _spawnPoint.Set(Vector2.Zero);
        _timeScale.Set(1f);
        _palette.Set([]);
        _events.Set([]);
    }
    #endregion
    #region 属性
    [Notify,Export] public Vector2 SpawnPoint { get => _spawnPoint.Get(); set => _spawnPoint.Set(value); }
    [Notify,Export] public float TimeScale { get => _timeScale.Get(); set => _timeScale.Set(value); }
    [Notify] public Dictionary<string,Color> Palette { get => _palette.Get(); set => _palette.Set(value); }
    [Notify] public Dictionary<int,CommandSet> Events { get => _events.Get(); set => _events.Set(value); }
    #endregion
}