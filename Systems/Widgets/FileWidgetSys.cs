using Godot;
using RawPremiere.Components;

namespace RawPremiere.Systems;

public partial class FileWidgetSys : WidgetSys<string>
{
    #region 组件
    [Export] public PreviewComp PreviewComp { get; private set; }
    #endregion
    #region 重写
    public override void SetValue(string value)
    {
        WidgetComp.Value = value;
        WidgetComp.DisplayValue = value;
        PreviewComp.Preview = ResourceLoader.Load<Texture2D>(value);
    }
    #endregion
}