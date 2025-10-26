using Godot;

namespace RawPremiere.Components;

public partial class ArrayElemComp : Node
{
    #region 属性
    [Notify] public int ArrayIndex { get => _arrayIndex.Get(); set=>_arrayIndex.Set(value); }
    #endregion
}