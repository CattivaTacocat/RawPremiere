using Godot;
using System;

public partial class ElementsTest : Node
{
    [Export] public ShapeElement ShapeElement;
    [Export] public ShapeEntity ShapeEntity;

    private float _h;
    
    public override void _Process(double delta)
    {
        // ShapeElement.CompleteRotation.Rotation = ShapeElement.CompleteRotation.Rotation with
        // {
        //     Z = ShapeElement.CompleteRotation.Rotation.Z + (float)delta * 30
        // };
        //
        // ShapeElement.Shape.SectorAmount += (float)(delta) * 30;
        //
        // _h = (_h + (float)(delta / 10)) % 0.5f;
        //
        // ShapeElement.Shape.Hollow = _h;
        //
        // GD.Print(ShapeEntity.Polygon.Length);
    }
}
