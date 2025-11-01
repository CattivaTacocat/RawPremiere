namespace RawPremiere.Components.Enums;

/// <summary>
/// 触发器接收模式
/// 代表玩家应该要以什么样的方式去触发触发器
/// </summary>
public enum TriggerReceiveModeEnum
{
    /// <summary>
    /// 悬停
    /// </summary>
    Hover,
    /// <summary>
    /// 离开
    /// </summary>
    Leave,
    /// <summary>
    /// 交互
    /// </summary>
    Enter,
    /// <summary>
    /// 总是激活
    /// </summary>
    Always,
}