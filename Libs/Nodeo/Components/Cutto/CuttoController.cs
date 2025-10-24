using System;
using System.Threading.Tasks;
using Godot;

namespace DeadDog.Nodeo.Components.Cutto;

public class CuttoController
{
    #region 属性
    public CuttoStyleEnum P_CuttoStyle { get; set; }
    public int P_CuttoAwaitTimeMs { get; set; } = 300;

    public CuttoDto P_CuttoDto { get; set; } = new()
    {
        Duration = 1f,
        ModulateColor = Colors.White,
        OverlayTexturePath = "res://Libs/Nodeo/Assets/Textures/white.tres",
        IsTiled = false,
        CuttoLayerIndex = 1
    };
    #endregion
    #region 操作
    public async Task Cutto(Node source,Func<Task> handleAsync)
    {
        var cutto = CuttoFactory.CreateCutto(P_CuttoStyle);
        source.AddChild(cutto);
        ModifyCutto(cutto);

        var transInTcs = new TaskCompletionSource<bool>();
        var transOutTcs = new TaskCompletionSource<bool>();

        cutto.OnTransInFinished += () => transInTcs.TrySetResult(true);
        cutto.OnTransOutFinished += () => transOutTcs.TrySetResult(true);
        
        cutto.TransIn();
        await transInTcs.Task;
        if (handleAsync is not null) await handleAsync();
        if (P_CuttoAwaitTimeMs > 0) await Task.Delay(P_CuttoAwaitTimeMs);
        cutto.TransOut();
        await transOutTcs.Task;
        
        source.RemoveChild(cutto);
        cutto.QueueFree();
    }
    #endregion
    #region 处理
    private void ModifyCutto(SimpleCutto cutto) => cutto.LoadFromDto(P_CuttoDto);
    #endregion
}
