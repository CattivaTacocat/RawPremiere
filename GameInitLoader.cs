using DeadDog.Ordexp.Configuration;
using Godot;
using DeadDog.RecallPast.Libs.Nodeo.Input;

public partial class GameInitLoader : Node
{
    #region 创建
    public override void _EnterTree()
    {
        GetTree().Root.OversamplingOverride = 16;
        ConfigurationManager.Init();
        TranslationServer.SetLocale("zh_CN");
        QueueFree();
    }
    #endregion
}
