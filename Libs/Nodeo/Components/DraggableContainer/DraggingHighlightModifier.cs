using Godot;

namespace RecallPast.Libs.Nodeo.Components.DraggableContainer;

public class DraggingHighlightModifier
{
    #region 辅助字段
    private CanvasLayer _canvas;

    private Line2D _line;
    private ColorRect _rect;
    #endregion
    #region 创建
    public DraggingHighlightModifier(CanvasLayer canvas)
    {
        _canvas = canvas;
    }
    #endregion
    #region 操作
    public void StartDrawRect(Rect2 rect, Color color)
    {
        _rect = new ColorRect();
        _rect.Position = rect.Position;
        _rect.Size = rect.Size;
        _rect.Color = color;
        _canvas.AddChild(_rect);
    }

    public void DrawingRect(Vector2 pos)
    {
        if (_rect is null) return;
        _rect.Position = pos - _rect.Size / 2;
    }
    
    public void EndDrawRect()
    {
        if (_rect is null) return;
        _canvas.RemoveChild(_rect);
        _rect.QueueFree();
        _rect = null;
    }

    public void StartDrawLine(float length, Color color, float width = 4)
    {
        _line = new Line2D();
        _line.Position = Vector2.Inf;
        _line.Points = [Vector2.Zero, Vector2.Down * length];
        _line.Width = width;
        _line.DefaultColor = color;
        _canvas.AddChild(_line);
    }

    public void DrawingLine(Vector2 pos)
    {
        if (_line is null) return;
        _line.GlobalPosition = pos;
    }
    
    public void EndDrawLine()
    {
        if (_line is null) return;
        _canvas.RemoveChild(_line);
        _line.QueueFree();
        _line = null;
    }
    #endregion
}