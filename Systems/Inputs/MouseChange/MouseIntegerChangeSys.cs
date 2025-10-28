using Godot;
using RawPremiere.Components;
using RawPremiere.Components.Enums;

namespace RawPremiere.Systems.Inputs;

public partial class MouseIntegerChangeSys : Node,IMouseChangeSys<int>
{
    #region 组件
    [Export] public IntegerWidgetComp WidgetComp { get; private set; }
    [Export] public MouseIntegerChangeComp MouseValueChangeComp { get; private set; }
    [Export] public HoverComp HoverComp { get; private set; }
    #endregion
    #region 系统
    [Export] public IntegerWidgetSys IntegerWidgetSys;
    #endregion
    #region 字段
    private int _currentValue;

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
        var v = (int)(offset * MouseValueChangeComp.AltRatio);
        var value = (_currentValue + v) / MouseValueChangeComp.AltRatio * MouseValueChangeComp.AltRatio;
        SetWidgetCompValue(value);
    }

    public void NormalChangeValue(float offset)
    {
        var value = _currentValue + (int)offset * MouseValueChangeComp.NormalIncrement;
        SetWidgetCompValue(value);
    }
    #endregion
    #region 决策
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
    #endregion
    #region 处理
    public void SetWidgetCompValue(int value)
    {
        if (IsInstanceValid(IntegerWidgetSys))
            IntegerWidgetSys.SetValue(value);
        else
            WidgetComp.Value = value;
    }
    #endregion
}