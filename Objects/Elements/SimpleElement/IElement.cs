using Godot;
using RawPremiere.Components.Elements;

namespace RawPremiere.Objects.Elements;

public interface IElement
{
    #region 组件
    ElementInfoComp ElementInfoComp { get; }
    #endregion
}