using DeadDog.RawPremiere.Standards;
using Godot;
using RawPremiere.Components.Enums;

namespace RawPremiere.Systems.Inputs;

public partial class MouseChangeOffsetObserver
{
    #region 属性
    [Notify] public float Offset { get => _offset.Get(); protected set => _offset.Set(value); }
    public bool StartChange { get; private set; }
    #endregion
    #region 辅助字段
    private Vector2 _mouseStart;
    private Vector2 _mousePos;
    #endregion
    #region 操作
    public void ObserveOffset(InputEvent @event,MouseChangeMethodEnum method)
    {
        if (@event is not InputEventMouse em) return;
        _mousePos = em.GlobalPosition;
        switch (method)
        {
            case MouseChangeMethodEnum.Horizontal:
                HObserve(em);
                break;
            case MouseChangeMethodEnum.Vertical:
                VObserve(em);
                break;
            case MouseChangeMethodEnum.Roller:
                RObserve(em);
                break;
        }
    }

    public void ResetOffset()
    {
        _mouseStart = _mousePos;
        Offset = 0;
    }
    #endregion
    #region 处理
    private void HObserve(InputEventMouse em)
    {
        switch (em)
        {
            case InputEventMouseButton {Pressed:true, ButtonIndex: MouseButton.Left }:
                StartChange = true;
                _mouseStart = em.GlobalPosition;
                break;
            case InputEventMouseButton {Pressed:false, ButtonIndex: MouseButton.Left }:
                StartChange = false;
                break;
        }

        if (em is not InputEventMouseMotion emm || !StartChange) return;
        Offset = (emm.GlobalPosition.X - _mouseStart.X) / GlobalUnit.UNIT_DRAG_OFFSET;
        OffsetChanged?.Invoke();
    }

    private void VObserve(InputEventMouse em)
    {
        switch (em)
        {
            case InputEventMouseButton {Pressed:true, ButtonIndex: MouseButton.Left }:
                StartChange = true;
                _mouseStart = em.GlobalPosition;
                break;
            case InputEventMouseButton {Pressed:false, ButtonIndex: MouseButton.Left }:
                StartChange = false;
                break;
        }

        if (em is not InputEventMouseMotion emm || !StartChange) return;
        Offset = (emm.GlobalPosition.Y - _mouseStart.Y) / -GlobalUnit.UNIT_DRAG_OFFSET;
        OffsetChanged?.Invoke();
    }

    private void RObserve(InputEventMouse em)
    {
        if (em is not InputEventMouseButton emb) return;
        if (emb.ButtonIndex == MouseButton.Left) StartChange = emb.Pressed;
        if (!StartChange) return;
        Offset = emb.ButtonIndex switch
        {
            MouseButton.WheelUp => Offset + 0.5f,
            MouseButton.WheelDown => Offset - 0.5f,
            _ => 0
        };
    }
    #endregion
}