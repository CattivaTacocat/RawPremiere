using System;
using DeadDog.Ordexp;
using Godot;

namespace DeadDog.Nodeo.Components.Lines.ConnectingLine;

public class Turn135LineTrailCalculator : SimpleLineTrailCalculator
{
    #region 处理
    public override Vector2[] CalcTrail(Vector2 start, Vector2 end)
    {
        if (start.IsEqualApprox(end)) return [];
        var d = end - start;
        if (d.X.IsEqualsApprox(0) || d.Y.IsEqualsApprox(0)) return [start, end];
        var kd = d.Y / d.X;
        var k = kd > 0 ? 1f : -1f;
        if (Math.Abs(kd).IsEqualsApprox(1)) return [start, end];
        var mid = (start + end) / 2;
        var b = mid.Y - k * mid.X;
        if (Math.Abs(kd) > 1)
        {
            var ns = new Vector2(start.X, k * start.X + b);
            var ne = new Vector2(end.X, k * end.X + b);
            return [start, ns, ne, end];
        }
        else
        {
            var ns = new Vector2((start.Y - b) / k, start.Y);
            var ne = new Vector2((end.Y - b) / k, end.Y);
            return [start, ns, ne, end];
        }
    }
    #endregion
}