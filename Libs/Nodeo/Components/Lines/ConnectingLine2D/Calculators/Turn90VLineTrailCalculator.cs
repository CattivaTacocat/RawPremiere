using DeadDog.Ordexp;
using Godot;

namespace DeadDog.Nodeo.Components.Lines.ConnectingLine;

public class Turn90VLineTrailCalculator : SimpleLineTrailCalculator
{
    #region 处理
    public override Vector2[] CalcTrail(Vector2 start, Vector2 end)
    {
        if (start.IsEqualApprox(end)) return [];
        var d = end - start;
        if (d.Y.IsEqualsApprox(0)) return [start, end];
        var mid = (start + end) / 2;
        var ns = new Vector2(mid.X, start.Y);
        var ne = new Vector2(mid.X, end.Y);
        return [start, ns, ne, end];
    }
    #endregion
}