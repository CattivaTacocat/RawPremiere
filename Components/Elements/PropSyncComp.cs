using Godot;
using RawPremiere.Objects.Elements;

namespace RawPremiere.Components.Elements;

public partial class PropSyncComp : Node
{
    #region 创建
    public PropSyncComp()
    {
        _following.Set(null!);
        _syncProps.Set([]);
    }
    #endregion
    #region 属性
    [Notify] public IElement Following { get => _following.Get(); set => _following.Set(value); }
    [Notify,Export] public string[] SyncProps { get => _syncProps.Get(); set => _syncProps.Set(value); }
    #endregion
}