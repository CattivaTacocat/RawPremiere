using Godot;

namespace RawPremiere.Components;

public partial class ColorWidgetComp : WidgetComp<Color>
{
    [Notify][Export] public override Color Value { get => _value.Get(); set=> _value.Set(value); }
}