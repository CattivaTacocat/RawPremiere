using RawPremiere.Components.Elements;

namespace RawPremiere.Systems;

public partial class ElementWidgetSys : WidgetSys<ElementComp>
{
    #region 重写
    public override void SetValue(ElementComp value)
    {
        WidgetComp.Value = value;
        WidgetComp.DisplayValue = value.ElementName;
    }
    #endregion
}