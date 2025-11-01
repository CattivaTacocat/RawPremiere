using Godot;
using RawPremiere.Components.Enums;

namespace RawPremiere.Components.Elements;

public partial class TextComp : Node
{
    #region 创建
    public TextComp()
    {
        _content.Set(string.Empty);
        _font.Set(TextFontEnum.Clear);
        _visibilityRatio.Set(1);
        _outline.Set(0);
        _fillColor.Set(Colors.White);
        _outlineColor.Set(Colors.Black);
        _space.Set(Vector2.One);
    }
    #endregion
    #region 属性
    [Notify,Export] public string Content { get => _content.Get(); set => _content.Set(value); }
    [Notify,Export] public TextFontEnum Font { get => _font.Get(); set => _font.Set(value); }
    [Notify,Export] public float VisibilityRatio { get => _visibilityRatio.Get(); set => _visibilityRatio.Set(value); }
    [Notify,Export] public int Outline { get => _outline.Get(); set => _outline.Set(value); }
    [Notify,Export] public Color FillColor { get => _fillColor.Get(); set => _fillColor.Set(value); }
    [Notify,Export] public Color OutlineColor { get => _outlineColor.Get(); set => _outlineColor.Set(value); }
    [Notify,Export] public Vector2 Space { get => _space.Get(); set => _space.Set(value); }
    #endregion
}