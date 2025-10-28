using System;
using RawPremiere.Components;

namespace RawPremiere.Systems.Inputs;

public interface IMouseChangeSys<in T> where T : IComparable<T>
{
    #region 组件
    HoverComp HoverComp { get; }
    #endregion
    #region 操作
    void CtrlChangeValue(float offset);
    void ShiftChangeValue(float offset);
    void AltChangeValue(float offset);
    void NormalChangeValue(float offset);
    #endregion
    #region 处理
    void SetWidgetCompValue(T value);
    #endregion
}