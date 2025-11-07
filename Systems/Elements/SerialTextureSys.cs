using Godot;
using RawPremiere.Components.Elements;

namespace RawPremiere.Systems.Elements;

public partial class SerialTextureSys : Node
{
    #region 组件
    [Notify, Export] public SerialTextureComp Comp { get => _comp.Get(); set => _comp.Set(value); }
    #endregion
    #region 节点
    [Export] public Timer Timer { get; private set; }
    #endregion
    #region 字段
    private int _frameMax;
    private int _frameIndex;
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitEvents();
        RespondAll();
    }
    
    private void InitEvents()
    {
        Timer.Timeout += ChangeFrameIndex;
        Comp.FPSChanged += OnFPSChanged;
        Comp.GridCountChanged += OnGridCountChanged;
    }
    #endregion
    #region 销毁
    public override void _ExitTree()
    {
        DestroyEvents();   
    }
    
    private void DestroyEvents()
    {
        Timer.Timeout -= ChangeFrameIndex;
        Comp.FPSChanged -= OnFPSChanged;
        Comp.GridCountChanged -= OnGridCountChanged;
    }
    #endregion
    #region 响应
    private void RespondAll()
    {
        OnFPSChanged();
        OnGridCountChanged();
    }

    private void OnFPSChanged() => Timer.WaitTime = Comp.FPS > 0 ? 1.0 / Comp.FPS : Timer.WaitTime;
    
    private void OnGridCountChanged() => _frameMax = Comp.GridCount.X * Comp.GridCount.Y - 1;
    #endregion
    #region 操作
    public void Play() => Timer.Start();

    public void Stop() => Timer.Stop();
    #endregion
    #region 处理
    private void ChangeFrameIndex()
    {
        if (Comp.Texture is null) return;
        if (Comp.IsLoop)
            _frameIndex = (_frameIndex + 1) % (_frameMax + 1);
        else
        {
            if (_frameIndex == _frameMax) return;
            if (_frameIndex < _frameMax)
                _frameIndex += 1;
            else
                _frameIndex = _frameMax;
        }
        Comp.FrameIndex = _frameIndex;
    }
    #endregion
}