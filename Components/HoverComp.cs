using Godot;

namespace RawPremiere.Components;

public partial class HoverComp : Node
{
    #region 属性
    [Notify,Export] public bool IsHovered { get => _isHovered.Get(); set => _isHovered.Set(value); }
    #endregion
}