using DeadDog.Nodeo.Tools;
using DeadDog.Ordexp;
using Godot;

namespace DeadDog.Nodeo.Components.Lines.ConnectingLine;

public partial class ConnectingLine2D : Node2D
{
    #region 属性字段
    private Vector2 _startPoint;
    private Vector2 _endPoint;
    private float _width = 8;
    private Texture2D _startIcon = new();
    private Texture2D _endIcon = new();
    private ConnectingLineStyleEnum _style;
    #endregion
    #region 属性
    [Export] public Vector2 StartPoint
    {
        get => _startPoint;
        set
        {
            if (_startPoint.Equals(value)) return;
            _startPoint = value;
            UpdateLineView();
        }
    }
    
    [Export] public Vector2 EndPoint
    {
        get => _endPoint;
        set
        {
            if (_endPoint.Equals(value)) return;
            _endPoint = value;
            UpdateLineView();
        }
    }
    
    [Export] public float Width
    {
        get => _width;
        set
        {
            if (_width.CantAssignValue(value)) return;
            _width = value;
            UpdateWidthView();
        }
    }
    
    [Export] public Texture2D StartIcon
    {
        get => _startIcon;
        set
        {
            if (_startIcon.CantAssignValue(value,true)) return;
            _startIcon = value;
            UpdateStartIconView();
        }
    }
    
    [Export] public Texture2D EndIcon
    {
        get => _endIcon;
        set
        {
            if (_endIcon.CantAssignValue(value,true)) return;
            _endIcon = value;
            UpdateEndIconView();
        }
    }
    
    [Export] public ConnectingLineStyleEnum Style
    {
        get => _style;
        set
        {
            if (_style.CantAssignValue(value)) return;
            _style = value;
            UpdateStyleView();
        }
    }
    #endregion
    #region 辅助字段
    private SimpleLineTrailCalculator _calculator;
    #endregion
    #region 节点
    [ExportGroup("Nodes")]
    [Export] public Line2D N_Line { get; private set; }
    [Export] public Sprite2D N_StartIcon { get; private set; }
    [Export] public Sprite2D N_EndIcon { get; private set; }
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
        UpdateStartIconView();
        UpdateEndIconView();
        UpdateStyleView();
    }
    
    private void UpdateLineView()
    {
        if (!IsInstanceValid(N_Line)) return;
        if (!IsInstanceValid(N_EndIcon)) return;
        if (!IsInstanceValid(N_StartIcon)) return;
        N_Line.Points = _calculator.CalcTrail(StartPoint, EndPoint);
        N_EndIcon.Position = EndPoint;
        N_StartIcon.Position = StartPoint;
    }

    private void UpdateWidthView()
    {
        if (!IsInstanceValid(N_Line)) return;
        N_Line.Width = Width;
    }

    private void UpdateStartIconView()
    {
        if (!IsInstanceValid(N_StartIcon)) return;
        N_StartIcon.Texture = StartIcon.GetSvgTexture();
    }
    
    private void UpdateEndIconView()
    {
        if (!IsInstanceValid(N_EndIcon)) return;
        N_EndIcon.Texture = EndIcon.GetSvgTexture();
    }

    private void UpdateStyleView()
    {
        _calculator = LineTrailCalculatorFactory.CreateCalculator(Style);
        UpdateLineView();
    }
    #endregion
}
