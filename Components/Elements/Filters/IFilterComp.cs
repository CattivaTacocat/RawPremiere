using Godot;
using RawPremiere.Components.Enums;

namespace RawPremiere.Components.Elements.Filters;

public interface IFilterComp
{
    #region 属性
    public FilterTypeEnum Type { get; }
    public ShaderMaterial Shader { get; }
    #endregion
}