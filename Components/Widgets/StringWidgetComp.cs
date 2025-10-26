using Godot;

namespace RawPremiere.Components;

public partial class StringWidgetComp : WidgetComp<string>
{
    [Notify][Export] public override string Value { get => _value.Get(); set => _value.Set(value); }
}