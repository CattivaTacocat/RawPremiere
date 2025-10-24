using Godot;

namespace DeadDog.Nodeo.Tools;

/// <summary>
/// 画布工具（来自Nodeo库）
/// </summary>
public static class CanvasTool
{
    #region 操作

    public static void Dyeing(this Color color, params CanvasItem[] items)
    {
        if (items is null || items.Length == 0) return;
        foreach (var item in items)
        {
            if (!ErrForDyeingAndFillingOfItem(item))
                item.SelfModulate = color;
        }
    }

    public static void Dyeing(this string color, params CanvasItem[] items)
    {
        if (ErrForDyeingAndFillingOfStringColor(color)) return;
        Dyeing(Color.FromString(color,Colors.White), items);
    }

    public static void Filling(this Color color, params CanvasItem[] items)
    {
        if (items is null || items.Length == 0) return;
        foreach (var item in items)
        {
            if (!ErrForDyeingAndFillingOfItem(item))
                item.Modulate = color;
        }
    }

    public static void Filling(this string color, params CanvasItem[] items)
    {
        if (ErrForDyeingAndFillingOfStringColor(color)) return;
        Filling(Color.FromString(color,Colors.White), items);
    }

    #endregion

    #region 异常处理

    private static bool ErrForDyeingAndFillingOfItem(CanvasItem item)
    {
        if (item is not null) return false;
        GD.PushError($"{nameof(Dyeing)}:要操作的对象为null");
        return true;
    }

    private static bool ErrForDyeingAndFillingOfStringColor(string color)
    {
        if (Color.HtmlIsValid(color)) return false;
        GD.PushError($"{nameof(Dyeing)}:\"{color}\" 不是有效的颜色代码");
        return true;
    }

    #endregion
}