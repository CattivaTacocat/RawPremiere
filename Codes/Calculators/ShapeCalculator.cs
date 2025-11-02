using System.Linq;
using DeadDog.Ordexp;
using Godot;

namespace RawPremiere.Codes.Calculators;

public static class ShapeCalculator
{
    #region 操作
    public static Vector2[] ScalePolygon(Vector2[] points, float ratio)
    {
        return points.Select(p => p * ratio).ToArray();
    }
    
    public static Vector2[] ClipHollow(Vector2[] points, float ratio)
    {
        if (points is null || points.Length < 3) return [];
        if (ratio <= 0)
        {
            return points;
        }
        if (ratio.IsEqualsApprox(1))
        {
            return [];
        }
        var other = ScalePolygon(points, ratio);
        return ratio is > 0 and < 1 
            ? ClipSimpleHollow(points, other) : ClipSimpleHollow(other, points);
    }
    
    public static Vector2[] ClipSector(Vector2[] points, float start, float amount)
    {
        if (points is null || points.Length < 3) return [];
        if (amount % 360 == 0) return points;
        var sa = (amount % 360).DegToRad();
        var ss = (start % 360).DegToRad();
        var length = GetMaxDistanceOptimized(points) * Mathf.Sqrt2;
        var sector = new Vector2[6]
        {
            Vector2.Zero,
            MathExpends.RadToVector2(ss,length),
            MathExpends.RadToVector2(ss+sa/4,length),
            MathExpends.RadToVector2(ss+sa/4*2,length),
            MathExpends.RadToVector2(ss+sa/4*3,length),
            MathExpends.RadToVector2(ss+sa,length),
        };
        return Geometry2D.ClipPolygons(points, sector)[0];
    }
    #endregion
    #region 处理
    private static Vector2[] ClipSimpleHollow(Vector2[] outer, Vector2[] inner)
    {
        var len = outer.Length;
        var total = (len + 1) * 2;
        var shape = new Vector2[total];
        for (int i = 0; i < len; i++)
        {
            shape[i] = outer[i];
            shape[len + 2 + i] = inner[len - i - 1];
        }
        shape[len] = outer[0];
        shape[len + 1] = inner[0];
        return shape;
    }
    
    private static float GetMaxDistanceOptimized(Vector2[] points)
    {
        if (points == null || points.Length == 0)
            return 0f;

        float maxSqrDistance = points
            .Select(point => point.X * point.X + point.Y * point.Y)
            .Prepend(0f).Max();
        return Mathf.Sqrt(maxSqrDistance);
    }
    #endregion
}