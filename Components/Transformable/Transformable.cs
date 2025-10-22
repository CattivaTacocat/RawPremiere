using Godot;
using System;

namespace DeadDog.RawPremiere.Components;

public partial class Transformable : Node
{
    #region 属性字段
    private Vector2 _position; 
    private Vector2 _scale;
    private float _rotation;
    #endregion
    #region 属性
    [Export] public Vector2 Position { get; set; }
    [Export] public Vector2 Scale { get; set; }
    [Export] public float Rotation { get; set; }
    #endregion
}
