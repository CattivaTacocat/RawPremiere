using Godot;

namespace RawPremiere.Components;

public partial class MouseFloatChangeComp : MouseValueChangeComp<float>
{
    #region 属性
    [Notify,Export] public override float NormalIncrement { get => _normalIncrement.Get(); set => _normalIncrement.Set(value); }
    [Notify,Export] public override float ShiftIncrement { get => _shiftIncrement.Get(); set => _shiftIncrement.Set(value); }
    [Notify,Export] public override float CtrlIncrement { get => _ctrlIncrement.Get(); set => _ctrlIncrement.Set(value); }
    [Notify,Export] public override float AltRatio { get => _altRatio.Get(); set => _altRatio.Set(value); }
    #endregion
}