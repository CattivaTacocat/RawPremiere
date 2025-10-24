using DeadDog.RecallPast.Libs.Nodeo.Input;

namespace DeadDog.RecallPast.Libs.Nodeo.Input;

public record NodeoInputDto
{
    /// <summary>
    /// 输入设备类型
    /// </summary>
    public NodeoInputTypeEnum InputType { get; set; }
    /// <summary>
    /// 输入按键索引
    /// </summary>
    public long InputIndex { get; set; }
    /// <summary>
    /// 轴值
    /// 该值仅对JoypadMotion类型的输入有效
    /// </summary>
    public float? AxisValue { get; set; }
}