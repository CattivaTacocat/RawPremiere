using Godot;
using RawPremiere.Components.Enums;

namespace RawPremiere.Components.Elements.Filters;

public partial class CustomFilterComp : Node, IFilterComp
{
    #region 创建
    public CustomFilterComp()
    {
        _type.Set(FilterTypeEnum.Custom);
        _shader.Set(null!);
        _parameters.Set([]);
    }
    #endregion
    #region 属性
    [Notify,Export] public FilterTypeEnum Type { get => _type.Get(); private set => _type.Set(value); }
    [Notify,Export] public ShaderMaterial Shader { get => _shader.Get(); private set => _shader.Set(value); }
    [Notify] public object[] Parameters { get => _parameters.Get(); set => _parameters.Set(value); }
    #endregion
}