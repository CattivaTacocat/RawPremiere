using Godot;
using System;
using System.Linq;
using DeadDog.Ordexp;

public partial class Rotation3dTest : Node2D
{
    [Notify,Export] public Vector3 Rotation3D { get => _rotation3D.Get(); set=>_rotation3D.Set(value,Rotate3D); }
    
    [Export] public Node2D X { get; set; }
    [Export] public Node2D Y { get; set; }
    [Export] public Node2D Z { get; set; }
    [Export] public Node2D Center { get; set; }
    [Export] public Node2D Test { get; set; }

    private Transform2D o;
    
    public override void _Ready()
    {
        o = Test.Transform;
    }
    
    private void Rotate3D()
    {
        X.Skew = Rotation3D.X.DegToRad();
        Y.Skew = Rotation3D.Y.DegToRad();
        Z.Rotation = Rotation3D.Z.DegToRad();

        var x = new Transform2D(0, Vector2.One, Rotation3D.X.DegToRad(), Vector2.Zero);
        var y = new Transform2D(90f.DegToRad(), Vector2.One, Rotation3D.Y.DegToRad(), Vector2.Zero);
        var z = new Transform2D(Rotation3D.Z.DegToRad(), Vector2.One, 0, Vector2.Zero);
        var fix = new Transform2D(-90f.DegToRad(), Vector2.One, 0, Vector2.Zero);
        var final = o * x * y * z * fix;
        Test.Transform = final;
    }
}
