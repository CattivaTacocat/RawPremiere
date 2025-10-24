using System;
using DeadDog.Nodeo.Tools;
using DeadDog.Ordexp;
using Godot;

namespace DeadDog.Nodeo.Components.Lines.ConnectingLine;

public class BezierLineTrailCalculator : SimpleLineTrailCalculator
{
    #region 处理
    public override Vector2[] CalcTrail(Vector2 start, Vector2 end)
    {
        if (start.IsEqualApprox(end)) return [];
        var d = end - start;
        if (d.X.IsEqualsApprox(0) || d.Y.IsEqualsApprox(0)) return [start, end];
        var mid = (start + end) / 2;
        var k = Math.Abs(d.Y / d.X);
        Vector2 ns, ne;
        if (k > 1)
        {
            ns = new Vector2(mid.X, start.Y);
            ne = new Vector2(mid.X, end.Y);
        }
        else
        {
            ns = new Vector2(start.X, mid.Y);
            ne = new Vector2(end.X, mid.Y);
        }
        return MathTool.CalcBezierCurve3([start, ns,ne, end], (int)d.Length() / 2);
    }
    #endregion
}