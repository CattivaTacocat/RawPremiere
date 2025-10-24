using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeadDog.Ordexp;
using RecallPast.Libs.Nodeo.Components.DraggableContainer;

namespace DeadDog.Nodeo.Components.DraggableContainer;

public partial class DraggableHBoxContainer : HBoxContainer
{
    #region 属性
    [Export] public bool P_IsDraggable { get; set; } = true;

    [Export] public Color P_FlowBoxColor { get; set; } = new Color(1,1,1,0.6f);

    [Export] public Color P_InsertLineColor { get; set; } = new Color(1,1,1,0.6f);

    [Export] public MouseButton P_DraggingMask { get; set; } = MouseButton.Left;
    
    [Export] public MouseButton P_CancellingMask { get; set; } = MouseButton.Right;
    #endregion
    #region 辅助字段
    private Control _currentOperateNode;

    private readonly List<DraggingObserver> _observers = [];
    private DraggingHCalculator _calculator;
    private DraggingHighlightModifier _modifier;
    private Vector2 _startDragPos = Vector2.Inf;
    private int _currentIdx;
    #endregion
    #region 事件
    public event Action OnStartDragging;
    #endregion
    #region 节点
    public CanvasLayer N_HighlightCanvas { get; private set; }
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitChildren();
        InitNodes();
        InitClazz();
    }

    private void InitNodes()
    {
        N_HighlightCanvas = new CanvasLayer();
        AddChild(N_HighlightCanvas,false,InternalMode.Back);
    }
    
    private void InitChildren()
    {
        HandleAddChildrenLogic();
    }

    private void InitClazz()
    {
        _calculator ??= new(this);
        _modifier ??= new(N_HighlightCanvas);
    }
    #endregion
    #region 响应
    private void RespondStartDragged(Control ctrl)
    {
        OnStartDragging?.Invoke();
        _startDragPos = GetGlobalMousePosition();
        _modifier.StartDrawLine(GetGlobalRect().Size.Y,P_InsertLineColor);
        _modifier.StartDrawRect(ctrl.GetGlobalRect(),P_FlowBoxColor);
    }

    private void RespondEndDragged(Control ctrl)
    {
        _modifier.EndDrawLine();
        _modifier.EndDrawRect();
        HandleReOrder(ctrl,_currentIdx);
    }
    
    private void RespondStartCancelled(Control ctrl)
    {
        _modifier.EndDrawLine();
        _modifier.EndDrawRect();
    }
    
    private void RespondEndCancelled(Control ctrl)
    {
    }
    #endregion
    #region 操作
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventMouseMotion emm)
        {
            ModifyHighlight();
        }
    }

    public void AddChild(Control child)
    {
        if (IsInstanceValid(child)) return;
        AddChild(child, true);
        HandleAddChildLogic(child);
    }

    public void RemoveChild(Control child)
    {
        HandleRemoveChildLogic(child);
        RemoveChild(child as Node);
    }
    #endregion
    #region 处理
    private void HandleAddChildrenLogic()
    {
        var children = GetChildren();
        foreach (var node in children)
        {
            if (node is not Control child) continue;
            var obs = new DraggingObserver(child);
            _observers.Add(obs);
        }
        Parallel.For(0,_observers.Count, i =>
        {
            var obs = _observers[i];
            obs.DraggingMask = P_DraggingMask;
            obs.CancellingMask = P_CancellingMask;
            obs.OnStartDragged += RespondStartDragged;
            obs.OnEndDragged += RespondEndDragged;
            obs.OnStartCancelled += RespondStartCancelled;
            obs.OnEndCancelled += RespondEndCancelled;
        });
    }
    
    private void HandleRemoveChildrenLogic()
    {
        Parallel.For(0,_observers.Count, i =>
        {
            var obs = _observers[i];
            obs.OnStartDragged -= RespondStartDragged;
            obs.OnEndDragged -= RespondEndDragged;
            obs.OnStartCancelled -= RespondStartCancelled;
            obs.OnEndCancelled -= RespondEndCancelled;
        });
        _observers.Clear();
    }

    private void HandleAddChildLogic(Control child)
    {
        var obs = new DraggingObserver(child);
        _observers.Add(obs);
        obs.OnStartDragged += RespondStartDragged;
        obs.OnEndDragged += RespondEndDragged;
        obs.OnStartCancelled += RespondStartCancelled;
        obs.OnEndCancelled += RespondEndCancelled;
    }
    
    private void HandleRemoveChildLogic(Control child)
    {
        var obs = _observers.Find(x => x.Control == child);
        if (obs is null) return;
        obs.OnStartDragged -= RespondStartDragged;
        obs.OnEndDragged -= RespondEndDragged;
        obs.OnStartCancelled -= RespondStartCancelled;
        obs.OnEndCancelled -= RespondEndCancelled;
        _observers.Remove(obs);
    }

    private void HandleReOrder(Control oldCtrl,int newIdx)
    {
        var oldIdx = GetChildren().IndexOf(oldCtrl);
        if (oldIdx == -1 || newIdx == -1)
            return;
        if (oldIdx == newIdx)
            return;
        if (oldIdx < newIdx) newIdx--;
        MoveChild(oldCtrl,newIdx);
    }

    private void ModifyHighlight()
    {
        var idx = _calculator.GetInsertIndex(_startDragPos,GetGlobalMousePosition());
        var linePos = _calculator.GetInsertLinePos(idx);
        _currentIdx = idx;
        _modifier.DrawingLine(linePos);
        _modifier.DrawingRect(GetGlobalMousePosition());
    }
    #endregion
}
