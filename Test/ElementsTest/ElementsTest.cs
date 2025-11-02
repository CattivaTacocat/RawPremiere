using Godot;
using System;

public partial class ElementsTest : Node
{
    [Export] public PlatformElement PlatformElement;
    [Export] public ShapeEntity ShapeEntity;

    private float _h;
    
    public override void _Process(double delta)
    {
        PlatformElement.CompleteRotation.Rotation = PlatformElement.CompleteRotation.Rotation with
        {
            Z = PlatformElement.CompleteRotation.Rotation.Z + (float)delta * 30
        };
        
        PlatformElement.Shape.SectorAmount += (float)(delta) * 30;

        _h = (_h + (float)(delta / 10)) % 0.5f;

        PlatformElement.Shape.Hollow = _h;

        GD.Print(ShapeEntity.Polygon.Length);
    }
}
