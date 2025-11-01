using Godot;
using RawPremiere.Components.Enums;

namespace RawPremiere.Components.Elements;

public partial class EnvironmentComp : Node
{
    #region 创建
    public EnvironmentComp()
    {
        _brightness.Set(1);
        _contrast.Set(1);
        _saturation.Set(1);
        _glowBlendMode.Set(GlowBlendModeEnum.Screen);
        _highlightHue.Set(Colors.White);
        _shadowHue.Set(Colors.Black);
        _glowHue.Set(Colors.White);
        _intensity.Set(1);
        _strength.Set(1);
        _bloom.Set(0);
    }
    #endregion
    #region 属性
    [Notify,Export] public float Brightness { get => _brightness.Get(); set => _brightness.Set(value); }
    [Notify,Export] public float Contrast { get => _contrast.Get(); set => _contrast.Set(value); }
    [Notify,Export] public float Saturation { get => _saturation.Get(); set => _saturation.Set(value); }
    [Notify,Export] public GlowBlendModeEnum GlowBlendMode { get => _glowBlendMode.Get(); set => _glowBlendMode.Set(value); }
    [Notify,Export] public Color HighlightHue { get => _highlightHue.Get(); set => _highlightHue.Set(value); }
    [Notify,Export] public Color ShadowHue { get => _shadowHue.Get(); set => _shadowHue.Set(value); }
    [Notify,Export] public Color GlowHue { get => _glowHue.Get(); set => _glowHue.Set(value); }
    [Notify,Export] public float Intensity { get => _intensity.Get(); set => _intensity.Set(value); }
    [Notify,Export] public float Strength { get => _strength.Get(); set => _strength.Set(value); }
    [Notify,Export] public float Bloom { get => _bloom.Get(); set => _bloom.Set(value); }
    #endregion
}