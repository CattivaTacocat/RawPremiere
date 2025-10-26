using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems.Inputs;

public partial class HoverSys : Node
{
    #region 组件和视图
    [Export] public HoverComp HoverComp { get; private set; }

    [Notify]
    [Export]
    public Control View
    {
        get => _view.Get();
        set
        {
            _view.Get().MouseEntered -= RespondMouseEntered;
            _view.Get().MouseExited -= RespondMouseExited;
            _view.Set(value);
            _view.Get().MouseEntered += RespondMouseEntered;
            _view.Get().MouseExited += RespondMouseExited;
        }
    }
    #endregion
    #region 响应
    private void RespondMouseEntered()
        => HoverComp.IsHovered = true;
    private void RespondMouseExited()
        => HoverComp.IsHovered = false;
    #endregion
    #region 操作
    public void SetHovered(bool hovered) => HoverComp.IsHovered = hovered;
    #endregion
}