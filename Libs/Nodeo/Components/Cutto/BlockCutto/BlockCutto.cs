using System;
using DeadDog.Nodeo.Tools;
using Godot;

namespace DeadDog.Nodeo.Components.Cutto;

public partial class BlockCutto : SimpleCutto
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
            UpdateIsTiledView();
        }
    }
    #endregion
    #region 辅助字段
    private ShaderMaterial _shader;
    private Tween _tween;
    #endregion
    #region 事件
    public override event Action OnTransInFinished;
    public override event Action OnTransOutFinished;
    #endregion
    #region 创建
    public override void _Ready()
    {
        base._Ready();
        InitShader();
    }

    private void InitShader() => _shader = N_CuttoContainer.Material as ShaderMaterial;
    #endregion
    #region 动画
    public override void TransIn()
    {
        _tween = TweenTool.CreateFrom(this);
        var v = CalcFinalProgress();
        _shader.SetShaderParameter("progress_bias",Vector2.One * 10);
        _tween.TweenMethod(Callable.From<float>(
                value => _shader.SetShaderParameter("progress", value))
            ,0,v,P_Duration );
        _tween.TweenCallback(Callable.From(() => OnTransInFinished?.Invoke()));
    }

    public override void TransOut()
    {
        _tween = TweenTool.CreateFrom(this);
        var v = CalcFinalProgress();
        _shader.SetShaderParameter("progress_bias",Vector2.One*-10);
        _tween.TweenMethod(Callable.From<float>(
                value => _shader.SetShaderParameter("progress", value))
            ,1,-v+1,P_Duration );
        _tween.TweenCallback(Callable.From(() => OnTransOutFinished?.Invoke()));
    }
    #endregion
    #region 视图
    protected override void UpdateIsTiledView()
    {
        base.UpdateIsTiledView();
        if (P_IsTiled)
            _shader?.SetShaderParameter("grid_size", Vector2.One);
        else
            _shader?.SetShaderParameter("grid_size", Vector2.One * 20);
    }
    #endregion
    #region 处理
    private float CalcFinalProgress()
    {
        if (P_IsTiled)
        {
            var size = N_CuttoContainer.Texture.GetSize();
            var dxy = N_CuttoContainer.Size / size;
            return dxy.X + dxy.Y;
        }
        else
        {
            var grid = (Vector2)_shader.GetShaderParameter("grid_size");
            return grid.X + grid.Y;
        }
    }
    #endregion
}
