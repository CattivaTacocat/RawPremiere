using DeadDog.Ordexp;
using Godot;

namespace DeadDog.Nodeo.Components.Lines.ConnectingLine;

public class Turn90HLineTrailCalculator : SimpleLineTrailCalculator
{
    public override Vector2[] CalcTrail(Vector2 start, Vector2 end)
    {
        if (start.IsEqualApprox(end)) return [];
        var d = end - start;
        if (d.X.IsEqualsApprox(0)) return [start, end];
        var mid = (start + end) / 2;
        var ns = new Vector2(start.X, mid.Y);
        var ne = new Vector2(end.X, mid.Y);
        return [start, ns, ne, end];
    }
}