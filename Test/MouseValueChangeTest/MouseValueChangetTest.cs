using Godot;
using System;
using RawPremiere.Components;

public partial class MouseValueChangetTest : Node
{
    [Export] public IntegerWidgetComp WidgetComp;

    public override void _Ready()
    {
        WidgetComp.ValueChanged += () =>
        {
            GD.Print(WidgetComp.Value);
        };
    }
}
