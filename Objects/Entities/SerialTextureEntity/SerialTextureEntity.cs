using Godot;
using System;
using RawPremiere.Components.Elements;

public partial class SerialTextureEntity : Sprite2D
{
    #region 组件
    [Notify,Export] public SerialTextureComp SerialTextureComp { get => _serialTextureComp.Get(); set => _serialTextureComp.Set(value); }
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitEvents();
        RespondAll();
    }
    
    private void InitEvents()
    {
        SerialTextureComp.FrameIndexChanged += OnFrameIndexChanged;
        SerialTextureComp.GridCountChanged += OnGridCountChanged;
        SerialTextureComp.TextureChanged += OnTextureChanged;
        SerialTextureComp.ClipRangeChanged += OnClipRangeChanged;
    }
    #endregion
    #region 销毁
    public override void _ExitTree()
    {
        DestroyEvents();
    }
    
    private void DestroyEvents()
    {
        SerialTextureComp.FrameIndexChanged -= OnFrameIndexChanged;
        SerialTextureComp.GridCountChanged -= OnGridCountChanged;
        SerialTextureComp.TextureChanged -= OnTextureChanged;
        SerialTextureComp.ClipRangeChanged -= OnClipRangeChanged;
    }
    #endregion
    #region 响应
    private void RespondAll()
    {
        OnFrameIndexChanged();
        OnGridCountChanged();
        OnTextureChanged();
        OnClipRangeChanged();
    }

    private void OnFrameIndexChanged() => Frame = SerialTextureComp.FrameIndex;

    private void OnGridCountChanged()
    {
        var g = SerialTextureComp.GridCount;
        Hframes = g.X;
        Vframes = g.Y;
    }
    
    private void OnTextureChanged() => Texture = SerialTextureComp.Texture;
    
    private void OnClipRangeChanged() => RegionRect = SerialTextureComp.ClipRange;
    #endregion
}
