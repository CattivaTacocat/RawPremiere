using System;
using Godot;

namespace RawPremiere.Temp.NCSTemp;

public struct MovementComp : IEquatable<MovementComp>
{
    public MovementComp(){}
    
    public MovementComp(Vector2 direction, float speed)
    {
        Direction = direction;
        Speed = speed;
    }
    
    public Vector2 Direction = Vector2.Zero;
    public float Speed = 200;

    public bool Equals(MovementComp other)
    {
        return Direction.Equals(other.Direction) && Speed.Equals(other.Speed);
    }

    public override bool Equals(object obj)
    {
        return obj is MovementComp other && Equals(other);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Direction, Speed);
    }

    public override string ToString()
    {
        return $"(Direction: {Direction}, Speed: {Speed})";
    }
    
    public static bool operator ==(MovementComp left, MovementComp right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(MovementComp left, MovementComp right)
    {
        return !(left == right);
    }
}