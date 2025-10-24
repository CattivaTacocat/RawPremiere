using Godot;
using System;

namespace DeadDog;

public partial class VerticalRuler : ColorRect
{
    #region 属性字段
    private float _maxCoordinate = 1000.0f;
    private float _minCoordinate = -1000.0f;
    private Color _markingColor = Colors.Black;
    private Color _backgroundColor = new Color(0.95f, 0.95f, 0.95f);
    private float _centerCoordinate = 0.0f;
    private MarkingDirection _markingDirection = MarkingDirection.Right;
    private Font _labelFont;
    private float _currentZoom = 1.0f;
    #endregion
    #region 方向
    public enum MarkingDirection
    {
        Left,
        Right,
        Both
    }
    #endregion
    #region 属性
    [ExportGroup("标尺设置")]
    [Export]
    public float P_MaxCoordinate
    {
        get => _maxCoordinate;
        set { _maxCoordinate = Math.Max(value, _minCoordinate + 1); QueueRedraw(); }
    }

    [Export]
    public float P_MinCoordinate
    {
        get => _minCoordinate;
        set { _minCoordinate = Math.Min(value, _maxCoordinate - 1); QueueRedraw(); }
    }

    [Export]
    public Color P_MarkingColor
    {
        get => _markingColor;
        set { _markingColor = value; QueueRedraw(); }
    }

    [Export]
    public Color P_BackgroundColor
    {
        get => _backgroundColor;
        set { _backgroundColor = value; QueueRedraw(); }
    }

    [Export]
    public float P_CenterCoordinate
    {
        get => _centerCoordinate;
        set
        {
            var bounds = GetCurrentBounds();
            _centerCoordinate = Mathf.Clamp(value, bounds.bottomBound, bounds.topBound);
            QueueRedraw();
        }
    }

    [Export]
    public MarkingDirection P_MarkingDirection
    {
        get => _markingDirection;
        set { _markingDirection = value; QueueRedraw(); }
    }

    [Export]
    public float P_CurrentZoom
    {
        get => _currentZoom;
        set
        {
            var oldZoom = _currentZoom;
            _currentZoom = Mathf.Clamp(value, P_MinZoom, P_MaxZoom);

            if (oldZoom > 0 && _currentZoom > 0)
            {
                var bounds = GetCurrentBounds();
                _centerCoordinate = Mathf.Clamp(_centerCoordinate, bounds.bottomBound, bounds.topBound);
            }
            QueueRedraw();
        }
    }

    [Export] public float P_MinZoom { get; set; } = 1.0f;
    [Export] public float P_MaxZoom { get; set; } = 100.0f;

    [ExportGroup("标签设置")]
    [Export]
    public Font P_LabelFont
    {
        get => _labelFont;
        set
        {
            _labelFont = value;
            QueueRedraw();
        }
    }

    [Export]
    public int P_LabelFontSize { get; set; } = 10;

    [Export]
    public Color P_LabelColor { get; set; } = Colors.Black;

    [Export]
    public HorizontalAlignment P_LabelAlignment { get; set; } = HorizontalAlignment.Left;
    #endregion
    #region 边界计算
    public (float bottomBound, float topBound) GetCurrentBounds()
    {
        float visibleRange = (_maxCoordinate - _minCoordinate) / _currentZoom;
        float bottomBound = _minCoordinate + visibleRange / 2;
        float topBound = _maxCoordinate - visibleRange / 2;

        if (bottomBound > topBound)
        {
            float center = (_minCoordinate + _maxCoordinate) / 2;
            return (center, center);
        }

        return (bottomBound, topBound);
    }

    private bool IsBeyondBounds(float proposedCenter)
    {
        var bounds = GetCurrentBounds();
        return proposedCenter < bounds.bottomBound || proposedCenter > bounds.topBound;
    }

    public (float start, float end) GetVisibleRange()
    {
        float visibleRange = (_maxCoordinate - _minCoordinate) / _currentZoom;
        float start = _centerCoordinate - visibleRange / 2;
        float end = _centerCoordinate + visibleRange / 2;
        return (start, end);
    }

    public bool CanMove(float delta)
    {
        float newCenter = _centerCoordinate + delta / _currentZoom;
        return !IsBeyondBounds(newCenter);
    }

    public bool CanZoom(float zoomFactor)
    {
        float newZoom = _currentZoom * zoomFactor;
        newZoom = Mathf.Clamp(newZoom, P_MinZoom, P_MaxZoom);
        float visibleRange = (_maxCoordinate - _minCoordinate) / newZoom;
        return visibleRange > 0 && visibleRange <= (_maxCoordinate - _minCoordinate);
    }
    #endregion
    #region 创建
    public override void _Ready()
    {
        _labelFont ??= ThemeDB.FallbackFont;
        Resized += OnResized;
    }

    private void OnResized()
    {
        QueueRedraw();
    }

    public override void _Draw()
    {
        DrawRulerBackground();
        DrawScale();
        DrawBoundaryIndicators();
    }
    #endregion
    #region 视图
    private void DrawRulerBackground()
    {
        DrawRect(new Rect2(Vector2.Zero, Size), _backgroundColor);
    }

    private void DrawScale()
    {
        if (Size.X <= 0 || Size.Y <= 0) return;

        float visibleRange = (_maxCoordinate - _minCoordinate) / _currentZoom;
        float visibleStart = _centerCoordinate - visibleRange / 2;
        float visibleEnd = _centerCoordinate + visibleRange / 2;
        float sizeX = Size.X;
        float sizeY = Size.Y;

        float pixelsPerUnit = sizeY / visibleRange;

        float baseInterval = CalculateBaseInterval(pixelsPerUnit);
        baseInterval = Mathf.Min(baseInterval, visibleRange / 5f);
        baseInterval = Mathf.Max(baseInterval, 10f);

        float majorInterval = baseInterval;
        float mediumInterval = baseInterval / 2;
        float minorInterval = baseInterval / 10;

        majorInterval = Mathf.Max(majorInterval, 10f);
        mediumInterval = Mathf.Max(mediumInterval, 5f);
        minorInterval = Mathf.Max(minorInterval, 1f);

        float startPos = FindFirstMarking(visibleStart, minorInterval);

        int tickCount = 0;
        for (float logicalPos = startPos; logicalPos <= visibleEnd + minorInterval; logicalPos += minorInterval)
        {
            if (tickCount++ > 1000) break;

            if (logicalPos < _minCoordinate || logicalPos > _maxCoordinate) continue;

            float normalized = (logicalPos - visibleStart) / visibleRange;
            float pixelY = normalized * sizeY;

            if (!(pixelY >= -1f) || !(pixelY <= sizeY + 1f)) continue;

            var level = DetermineTickLevel(logicalPos, majorInterval, mediumInterval, minorInterval);
            DrawMarking(pixelY, level, logicalPos, sizeX, baseInterval);
        }

        if (tickCount == 0)
        {
            DrawCenterLine(sizeX, sizeY);
        }
    }

    private void DrawBoundaryIndicators()
    {
        var bounds = GetCurrentBounds();
        bool atBottomBound = Mathf.Abs(_centerCoordinate - bounds.bottomBound) < 0.001f;
        bool atTopBound = Mathf.Abs(_centerCoordinate - bounds.topBound) < 0.001f;

        if (atBottomBound || atTopBound)
        {
            Color boundaryColor = new Color(1, 0.3f, 0.3f, 0.4f);
            float indicatorHeight = 3f;

            if (atBottomBound)
            {
                DrawRect(new Rect2(0, 0, Size.X, indicatorHeight), boundaryColor);
            }

            if (atTopBound)
            {
                DrawRect(new Rect2(0, Size.Y - indicatorHeight, Size.X, indicatorHeight), boundaryColor);
            }
        }
    }

    private void DrawCenterLine(float sizeX, float sizeY)
    {
        float centerY = sizeY / 2f;
        float xStart, xEnd;

        switch (_markingDirection)
        {
            case MarkingDirection.Left:
                xStart = sizeX;
                xEnd = sizeX * 0.2f;
                break;
            case MarkingDirection.Right:
                xStart = 0;
                xEnd = sizeX * 0.8f;
                break;
            case MarkingDirection.Both:
            default:
                float centerX = sizeX / 2f;
                xStart = centerX - sizeX * 0.4f;
                xEnd = centerX + sizeX * 0.4f;
                break;
        }

        DrawLine(new Vector2(xStart, centerY), new Vector2(xEnd, centerY), _markingColor, 2f);
    }

    private float CalculateBaseInterval(float pixelsPerUnit)
    {
        float targetPixelSpacing = 80f;
        float baseInterval = targetPixelSpacing / pixelsPerUnit;
        baseInterval = RoundToMultipleOfTen(baseInterval);
        return Mathf.Clamp(baseInterval, 10f, 10000f);
    }

    private float RoundToMultipleOfTen(float value)
    {
        if (value <= 10f) return 10f;
        float rounded = Mathf.Round(value / 10f) * 10f;
        return rounded;
    }

    private MarkingLevel DetermineTickLevel(float logicalPos, float majorInterval, float mediumInterval, float minorInterval)
    {
        float tolerance = minorInterval * 0.01f;
        float majorRemainder = Mathf.Abs(logicalPos % majorInterval);
        if (majorRemainder < tolerance || majorRemainder > majorInterval - tolerance)
            return MarkingLevel.Major;
        float mediumRemainder = Mathf.Abs(logicalPos % mediumInterval);
        if (mediumRemainder < tolerance || mediumRemainder > mediumInterval - tolerance)
            return MarkingLevel.Medium;
        return MarkingLevel.Minor;
    }

    private enum MarkingLevel { Major, Medium, Minor }

    private void DrawMarking(float y, MarkingLevel level, float logicalValue, float sizeX, float baseInterval)
    {
        float tickWidth;
        float lineWidth;
        switch (level)
        {
            case MarkingLevel.Major:
                tickWidth = sizeX;
                lineWidth = 2.0f;
                break;
            case MarkingLevel.Medium:
                tickWidth = sizeX * 0.7f;
                lineWidth = 1.5f;
                break;
            case MarkingLevel.Minor:
            default:
                tickWidth = sizeX * 0.4f;
                lineWidth = 1.0f;
                break;
        }

        float xStart, xEnd;
        switch (_markingDirection)
        {
            case MarkingDirection.Left:
                xStart = sizeX;
                xEnd = sizeX - tickWidth;
                break;
            case MarkingDirection.Right:
                xStart = 0;
                xEnd = tickWidth;
                break;
            case MarkingDirection.Both:
            default:
                float centerX = sizeX / 2f;
                float halfWidth = tickWidth / 2f;
                xStart = centerX - halfWidth;
                xEnd = centerX + halfWidth;
                break;
        }

        DrawLine(new Vector2(xStart, y), new Vector2(xEnd, y), _markingColor, lineWidth);

        if (level == MarkingLevel.Major && _labelFont != null)
        {
            DrawMarkingLabel(y, logicalValue, baseInterval);
        }
    }

    private void DrawMarkingLabel(float y, float logicalValue, float baseInterval)
    {
        string label = FormatLabel(logicalValue, baseInterval);
        var stringSize = _labelFont.GetStringSize(label, HorizontalAlignment.Left, -1, P_LabelFontSize);
        Vector2 labelPos = CalculateLabelPosition(y, stringSize);
        var originalTransform = GetCanvasTransform();
        var rotationTransform = new Transform2D(-Mathf.Pi / 2, labelPos);
        DrawSetTransformMatrix(rotationTransform);
        DrawString(_labelFont, Vector2.Zero, label, HorizontalAlignment.Left, -1, P_LabelFontSize, P_LabelColor);
        DrawSetTransformMatrix(originalTransform);
    }

    private Vector2 CalculateLabelPosition(float y, Vector2 stringSize)
    {
        float horizontalPosition = 0;
        switch (P_LabelAlignment)
        {
            case HorizontalAlignment.Center:
                horizontalPosition = Size.X / 2;
                break;
            case HorizontalAlignment.Right:
                horizontalPosition = Size.X - stringSize.Y - 2;
                break;
            case HorizontalAlignment.Left:
            default:
                horizontalPosition = 2 + stringSize.Y - 7; 
                break;
        }
        float verticalPosition = y - stringSize.X / 2;

        return new Vector2(horizontalPosition, verticalPosition);
    }
    private string FormatLabel(float value, float baseInterval)
    {
        string formatted = value.ToString("0.##");
        return formatted;
    }

    private float FindFirstMarking(float visibleStart, float interval)
    {
        if (interval <= 0) return visibleStart;
        float ticksBeforeStart = Mathf.Floor(visibleStart / interval);
        return ticksBeforeStart * interval;
    }
    #endregion
    #region 操作
    /// <summary>
    /// 移动标尺视图
    /// </summary>
    public void Move(float delta)
    {
        float proposedCenter = _centerCoordinate + delta / _currentZoom;

        var bounds = GetCurrentBounds();
        _centerCoordinate = Mathf.Clamp(proposedCenter, bounds.bottomBound, bounds.topBound);

        QueueRedraw();
    }

    /// <summary>
    /// 重置标尺到初始状态（中心坐标为0，缩放为1）
    /// </summary>
    public void Reset()
    {
        _centerCoordinate = 0.0f;
        _currentZoom = 1.0f;
        QueueRedraw();
    }

    /// <summary>
    /// 设置标尺刻度的绘制方向
    /// </summary>
    public void SetMarkingDirection(MarkingDirection direction)
    {
        _markingDirection = direction;
        QueueRedraw();
    }

    /// <summary>
    /// 设置缩放级别，基于指定的缩放中心点
    /// </summary>
    public void SetZoom(float zoomLevel, Vector2 zoomCenter)
    {
        float oldZoom = _currentZoom;
        float newZoom = Mathf.Clamp(zoomLevel, P_MinZoom, P_MaxZoom);

        if (oldZoom > 0 && newZoom > 0)
        {
            float zoomRatio = newZoom / oldZoom;
            float proposedCenter = zoomCenter.Y + (_centerCoordinate - zoomCenter.Y) / zoomRatio;

            float visibleRange = (_maxCoordinate - _minCoordinate) / newZoom;
            float bottomBound = _minCoordinate + visibleRange / 2;
            float topBound = _maxCoordinate - visibleRange / 2;

            _currentZoom = newZoom;
            _centerCoordinate = Mathf.Clamp(proposedCenter, bottomBound, topBound);
        }

        QueueRedraw();
    }

    /// <summary>
    /// 基于当前缩放级别进行缩放
    /// </summary>
    public void Zoom(float zoomFactor, Vector2 zoomCenter)
    {
        SetZoom(_currentZoom * zoomFactor, zoomCenter);
    }

    /// <summary>
    /// 基于当前中心点进行缩放
    /// </summary>
    public void Zoom(float zoomFactor)
    {
        Zoom(zoomFactor, new Vector2(0, _centerCoordinate));
    }

    /// <summary>
    /// 设置缩放级别，基于当前中心点
    /// </summary>
    public void SetZoom(float zoomLevel)
    {
        SetZoom(zoomLevel, new Vector2(0, _centerCoordinate));
    }

    /// <summary>
    /// 直接设置可见范围（起点和终点）
    /// </summary>
    public void SetVisibleRange(float start, float end)
    {
        float newCenter = (start + end) / 2;
        float requiredZoom = (_maxCoordinate - _minCoordinate) / (end - start);

        _currentZoom = Mathf.Clamp(requiredZoom, P_MinZoom, P_MaxZoom);

        var bounds = GetCurrentBounds();
        _centerCoordinate = Mathf.Clamp(newCenter, bounds.bottomBound, bounds.topBound);

        QueueRedraw();
    }
    #endregion
}