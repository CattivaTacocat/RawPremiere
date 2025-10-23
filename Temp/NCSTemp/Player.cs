using Godot;
using System;

public partial class Player : Node
{
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_cancel"))
            GetChild(0).QueueFree();
    }
}
