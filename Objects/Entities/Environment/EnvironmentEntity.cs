using Godot;
using System;
using RawPremiere.Components.Elements;
using RawPremiere.Components.Enums;
using Environment = Godot.Environment;

public partial class EnvironmentEntity : WorldEnvironment
{
    #region 组件
    [Notify,Export] public EnvironmentComp EnvironmentComp { get => _environmentComp.Get(); set => _environmentComp.Set(value); }
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitEvents();
    }

    private void InitEvents()
    {
        EnvironmentComp.BrightnessChanged += OnBrightnessChanged;
        EnvironmentComp.ContrastChanged += OnContrastChanged;
        EnvironmentComp.SaturationChanged += OnSaturationChanged;
        EnvironmentComp.GlowBlendModeChanged += OnGlowBlendModeChanged;
        EnvironmentComp.HighlightHueChanged += OnHighlightHueChanged;
        EnvironmentComp.ShadowHueChanged += OnShadowHueChanged;
        EnvironmentComp.GlowHueChanged += OnGlowHueChanged;
        EnvironmentComp.IntensityChanged += OnIntensityChanged;
        EnvironmentComp.StrengthChanged += OnStrengthChanged;
        EnvironmentComp.BloomChanged += OnBloomChanged;
        EnvironmentCompChanged += RespondAll;
    }
    #endregion
    #region 销毁
    public override void _ExitTree()
    {
        DestroyEvents();
    }
    
    private void DestroyEvents()
    {
        EnvironmentComp.BrightnessChanged -= OnBrightnessChanged;
        EnvironmentComp.ContrastChanged -= OnContrastChanged;
        EnvironmentComp.SaturationChanged -= OnSaturationChanged;
        EnvironmentComp.GlowBlendModeChanged -= OnGlowBlendModeChanged;
        EnvironmentComp.HighlightHueChanged -= OnHighlightHueChanged;
        EnvironmentComp.ShadowHueChanged -= OnShadowHueChanged;
        EnvironmentComp.GlowHueChanged -= OnGlowHueChanged;
        EnvironmentComp.IntensityChanged -= OnIntensityChanged;
        EnvironmentComp.StrengthChanged -= OnStrengthChanged;
        EnvironmentComp.BloomChanged -= OnBloomChanged;
        EnvironmentCompChanged -= RespondAll;
    }
    #endregion
    #region 响应
    private void RespondAll()
    {
        OnBrightnessChanged();
        OnContrastChanged();
        OnSaturationChanged();
        OnGlowBlendModeChanged();
        OnHighlightHueChanged();
        OnShadowHueChanged();
        OnGlowHueChanged();
        OnIntensityChanged();
        OnStrengthChanged();
        OnBloomChanged();
    }

    private void OnBrightnessChanged()
    {
        if (Environment is null) return;
        Environment.AdjustmentBrightness =
            EnvironmentComp.Brightness;
    }
    
    private void OnContrastChanged()
    {
        if (Environment is null) return;
        Environment.AdjustmentContrast =
            EnvironmentComp.Contrast;
    }
    
    private void OnSaturationChanged()
    {
        if (Environment is null) return;
        Environment.AdjustmentSaturation =
            EnvironmentComp.Saturation;
    }
    
    private void OnGlowBlendModeChanged()
    {
        if (Environment is null) return;
        var mode = EnvironmentComp.GlowBlendMode switch
        {
            GlowBlendModeEnum.Additive => Environment.GlowBlendModeEnum.Additive,
            GlowBlendModeEnum.Screen => Environment.GlowBlendModeEnum.Screen,
            GlowBlendModeEnum.Softlight => Environment.GlowBlendModeEnum.Softlight,
            GlowBlendModeEnum.Replace => Environment.GlowBlendModeEnum.Replace,
            _ => Environment.GlowBlendModeEnum.Mix
        };
        Environment.GlowBlendMode = mode;
    }
    
    private void OnHighlightHueChanged()
    {
        if (Environment?.AdjustmentColorCorrection is null) return;
        ((GradientTexture1D)Environment.AdjustmentColorCorrection).Gradient = new Gradient()
        {
            Colors = [EnvironmentComp.ShadowHue, EnvironmentComp.HighlightHue]
        };
    }
    
    private void OnShadowHueChanged()
    {
        if (Environment?.AdjustmentColorCorrection is null) return;
        ((GradientTexture1D)Environment.AdjustmentColorCorrection).Gradient =  new Gradient()
        {
            Colors = [EnvironmentComp.ShadowHue, EnvironmentComp.HighlightHue]
        };
    }
    
    private void OnGlowHueChanged()
    {
        if (Environment?.GlowMap is null) return;
        ((GradientTexture1D)Environment.GlowMap).Gradient = new Gradient()
        {
            Colors = [EnvironmentComp.GlowHue]
        };
    }
    
    private void OnIntensityChanged()
    {
        if (Environment is null) return;
        Environment.GlowIntensity =
            EnvironmentComp.Intensity;
    }
    
    private void OnStrengthChanged()
    {
        if (Environment is null) return;
        Environment.GlowStrength =
            EnvironmentComp.Strength;
    }
    
    private void OnBloomChanged()
    {
        if (Environment is null) return;
        Environment.GlowBloom =
            EnvironmentComp.Bloom;
    }
    #endregion
}
