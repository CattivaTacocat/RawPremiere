using System;
using System.Collections.Generic;
using DeadDog.Ordexp;
using Godot;

namespace DeadDog.Nodeo.Tools;

public static class MathTool
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
    #region 变形
    public static Transform2D Transform2DAs3D(this Transform2D origin, Vector3 rot, Vector2 pos,Vector2 sca)
    {
        var rf = 90f.DegToRad();
        var m = new Transform2D(0, Vector2.One, 0, pos);
        var s = new Transform2D(0, sca, 0, Vector2.Zero);
        var ms = m * s;
        
        var x = new Transform2D(0, Vector2.One, rot.X, Vector2.Zero);
        var y = new Transform2D(rf, Vector2.One, rot.Y, Vector2.Zero);
        var z = new Transform2D(rot.Z, Vector2.One, 0, Vector2.Zero);
        var fix = new Transform2D(-rf, Vector2.One, 0, Vector2.Zero);
        var r = x * y * z * fix;

        if (!ms.IsInvertible()) ms = ms.AddEpsilon();
        if (!r.IsInvertible()) r = r.AddEpsilon();

        return origin * ms * r;
    }
    
    public static Transform2D Transform2DAs3DDeg(this Transform2D origin, Vector3 rot, Vector2 pos,Vector2 sca)
    {
        var rotX = rot.X.DegToRad();
        var rotY = rot.Y.DegToRad();
        var rotZ = rot.Z.DegToRad();
        return origin.Transform2DAs3D(new Vector3(rotX, rotY, rotZ), pos, sca);
    }

    public static bool IsInvertible(this Transform2D origin,float epsilon = 1e-6f) => 
        float.Abs(origin.Determinant()) > epsilon;

    public static Transform2D AddEpsilon(this Transform2D t, float epsilon = 1e-3f) =>
        new(
            new(t.X.X + epsilon, t.X.Y + epsilon),
            new(t.Y.X + epsilon, t.Y.Y + epsilon),
            t.Origin
        );
    #endregion
    #endregion
}