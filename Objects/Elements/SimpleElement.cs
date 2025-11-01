using Godot;
using RawPremiere.Components.Elements;

namespace RawPremiere.Objects.Elements;

public partial class SimpleElement : Node
{
    #region 组件
    [Notify,Export] public ElementInfoComp ElementInfoComp { get; private set; }
    #endregion
}