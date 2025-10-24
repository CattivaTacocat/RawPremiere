using Godot;

namespace DeadDog.Nodeo.Components.Lines.ConnectingLine;

public class StraightLineTrailCalculator : SimpleLineTrailCalculator
{
    #region 处理
    public override Vector2[] CalcTrail(Vector2 start, Vector2 end) => [start, end];
    #endregion
}