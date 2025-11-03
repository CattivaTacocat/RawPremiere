using System;
using Godot;

namespace DeadDog.Nodeo.Structures;

    /// <summary>
/// 二维极坐标：以原点为极点，X 轴正方向为极轴，角度逆时针为正，单位为弧度。
/// 与 Vector2.Angle() 的角度定义一致。
/// </summary>
[Serializable]
public struct Polar2 : IEquatable<Polar2>
{
    /// <summary>极径（长度），非负</summary>
    public float R { get; }

    /// <summary>极角（弧度），范围通常为 [0, 2π)</summary>
    public float T { get; }

    /// <summary>构造：r >= 0；theta 会被规范化到 [0, 2π)</summary>
    public Polar2(float r, float theta)
    {
        R = r;
        T = theta;
    }

    /// <summary>从直角坐标构造（x, y）</summary>
    public static Polar2 FromCartesian(float x, float y)
    {
        var r = float.Sqrt(x * x + y * y);
        var theta = float.Atan2(y, x);
        return new Polar2(r, theta);
    }

    /// <summary>从 Vector2 构造</summary>
    public static Polar2 FromVector2(Vector2 v) => FromCartesian(v.X, v.Y);

    /// <summary>转换回直角坐标</summary>
    public Vector2 ToCartesian() => new Vector2(
        (float)(R * Math.Cos(T)),
        (float)(R * Math.Sin(T))
    );

    /// <summary>转换为 Vector2</summary>
    public static implicit operator Vector2(Polar2 p) => p.ToCartesian();

    /// <summary>从 Vector2 隐式构造</summary>
    public static implicit operator Polar2(Vector2 v) => FromVector2(v);
    
    /// <summary>规范化角度到 [0, 2π)</summary>
    public static float NormalizeAngle(float angle)
    {
        const float TwoPi = 2 * float.Pi;
        var a = angle % TwoPi;
        if (a < 0) a += TwoPi;
        return a;
    }

    /// <summary>与另一个 Polar2 比较是否近似相等</summary>
    public bool Equals(Polar2 other)
    {
        const double Epsilon = 1e-6;
        return Math.Abs(R - other.R) < Epsilon && Math.Abs(NormalizeAngle(T - other.T)) < Epsilon;
    }

    public override bool Equals(object obj) => obj is Polar2 other && Equals(other);

    public override int GetHashCode()
    {
        var a = NormalizeAngle(T);
        return HashCode.Combine(R, a);
    }

    public override string ToString() => $"Polar2(R={R:F3}, Theta={T:F3} rad ≈ {Math.Round(RadToDeg(T), 1)}°)";

    /// <summary>弧度转角度的便捷方法</summary>
    public float RadToDeg(float rad) => rad * (180f / float.Pi);

    /// <summary>角度转弧度的便捷方法</summary>
    public float DegToRad(float deg) => deg * (float.Pi / 180f);
    
    public static bool operator ==(Polar2 left, Polar2 right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(Polar2 left, Polar2 right)
    {
        return !(left == right);
    }
}