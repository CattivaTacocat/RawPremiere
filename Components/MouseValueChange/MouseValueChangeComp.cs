using System;
using Godot;
using RawPremiere.Components.Enums;

namespace RawPremiere.Components;

public partial class MouseValueChangeComp<T> : Node where T : IComparable<T>
{
    #region 属性
    [Notify][Export] public MouseChangeMethodEnum MouseChangeMethod
    {
        get => _mouseChangeMethod.Get(); set=> _mouseChangeMethod.Set(value);
    }
    [Notify] public virtual T NormalIncrement {get=> _normalIncrement.Get(); set=> _normalIncrement.Set(value);}
    [Notify] public virtual T ShiftIncrement {get=> _shiftIncrement.Get(); set=> _shiftIncrement.Set(value);}
    [Notify] public virtual T CtrlIncrement {get=> _ctrlIncrement.Get(); set=> _ctrlIncrement.Set(value);}
    [Notify] public virtual T AltRatio {get=> _altRatio.Get(); set=> _altRatio.Set(value);}
    #endregion
}