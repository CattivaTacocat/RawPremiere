using Godot;
using System;
using DeadDog.Ordexp;

namespace DeadDog.Nodeo.Components.Lines.DashLine;

public partial class DashLine2D : Node2D
{
    #region 辅助字段
    private Vector2[] _points = [];
    private Color _defaultColor = Colors.White;
    private float _width = 8;
    private float _dash = 50;
    #endregion
    #region 属性
    [Export] public Vector2[] Points
    {
        get => _points;
        set
        {
            if (_points.CantAssignValue(value)) return;
            _points = value;
            QueueRedraw();
        }
    }
    [Export] public Color DefaultColor
    {
        get => _defaultColor;
        set
        {
            if (value == _defaultColor) return;
            _defaultColor = value;
            QueueRedraw();
        }
    }
    [Export] public float Width
    {
        get => _width;
        set
        {
            if (_width.CantAssignValue(value)) return;
            _width = value;
            QueueRedraw();
        }
    }
    [Export] public float Dash
    {
        get => _dash;
        set
        {
            if (_dash.CantAssignValue(value)) return;
            _dash = value;
            QueueRedraw();
        }
    }
    #endregion
    #region 创建
    public override void _Ready()
    {
        UpdateView();
    }
    #endregion
    #region 视图
    public void UpdateView()
    {
        QueueRedraw();
    }
    
    public override void _Draw()
    {
        if (Points == null || Points.Length < 2)
            return;

        var length = Points.Length;
        for (int i = 1; i < length; i++)
        {
            var from = Points[i - 1];
            var to = Points[i];
            DrawDashedLine(from, to, DefaultColor, Width, Dash);
        }
    }
    #endregion
    #region 操作
    public void Reset()
    {
        Points = [];
    }
    #endregion
}
