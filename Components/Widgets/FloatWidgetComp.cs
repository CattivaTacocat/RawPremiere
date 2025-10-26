using Godot;

namespace RawPremiere.Components;

public partial class FloatWidgetComp : WidgetComp<float>
{
    [Notify][Export] public override float Value { get => _value.Get(); set => _value.Set(value); }
}