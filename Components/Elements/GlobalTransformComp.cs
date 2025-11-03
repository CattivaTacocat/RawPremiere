using Godot;

namespace RawPremiere.Components.Elements;

public partial class GlobalTransformComp : Node
{
    #region 属性
    [Notify,Export] public bool IsGlobal { get => _isGlobal.Get(); set => _isGlobal.Set(value); }
    #endregion
}