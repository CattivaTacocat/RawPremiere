using Godot;
using RawPremiere.Components.Elements;
using RawPremiere.Objects.Elements;

namespace RawPremiere.Components;

public partial class ElementWidgetComp : WidgetComp<SimpleElement>
{
    [Notify,Export] public override SimpleElement Value { get => _value.Get(); set => _value.Set(value); }
}