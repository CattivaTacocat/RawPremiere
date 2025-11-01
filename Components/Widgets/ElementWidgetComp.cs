using Godot;
using RawPremiere.Components.Elements;
using RawPremiere.Objects.Elements;

namespace RawPremiere.Components;

public partial class ElementWidgetComp : WidgetComp<IElement>
{
    [Notify] public override IElement Value { get => _value.Get(); set => _value.Set(value); }
}