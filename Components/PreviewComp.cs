using Godot;

namespace RawPremiere.Components;

public partial class PreviewComp : Node
{
    #region 属性
    [Notify,Export] public Texture2D Preview { get => _preview.Get(); set => _preview.Set(value); }
    #endregion
}