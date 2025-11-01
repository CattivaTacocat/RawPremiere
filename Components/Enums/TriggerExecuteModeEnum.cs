namespace RawPremiere.Components.Enums;

/// <summary>
/// 触发器执行模式
/// 即触发器在激活后以什么样的方式执行命令集
/// </summary>
public enum TriggerExecuteModeEnum
{
    /// <summary>
    /// 脉冲
    /// 激活后只会执行一次
    /// </summary>
    Pulse,
    /// <summary>
    /// 循环
    /// 只要在激活状态就会一直以执行间隔循环执行
    /// </summary>
    Loop,
    /// <summary>
    /// 熔断
    /// 只有第一次激活会使其执行一次
    /// </summary>
    Fuse,
    /// <summary>
    /// 水桶
    /// 只有第一次激活会使其以执行间隔循环执行
    /// </summary>
    Bucket,
}