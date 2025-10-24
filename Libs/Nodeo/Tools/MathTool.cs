using System.Collections.Generic;
using Godot;

namespace DeadDog.Nodeo.Tools;

public class MathTool
{
    #region 处理
    #region 贝塞尔
    public static Vector2[] CalcBezierCurve3(Vector2[] points, int precision)
    {
        var ps = CalcBezierCurve2(points, precision);
        ps = CalcBezierCurve2(ps,precision);
        return ps;
    }
    
    public static Vector2[] CalcBezierCurve2(Vector2[] points, int precision)
    {
        List<Vector2> curvePoints = [];

        if (points is null || points.Length < 2) return curvePoints.ToArray();

        var numSegments = precision + 1;
        for (int i = 0; i <= precision; i++)
        {
            var t = (float)i / precision;
            var p = CalcBezierPoint(t, points);
            curvePoints.Add(p);
        }

        return curvePoints.ToArray();
    }
    
    private static Vector2 CalcBezierPoint(float t, Vector2[] points)
    {
        List<Vector2> cntPoints = new(points);
        while (cntPoints.Count > 1)
        {
            List<Vector2> newPoints = new();
            for (int i = 0; i < cntPoints.Count - 1; i++)
            {
                var x = Mathf.Lerp(cntPoints[i].X, cntPoints[i + 1].X, t);
                var y = Mathf.Lerp(cntPoints[i].Y, cntPoints[i + 1].Y, t);
                newPoints.Add(new Vector2(x, y));
            }
            cntPoints = newPoints;
        }
        return cntPoints[0];
    }
    #endregion
    
    #endregion
}