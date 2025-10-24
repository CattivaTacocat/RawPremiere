using Godot;

namespace DeadDog.Nodeo.Components.Cutto;

public record CuttoDto
{
    /// <summary>
    /// 转场动画持续时间
    /// </summary>
    public float Duration { get; set; }
    /// <summary>
    /// 整体调色
    /// </summary>
    public Color ModulateColor { get; set; }
    /// <summary>
    /// 覆盖图路径
    /// </summary>
    public string OverlayTexturePath { get; set; }
    /// <summary>
    /// 是否平铺
    /// </summary>
    public bool IsTiled { get; set; }
    /// <summary>
    /// 转场图层索引
    /// </summary>
    public int CuttoLayerIndex { get; set; }
}