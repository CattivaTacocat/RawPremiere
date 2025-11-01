using Godot;

namespace RawPremiere.Components.Elements;

public partial class ScaleComp : Node
{
    #region 创建
    public ScaleComp()
    {
        _scale.Set(Vector2.One);
    }
    #endregion
    #region 属性
    [Notify,Export] public Vector2 Scale { get => _scale.Get(); set => _scale.Set(value); }
    #endregion
}