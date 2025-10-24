using System;
using Godot;

namespace RecallPast.Libs.Nodeo.Components.DraggableContainer;

public class DraggingObserver
{
    #region 属性
    public Control Control { get; private set; }
    public MouseButton DraggingMask { get; set; } = MouseButton.Left;
    public MouseButton CancellingMask { get; set; } = MouseButton.Right;
    #endregion
    #region 辅助字段
    private bool _isDragging;
    private bool _isCancelling;
    #endregion
    #region 事件
    public event Action<Control> OnStartDragged;
    public event Action<Control> OnEndDragged;
    public event Action<Control> OnStartCancelled;
    public event Action<Control> OnEndCancelled;
    #endregion
    #region 创建
    public DraggingObserver(Control control)
    {
        Control = control;
        Control.GuiInput += RespondGuiInput;
    }
    #endregion
    #region 响应
    private void RespondGuiInput(InputEvent e)
    {
        if (e is not InputEventMouseButton mouseEvent) return;
        if (mouseEvent.ButtonIndex == DraggingMask)
        {
            if (mouseEvent.Pressed)
            {
                if (_isCancelling) return;
                _isDragging = true;
                OnStartDragged?.Invoke(Control);
            }
            else
            {
                if (!_isDragging) return;
                _isDragging = false;
                OnEndDragged?.Invoke(Control);
            }
        }
        else if (mouseEvent.ButtonIndex == CancellingMask)
        {
            if (mouseEvent.Pressed)
            {
                if (!_isDragging) return;
                _isDragging = false;
                _isCancelling = true;
                OnStartCancelled?.Invoke(Control);
            }
            else
            {
                if (!_isCancelling) return;
                _isCancelling = false;
                OnEndCancelled?.Invoke(Control);
            }
        }
    }
    #endregion
}