using DeadDog.Nodeo.Tools;
using Godot;

namespace RawPremiere.Components.Elements;

public partial class HybridCoordinateDegComp : Node
{
    #region 属性
    [Notify,Export] public Vector2 Params { get=>_params.Get(); set => _params.Set(value,ChangeValue); }
    [Notify,Export] public bool IsPolar { get => _isPolar.Get(); set => _isPolar.Set(value,ChangeValue); }
    public Vector2 Position { get; private set; }
    #endregion
    #region 响应
    public void ChangeValue() => Position = IsPolar ? Params.FromDegPolarAsCartesian() : Params;
    #endregion
    #region 操作
    public void FixValue() => _params.Set(Position);
    #endregion
}