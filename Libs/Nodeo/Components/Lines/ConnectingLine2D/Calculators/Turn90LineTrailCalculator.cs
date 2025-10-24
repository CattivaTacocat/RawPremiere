using System;
using DeadDog.Ordexp;
using Godot;

namespace DeadDog.Nodeo.Components.Lines.ConnectingLine;

public class Turn90LineTrailCalculator : SimpleLineTrailCalculator
{
    #region 辅助字段
    private Turn90HLineTrailCalculator _hCalculator = new();
    private Turn90VLineTrailCalculator _vCalculator = new();
    #endregion
    #region 处理
    public override Vector2[] CalcTrail(Vector2 start, Vector2 end)
    {
        if (start.IsEqualApprox(end)) return [];
        var d = end - start;
        if (d.X.IsEqualsApprox(0) || d.Y.IsEqualsApprox(0)) return [start, end];
        var k = Math.Abs(d.Y / d.X);
        if (k > 1) return _hCalculator.CalcTrail(start, end);
        else return _vCalculator.CalcTrail(start, end);
    }
    #endregion
}