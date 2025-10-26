using Godot;

namespace RawPremiere.Components;

public partial class MouseIntegerChangeComp : MouseValueChangeComp<int>
{
    #region 属性
    [Notify,Export] public override int ShiftIncrement { get => _shiftIncrement.Get(); set => _shiftIncrement.Set(value); }
    [Notify,Export] public override int CtrlIncrement { get => _ctrlIncrement.Get(); set => _ctrlIncrement.Set(value); }
    [Notify,Export] public override int AltRatio { get => _altRatio.Get(); set => _altRatio.Set(value); }
    [Notify,Export] public override int NormalIncrement { get => _normalIncrement.Get(); set => _normalIncrement.Set(value); }
    #endregion
}