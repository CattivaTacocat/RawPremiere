using Godot;
using RawPremiere.Components.Elements;

namespace RawPremiere.Objects.Elements;

public partial class SimpleElement : Node
{
    #region 组件
    [Notify,Export] public ElementComp ElementComp { get; private set; }
    #endregion
}