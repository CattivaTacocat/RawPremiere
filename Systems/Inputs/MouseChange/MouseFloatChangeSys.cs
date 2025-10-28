using System;
using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems.Inputs;

//HACK:还没进行初步测试，只是从整型中简单修改
public partial class MouseFloatChangeSys : Node, IMouseChangeSys<float>
{
    #region 组件
    [Export] public FloatWidgetComp WidgetComp { get; private set; }
    [Export] public MouseFloatChangeComp MouseValueChangeComp { get; private set; }
    [Export] public HoverComp HoverComp { get; private set; }
    #endregion
    #region 系统
    [Export] public FloatWidgetSys FloatWidgetSys { get; private set; }
    #endregion
    #region 字段
    private float _currentValue;
    
    private MouseChangeOffsetObserver _offsetObserver;
    private MouseChangeKeyObserver _keyObserver;
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitClazz();
        InitEvents();
    }
    
    private void InitEvents()
    {
        _offsetObserver.OffsetChanged += OnOffsetChanged;
        _keyObserver.CurrentKeyChanged += OnKeyChanged;
    }
    
    private void InitClazz()
    {
        _offsetObserver ??= new();
        _keyObserver ??= new();
    }
    #endregion
    #region 销毁
    public override void _ExitTree()
    {
        DestroyEvents();
    }
    
    private void DestroyEvents()
    {
        _offsetObserver.OffsetChanged -= OnOffsetChanged;
        _keyObserver.CurrentKeyChanged -= OnKeyChanged;
    }
    #endregion
    #region 响应
    private void OnOffsetChanged()
    {
        ChangeValue(_offsetObserver.Offset);
    }

    private void OnKeyChanged()
    {
        if (!_offsetObserver.StartChange) return;
        _currentValue = WidgetComp.Value;
        _offsetObserver.ResetOffset();
    }
    #endregion
    #region 操作
    public override void _Input(InputEvent @event)
    {
        if (!HoverComp.IsHovered) return;
        _keyObserver.ObserveKey(@event);
        _offsetObserver.ObserveOffset(@event, MouseValueChangeComp.MouseChangeMethod);
        if (!_offsetObserver.StartChange) _currentValue = WidgetComp.Value;
    }

    public void CtrlChangeValue(float offset)
    {
        var value = _currentValue + (int)offset * MouseValueChangeComp.CtrlIncrement;
        SetWidgetCompValue(value);
    }

    public void ShiftChangeValue(float offset)
    {
        var value = _currentValue + (int)offset * MouseValueChangeComp.ShiftIncrement;
        SetWidgetCompValue(value);
    }

    public void AltChangeValue(float offset)
    {
        var v = _currentValue + offset * MouseValueChangeComp.AltRatio;
        var value = (int)(v / MouseValueChangeComp.AltRatio) * MouseValueChangeComp.AltRatio;
        SetWidgetCompValue(value);
    }

    public void NormalChangeValue(float offset)
    {
        var value = _currentValue + offset * MouseValueChangeComp.NormalIncrement;
        SetWidgetCompValue(value);
    }
    #endregion
    #region 处理
    private void ChangeValue(float offset)
    {
        if (_keyObserver.IsPressed)
        {
            switch (_keyObserver.CurrentKey)
            {
                case Key.Ctrl:
                    CtrlChangeValue(offset);
                    break;
                case Key.Shift:
                    ShiftChangeValue(offset);
                    break;
                case Key.Alt:
                    AltChangeValue(offset);
                    break;
                default:
                    NormalChangeValue(offset);
                    break;
            }
        }
        else
            NormalChangeValue(offset);
    }
    
    public void SetWidgetCompValue(float value)
    {
        if (IsInstanceValid(FloatWidgetSys))
            FloatWidgetSys.SetValue(value);
        else
            WidgetComp.Value = value;
    }
    #endregion
}