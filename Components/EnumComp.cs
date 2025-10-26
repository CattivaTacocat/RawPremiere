using System;
using Godot;

namespace RawPremiere.Components;

public partial class EnumComp<E> : Node where E : Enum
{
    #region 属性
    [Notify] public E[] Enums { get => _enums.Get(); set=>_enums.Set(value); }
    [Notify] public int CurrentIndex { get => _currentIndex.Get(); set => _currentIndex.Set(value); }
    #endregion
}