using Godot;

namespace RawPremiere.Components.Elements;

public partial class CanvasComp : Node
{
    #region 创建
    public CanvasComp()
    {
        _modulate.Set(Colors.White);
        _color.Set(Colors.White);
        _opacity.Set(1);
        _layerIndex.Set(0);
        _visible.Set(true);
    }
    #endregion
    #region 属性
    [Notify,Export] public Color Modulate {get => _modulate.Get(); set=>_modulate.Set(value);}
    [Notify,Export] public Color Color { get => _color.Get(); set=>_color.Set(value); }
    [Notify,Export] public float Opacity { get => _opacity.Get(); set=>_opacity.Set(value); }
    [Notify,Export] public int LayerIndex { get => _layerIndex.Get(); set=>_layerIndex.Set(value); }
    [Notify,Export] public bool Visible { get => _visible.Get(); set=>_visible.Set(value); }
    #endregion
}