using System;
using System.Linq;
using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems;

public partial class EnumWidgetSys<E> : WidgetSys<E> where E : Enum
{
    #region 组件
    [Export] public EnumComp<E> EnumComp;
    #endregion
    #region 重写
    public override void SetValue(E value)
    {
        var idx = EnumComp.Enums.ToList().IndexOf(value);
        EnumComp.CurrentIndex = idx;
        WidgetComp.Value = value;
        WidgetComp.DisplayValue = value.ToString();
    }
    #endregion
}