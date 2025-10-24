namespace DeadDog.Ordexp;

public static class BoolExpends
{
    #region 决策
    /// <summary>
    /// 能否赋值
    /// </summary>
    /// <param name="origin">原值</param>
    /// <param name="value">新值</param>
    /// <returns>不能赋值则为true，反之为false</returns>
    public static bool CantAssignValue(this bool origin, bool value) => origin == value;
    #endregion
}