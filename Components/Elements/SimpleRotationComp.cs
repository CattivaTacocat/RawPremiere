using Godot;

namespace RawPremiere.Components.Elements;

public partial class SimpleRotationComp : Node
{
    #region 属性
    [Notify, Export] public float Rotation {get=>_rotation.Get(); set => _rotation.Set(value); }
    #endregion
}