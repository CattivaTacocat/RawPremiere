using Godot;

namespace RawPremiere.Components.Elements;

public partial class SerialTextureComp : Node
{
    #region 创建
    public SerialTextureComp()
    {
        _texture.Set(new());
        _gridCount.Set(Vector2I.One * 4);
        _clipRange.Set(new());
        _fPS.Set(10);
        _isLoop.Set(true);
    }
    #endregion
    #region 属性
    [Notify,Export] public Texture2D Texture { get => _texture.Get(); set => _texture.Set(value); }
    [Notify,Export] public Vector2I GridCount { get => _gridCount.Get(); set => _gridCount.Set(value); }
    [Notify,Export] public Rect2I ClipRange { get  => _clipRange.Get(); set => _clipRange.Set(value); }
    [Notify,Export] public int FPS { get => _fPS.Get(); set => _fPS.Set(value); }
    [Notify,Export] public bool IsLoop { get => _isLoop.Get(); set => _isLoop.Set(value); }
    [Notify,Export] public int FrameIndex { get => _frameIndex.Get(); set => _frameIndex.Set(value); }
    #endregion
}