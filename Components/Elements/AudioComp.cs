using Godot;

namespace RawPremiere.Components.Elements;

public partial class AudioComp : Node
{
    #region 创建
    public AudioComp()
    {
        _audio.Set(new());
        _volume.Set(1);
    }
    #endregion
    #region 属性
    [Notify,Export] public AudioStream Audio { get => _audio.Get(); set => _audio.Set(value); }
    [Notify,Export] public int StartOffset { get => _startOffset.Get(); set => _startOffset.Set(value); }
    [Notify,Export] public int EndOffset { get => _endOffset.Get(); set => _endOffset.Set(value); }
    [Notify,Export] public float Volume { get => _volume.Get(); set => _volume.Set(value); }
    #endregion
}