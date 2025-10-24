using Godot;
using System;

namespace DeadDog.Nodeo.Components.OperationalCamera;

public partial class OperationalCamera2D : Camera2D
{
    #region 属性
    [Export] public float ZoomSpeed { get; set; } = 0.5f;
    [Export] public float MinZoom { get; set; } = 0.5f;
    [Export] public float MaxZoom { get; set; } = 3.0f;
    [Export] public bool SmoothDrag { get; set; } = true;
    [Export] public bool SmoothZoom { get; set; } = true;
    [Export] public bool CanOperate { get; set; } = true;
    #endregion
    #region 辅助字段
    private Vector2 _dragStartMousePos;
    private Vector2 _dragStartCameraPos;
    private float _targetZoom;
    private float _rawTargetZoom;
    private Vector2 _targetPos;
    private bool _isDragging = false;

    private Vector2 _originZoom;
    private Vector2 _originPosition;
    #endregion
    #region 重写方法
    public override void _Ready()
    {
        _targetZoom = Zoom.X;
        _rawTargetZoom = Zoom.X;
        _targetPos = Position;
        _originZoom = Zoom;
        _originPosition = Position;
    }

    public override void _Input(InputEvent @event)
    {
        if (!CanOperate) return;
        MouseDrag(@event);
        MouseZoom(@event);
    }

    public override void _Process(double delta)
    {
        LerpZoom(delta);
        LerpPosition(delta);
    }
    #endregion
    #region 视图方法
    private void LerpZoom(double delta)
    {
        if (Mathf.IsEqualApprox(Zoom.X, _rawTargetZoom, 0.001f)) return;
        if (SmoothZoom)
        {
            if (Zoom.X > MaxZoom || Zoom.X < MinZoom)
                Zoom = Vector2.One * (float)Mathf.Lerp(Zoom.X, _targetZoom, 3 * delta);
            else
                Zoom = Vector2.One * (float)Mathf.Lerp(Zoom.X, _rawTargetZoom, 8 * delta);
        }
        else Zoom = Vector2.One * _targetZoom;
    }

    private void LerpPosition(double delta)
    {
        if (Position.IsEqualApprox(_targetPos)) return;
        if (SmoothDrag)
            Position = new Vector2(
                (float)Mathf.Lerp(Position.X, _targetPos.X, 8 * (float)delta),
                (float)Mathf.Lerp(Position.Y, _targetPos.Y, 8 * (float)delta));
        else Position = _targetPos;
    }
    #endregion
    #region 操作方法
    public void Reset()
    {
        Zoom = _originZoom;
        Position = _originPosition;
    }

    public void Place(Vector2 pos)
    {
        _targetPos = pos;
        Position = pos;
    }

    public void SetLimit(Rect2I limit)
    {
        LimitLeft = limit.Position.X;
        LimitRight = limit.Position.X + limit.Size.X;
        LimitTop = limit.Position.Y;
        LimitBottom = limit.Position.Y + limit.Size.Y;
    }
    #endregion
    #region 逻辑方法
    private void MouseDrag(InputEvent @event)
    {
        if (@event is InputEventMouseButton mouseButton)
        {
            if (mouseButton.ButtonIndex != MouseButton.Middle) return;
            if (mouseButton.Pressed)
                StartDrag(mouseButton);
            else
                StopDrag();
            GetTree().Root.SetInputAsHandled();
        }
        else if (@event is InputEventMouseMotion mouseMotion && _isDragging)
        {
            DragCamera(mouseMotion);
            GetTree().Root.SetInputAsHandled();
        }
    }

    private void MouseZoom(InputEvent @event)
    {
        if (@event is not InputEventMouseButton mouseButton ||
            (mouseButton.ButtonIndex != MouseButton.WheelUp &&
             mouseButton.ButtonIndex != MouseButton.WheelDown)) return;
        ZoomCamera(mouseButton);
        GetTree().Root.SetInputAsHandled();
    }

    private void StartDrag(InputEventMouseButton ev)
    {
        if (ev.ButtonIndex != MouseButton.Middle) return;
        _isDragging = true;
        _dragStartMousePos = ev.Position;
        _dragStartCameraPos = Position;
    }
    private void StopDrag()
    {
        _isDragging = false;
        _dragStartMousePos = Vector2.Zero;
    }

    private void DragCamera(InputEventMouseMotion ev)
    {
        if (!_isDragging) return;
        var delta = _dragStartMousePos - ev.Position;
        _targetPos = _dragStartCameraPos + delta * 1 / _targetZoom;
    }

    private void ZoomCamera(InputEventMouseButton ev)
    {
        var currentZoom = Zoom.X;
        var zoomDirection = ev.ButtonIndex == MouseButton.WheelUp ? 1f : -1f;
        var zoomFactor = 1.0f + (zoomDirection * ZoomSpeed);
        var newZoom = currentZoom * zoomFactor;
        _rawTargetZoom = newZoom;
        _targetZoom = Math.Clamp(newZoom, MinZoom, MaxZoom);
    }
    #endregion
}
