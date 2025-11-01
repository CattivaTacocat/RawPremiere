using Godot;

namespace RawPremiere.Components.Elements;

public partial class SerialTextureComp : Node
{
    #region 创建
    public SerialTextureComp()
    {
        _texture.Set(new());
        _gridRange.Set(Vector2I.One * 8);
        _gridOffset.Set(Vector2I.Zero);
        _gridSpace.Set(Vector2I.Zero);
        _fPS.Set(10);
        _isLoop.Set(true);
    }
    #endregion
    #region 属性
    [Notify,Export] public Texture2D Texture { get => _texture.Get(); set => _texture.Set(value); }
    [Notify,Export] public Vector2I GridRange { get => _gridRange.Get(); set => _gridRange.Set(value); }
    [Notify,Export] public Vector2I GridOffset { get => _gridOffset.Get(); set => _gridOffset.Set(value); }
    [Notify,Export] public Vector2I GridSpace { get => _gridSpace.Get(); set => _gridSpace.Set(value); }
    [Notify,Export] public int FPS { get => _fPS.Get(); set => _fPS.Set(value); }
    [Notify,Export] public bool IsLoop { get => _isLoop.Get(); set => _isLoop.Set(value); }
    #endregion
}