using Godot;

namespace DeadDog.Nodeo.Tools;

public static class TweenTool
{
    #region 操作
    public static void Destroy(this Tween tween) => tween?.Stop();

    public static Tween CreateFrom(Node node)
    {
        return ErrForCreateFromByNodeNull(node) ? null : node.GetTree().CreateTween();
    }

    public static void ResetTween(ref Tween tween, Node node)
    {
        tween?.Destroy();
        tween = CreateFrom(node);
    }
    #endregion
    #region 异常处理
    private static bool ErrForCreateFromByNodeNull(Node node)
    {
        if (node is not null) return false; 
        GD.PushError($"输入的节点为空");
        return true;
    }
    #endregion
}