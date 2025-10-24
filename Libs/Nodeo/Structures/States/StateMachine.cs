using System.Collections.Generic;
using Godot;

namespace DeadDog.Nodeo.Structures.States;

public partial class StateMachine<SimpleState>
    : Node where SimpleState : IStateMachineState
{
    #region 辅助字段
    private string _currentKey = string.Empty;
    private SimpleState _currentState;
    protected Dictionary<string, SimpleState> _states;
    #endregion
    #region 创建
    public override void _EnterTree()
    {
        InitStatesDic();
    }

    /// <summary>
    /// 初始化状态字典_states，必须重写
    /// </summary>
    public virtual void InitStatesDic()
    {
        GD.PushError($"{nameof(InitStatesDic)}无有效重写");
    }
    #endregion
    #region 操作
    public void ChangeState(string stateName)
    {
        if (string.IsNullOrWhiteSpace(stateName)) return;
        if (_currentKey.Equals(stateName)) return;
        _currentState?.Exit();
        if (!_states.TryGetValue(stateName,out var state)) return;
        _currentState = state;
        _currentKey = stateName;
        _currentState?.Enter();
    }
    #endregion
    #region 原虚实现
    public override void _Process(double delta)
    {
        _currentState?.Update(delta);
    }

    public override void _Input(InputEvent @event)
    {
        _currentState?.Update(@event);
    }
    #endregion
}