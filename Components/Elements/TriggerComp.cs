using Godot;
using RawPremiere.Components.Enums;
using RawPremiere.Objects.Commands;

namespace RawPremiere.Components.Elements;

public partial class TriggerComp : Node
{
    #region 创建
    public TriggerComp()
    {
        _receiveMode.Set(TriggerReceiveModeEnum.Hover);
        _fixedMode.Set(TriggerFixedModeEnum.Peal);
        _executeMode.Set(TriggerExecuteModeEnum.Pulse);
        _isActive.Set(false);
        _waitingTime.Set(0);
        _interval.Set(100);
    }
    #endregion
    #region 属性
    [Notify,Export] public TriggerReceiveModeEnum ReceiveMode { get => _receiveMode.Get(); set => _receiveMode.Set(value); }
    [Notify,Export] public TriggerFixedModeEnum FixedMode { get => _fixedMode.Get(); set => _fixedMode.Set(value); }
    [Notify,Export] public TriggerExecuteModeEnum ExecuteMode { get => _executeMode.Get(); set => _executeMode.Set(value); }
    [Notify,Export] public bool IsActive { get => _isActive.Get(); set => _isActive.Set(value); }
    [Notify,Export] public int WaitingTime { get => _waitingTime.Get(); set => _waitingTime.Set(value); }
    [Notify,Export] public int Interval { get => _interval.Get(); set => _interval.Set(value); }
    [Notify] public CommandSet CommandSet { get => _commandSet.Get(); set => _commandSet.Set(value); }
    #endregion
}