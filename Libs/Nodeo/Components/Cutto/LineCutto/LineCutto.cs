using Godot;
using System;
using DeadDog.Nodeo.Components.Cutto;
using DeadDog.Nodeo.Tools;

public partial class LineCutto : SimpleCutto
{
    #region 属性字段
    private bool _isTiled;
    #endregion
    #region 属性
    public override float P_Duration { get; set; } = 1f;

    public override bool P_IsTiled
    {
        get => _isTiled;
        set
        {
            if (_isTiled == value) return;
            _isTiled = value;
            GD.PrintRich("[color=#ffff00]平铺对该转场样式无效");
        }
    }
    #endregion
    #region 辅助字段
    private ShaderMaterial _shader;
    private Tween _tween;
    
    private const float START_PROGRESS_VALUE = 0f;
    private const float END_PROGRESS_VALUE = 16f;
    private const float START_FEATHER_VALUE = 0f;
    private const float END_FEATHER_VALUE = 10f;
    #endregion
    #region 事件
    public override event Action OnTransInFinished;
    public override event Action OnTransOutFinished;
    #endregion
    #region 创建
    public override void _Ready()
    {
        base._Ready();
        _shader = N_CuttoContainer.Material as ShaderMaterial;
    }
    #endregion
    #region 动画
    public override void TransIn()
    {
        _tween = TweenTool.CreateFrom(this);
        _shader.SetShaderParameter("invert",true);
        _tween.TweenMethod(Callable.From<float>(
            value => _shader.SetShaderParameter("progress",value)),
            START_PROGRESS_VALUE, END_PROGRESS_VALUE, P_Duration);
        _tween.TweenMethod(Callable.From<float>(
            value => _shader.SetShaderParameter("shape_feather",value)),
            START_FEATHER_VALUE, END_FEATHER_VALUE, P_Duration);
        _tween.TweenCallback(Callable.From(() => OnTransInFinished?.Invoke()));
    }

    public override void TransOut()
    {
        _tween = TweenTool.CreateFrom(this);
        _shader.SetShaderParameter("invert",false);
        _tween.TweenMethod(Callable.From<float>(
            value => _shader.SetShaderParameter("progress",value)),
            START_PROGRESS_VALUE, END_PROGRESS_VALUE, P_Duration);
        _tween.TweenMethod(Callable.From<float>(
            value => _shader.SetShaderParameter("shape_feather",value)),
            START_FEATHER_VALUE, END_FEATHER_VALUE, P_Duration);
        _tween.TweenCallback(Callable.From(() => OnTransOutFinished?.Invoke()));
    }
    #endregion
}
