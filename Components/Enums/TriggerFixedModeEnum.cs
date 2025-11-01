namespace RawPremiere.Components.Enums;

/// <summary>
/// 触发器激活固定模式
/// 相当于在玩家触发触发器后，触发器要以什么样的方式管理自己的激活状态
/// </summary>
public enum TriggerFixedModeEnum
{
    /// <summary>
    /// 踏板
    /// 玩家符合触发器接收模式时，触发器会处于激活状态，玩家不符合触发器接收模式时，触发器会处于非激活状态
    /// </summary>
    Peal,
    /// <summary>
    /// 开关
    /// 玩家符合触发器接收模式的那一刻，触发器的激活状态会取反
    /// </summary>
    Toggle,
    /// <summary>
    /// 发条
    /// 玩家每次只有在触发触发器时会从0开始计时，到达等待时间后触发器会被激活
    /// </summary>
    Spring,
    /// <summary>
    /// 秒表
    /// 玩家触发触发器后，时间会从0累计到等待时间，到达等待时间后触发器会被激活
    /// </summary>
    Watch,
    /// <summary>
    /// 无
    /// 不接受任何触发
    /// </summary>
    None,
}