using System;
using Godot;

namespace RawPremiere.Components.Elements;

public partial class ElementComp : Node
{
    #region 属性
    [Notify][Export] public Node RelatedNode { get => _relatedNode.Get(); set => _relatedNode.Set(value); }
    [Notify][Export] public string ElementName { get => _elementName.Get(); set => _elementName.Set(value); }
    [Notify][Export] public Texture2D Icon { get => _icon.Get(); set=>_icon.Set(value); }
    #endregion
}