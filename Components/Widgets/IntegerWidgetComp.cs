using Godot;

namespace RawPremiere.Components;

public partial class IntegerWidgetComp : WidgetComp<int>
{
    [Notify][Export]public override int Value { get => _value.Get(); set=>_value.Set(value); }
}