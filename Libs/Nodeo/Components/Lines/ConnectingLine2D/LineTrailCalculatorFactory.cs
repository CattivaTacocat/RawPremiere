
using System.Collections.Generic;

namespace DeadDog.Nodeo.Components.Lines.ConnectingLine;

public class LineTrailCalculatorFactory
{
    #region 辅助字段
    private static Dictionary<ConnectingLineStyleEnum, SimpleLineTrailCalculator> _calculatorsDic = new()
    {
        { ConnectingLineStyleEnum.Unknown , new SimpleLineTrailCalculator()},
        { ConnectingLineStyleEnum.Straight , new StraightLineTrailCalculator()},
        { ConnectingLineStyleEnum.Turn135 , new Turn135LineTrailCalculator()},
        { ConnectingLineStyleEnum.Turn90H , new Turn90HLineTrailCalculator()},
        { ConnectingLineStyleEnum.Turn90V , new Turn90VLineTrailCalculator()},
        { ConnectingLineStyleEnum.Turn90 , new Turn90LineTrailCalculator()},
        { ConnectingLineStyleEnum.Bezier , new BezierLineTrailCalculator()},
        { ConnectingLineStyleEnum.Count , new SimpleLineTrailCalculator()},
    };
    #endregion
    #region 操作
    public static SimpleLineTrailCalculator CreateCalculator(ConnectingLineStyleEnum style)
    {
        return _calculatorsDic.TryGetValue(style, out var calculator) ?
            calculator : _calculatorsDic[ConnectingLineStyleEnum.Unknown];
    }
    #endregion
}