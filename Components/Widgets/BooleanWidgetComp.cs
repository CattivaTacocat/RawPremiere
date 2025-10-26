using Godot;

namespace RawPremiere.Components;

public partial class BooleanWidgetComp : WidgetComp<bool>
{
    [Notify,Export] public override bool Value { get => _value.Get(); set => _value.Set(value); }
}