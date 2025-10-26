using System;
using Godot;

namespace RawPremiere.Components;

public partial class WidgetComp<T> : Node
{
    #region 属性
    [Notify]public virtual T Value { get => _value.Get(); set=> _value.Set(value); }
    [Notify][Export] public virtual string DisplayValue { get => _displayValue.Get(); set=>_displayValue.Set(value); }
    [Notify][Export] public virtual bool CanUse { get => _canUse.Get(); set=> _canUse.Set(value); }
    [Notify][Export] public virtual bool Visibility { get => _visibility.Get(); set => _visibility.Set(value); }
    #endregion
}