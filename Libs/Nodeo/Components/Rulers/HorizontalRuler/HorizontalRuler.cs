using Godot;
using System;

namespace DeadDog;

public partial class HorizontalRuler : ColorRect
{
    #region 属性字段
    private float _maxCoordinate = 1000.0f;
    private float _minCoordinate = -1000.0f;
    private Color _markingColor = Colors.Black;
    private Color _backgroundColor = new Color(0.95f, 0.95f, 0.95f);
    private float _centerCoordinate = 0.0f;
    private MarkingDirection _markingDirection = MarkingDirection.Up;
    private Font _labelFont;
    private float _currentZoom = 1.0f;
    private RulerMode _currentMode = RulerMode.Normal;
    private IRulerStrategy _currentStrategy;
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
            _centerCoordinate = Mathf.Clamp(value, bounds.leftBound, bounds.rightBound);
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
                _centerCoordinate = Mathf.Clamp(_centerCoordinate, bounds.leftBound, bounds.rightBound);
            }
            QueueRedraw();
        }
    }

    [Export] public float P_MinZoom { get; set; } = 1.0f;
    [Export] public float P_MaxZoom { get; set; } = 100.0f;

    [Export]
    public RulerMode P_CurrentMode
    {
        get => _currentMode;
        set
        {
            _currentMode = value;
            UpdateStrategy();
            OnModeChanged();
            QueueRedraw();
        }
    }

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
    #region 管理
    public void UpdateStrategy()
    {
        _currentStrategy = CreateStrategyForMode(_currentMode);
    }

    protected virtual IRulerStrategy CreateStrategyForMode(RulerMode mode)
    {
        return mode switch
        {
            RulerMode.TimelineMilliseconds => new MillisecondsTimelineStrategy(),
            RulerMode.TimelineSeconds => new SecondsTimelineStrategy(),
            RulerMode.TimelineMinutes => new MinutesTimelineStrategy(),
            RulerMode.TimelineHours => new HoursTimelineStrategy(),
            _ => new NormalRulerStrategy()
        };
    }
    #endregion
    #region 受保护成员（供子类访问）
    protected IRulerStrategy CurrentStrategy => _currentStrategy;
    protected float FindFirstMarkingProtected(float visibleStart, float interval) => FindFirstMarking(visibleStart, interval);
    protected MarkingLevel DetermineTickLevelProtected(float logicalPos, float majorInterval, float mediumInterval, float minorInterval)
        => DetermineTickLevel(logicalPos, majorInterval, mediumInterval, minorInterval);
    protected void DrawMarkingProtected(float x, MarkingLevel level, float logicalValue, float sizeY, float baseInterval)
        => DrawMarking(x, level, logicalValue, sizeY, baseInterval);
    protected void DrawMarkingLabelProtected(float x, float logicalValue, float baseInterval)
        => DrawMarkingLabel(x, logicalValue, baseInterval);
    #endregion
    #region 处理
    protected virtual void OnModeChanged()
    {
        if (_currentMode == RulerMode.Normal)
        {
            P_MinCoordinate = -1000.0f;
            P_MaxCoordinate = 1000.0f;
            P_BackgroundColor = new Color(0.95f, 0.95f, 0.95f);
            P_MarkingColor = Colors.Black;
            P_LabelColor = Colors.Black;
        }
    }
    #endregion
    #region 边界计算
    public (float leftBound, float rightBound) GetCurrentBounds()
    {
        float visibleRange = (_maxCoordinate - _minCoordinate) / _currentZoom;

        if (visibleRange <= 0)
        {
            float center = (_minCoordinate + _maxCoordinate) / 2;
            return (center, center);
        }

        float leftBound = _minCoordinate + visibleRange / 2;
        float rightBound = _maxCoordinate - visibleRange / 2;

        if (leftBound > rightBound)
        {
            float center = (_minCoordinate + _maxCoordinate) / 2;
            return (center, center);
        }

        return (leftBound, rightBound);
    }

    private bool IsBeyondBounds(float proposedCenter)
    {
        var bounds = GetCurrentBounds();
        return proposedCenter < bounds.leftBound || proposedCenter > bounds.rightBound;
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
    #region 生命周期
    public override void _Ready()
    {
        if (_labelFont == null)
        {
            _labelFont = ThemeDB.FallbackFont;
        }
        UpdateStrategy();
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
    public void DrawRulerBackground()
    {
        DrawRect(new Rect2(Vector2.Zero, Size), _backgroundColor);
    }

    private void DrawScale()
    {
        if (Size.X <= 0 || Size.Y <= 0) return;

        float visibleRange = (_maxCoordinate - _minCoordinate) / _currentZoom;

        if (visibleRange <= 0.001f)
        {
            DrawCenterLine(Size.X, Size.Y);
            return;
        }

        float visibleStart = _centerCoordinate - visibleRange / 2;
        float visibleEnd = _centerCoordinate + visibleRange / 2;
        float sizeX = Size.X;
        float sizeY = Size.Y;

        float pixelsPerUnit = sizeX / visibleRange;

        float baseInterval = _currentStrategy.CalculateBaseInterval(pixelsPerUnit);

        baseInterval = Mathf.Clamp(baseInterval, _currentStrategy.GetMinimalInterval(),
            Mathf.Min(_currentStrategy.GetMaximalInterval(), visibleRange / 4f));

        float majorInterval = baseInterval;
        float mediumInterval = baseInterval / 2f;
        float minorInterval = _currentStrategy.GetMinorInterval(baseInterval);

        minorInterval = Mathf.Max(minorInterval, _currentStrategy.GetMinimalInterval());

        float startPos = FindFirstMarking(visibleStart, minorInterval);

        int tickCount = 0;
        bool hasVisibleTicks = false;

        for (float logicalPos = startPos; logicalPos <= visibleEnd + minorInterval; logicalPos += minorInterval)
        {
            if (tickCount++ > 2000) break;

            if (logicalPos < _minCoordinate || logicalPos > _maxCoordinate) continue;

            float normalized = (logicalPos - visibleStart) / visibleRange;
            float pixelX = normalized * sizeX;

            if (pixelX < -10f || pixelX > sizeX + 10f) continue;

            var level = DetermineTickLevel(logicalPos, majorInterval, mediumInterval, minorInterval);
            DrawMarking(pixelX, level, logicalPos, sizeY, baseInterval);
            hasVisibleTicks = true;
        }
        if (!hasVisibleTicks)
        {
            DrawCenterLine(sizeX, sizeY);
        }
    }

    public void DrawBoundaryIndicators()
    {
        var bounds = GetCurrentBounds();
        bool atLeftBound = Mathf.Abs(_centerCoordinate - bounds.leftBound) < 0.001f;
        bool atRightBound = Mathf.Abs(_centerCoordinate - bounds.rightBound) < 0.001f;

        if (atLeftBound || atRightBound)
        {
            Color boundaryColor = new Color(1, 0.3f, 0.3f, 0.4f);
            float indicatorWidth = 3f;

            if (atLeftBound)
            {
                DrawRect(new Rect2(0, 0, indicatorWidth, Size.Y), boundaryColor);
            }

            if (atRightBound)
            {
                DrawRect(new Rect2(Size.X - indicatorWidth, 0, indicatorWidth, Size.Y), boundaryColor);
            }
        }
    }

    public void DrawCenterLine(float sizeX, float sizeY)
    {
        float centerX = sizeX / 2f;
        float yStart, yEnd;

        switch (_markingDirection)
        {
            case MarkingDirection.Up:
                yStart = sizeY;
                yEnd = sizeY * 0.2f;
                break;
            case MarkingDirection.Down:
                yStart = 0;
                yEnd = sizeY * 0.8f;
                break;
            case MarkingDirection.Both:
            default:
                float centerY = sizeY / 2f;
                yStart = centerY - sizeY * 0.4f;
                yEnd = centerY + sizeY * 0.4f;
                break;
        }

        DrawLine(new Vector2(centerX, yStart), new Vector2(centerX, yEnd), _markingColor, 2f);
    }

    public MarkingLevel DetermineTickLevel(float logicalPos, float majorInterval, float mediumInterval, float minorInterval)
    {
        float tolerance = minorInterval * 0.01f;

        float majorRemainder = Mathf.Abs(logicalPos % majorInterval);
        if (majorRemainder < tolerance || majorRemainder > majorInterval - tolerance)
            return MarkingLevel.Major;

        if (Mathf.Abs(mediumInterval) > 0.001f)
        {
            float mediumRemainder = Mathf.Abs(logicalPos % mediumInterval);
            if (mediumRemainder < tolerance || mediumRemainder > mediumInterval - tolerance)
                return MarkingLevel.Medium;
        }

        return MarkingLevel.Minor;
    }

    private void DrawMarking(float x, MarkingLevel level, float logicalValue, float sizeY, float baseInterval)
    {
        float tickHeight;
        float lineWidth;
        switch (level)
        {
            case MarkingLevel.Major:
                tickHeight = sizeY;
                lineWidth = 2.0f;
                break;
            case MarkingLevel.Medium:
                tickHeight = sizeY * 0.7f;
                lineWidth = 1.5f;
                break;
            case MarkingLevel.Minor:
            default:
                tickHeight = sizeY * 0.4f;
                lineWidth = 1.0f;
                break;
        }

        float yStart, yEnd;
        switch (_markingDirection)
        {
            case MarkingDirection.Up:
                yStart = sizeY;
                yEnd = sizeY - tickHeight;
                break;
            case MarkingDirection.Down:
                yStart = 0;
                yEnd = tickHeight;
                break;
            case MarkingDirection.Both:
            default:
                float centerY = sizeY / 2f;
                float halfHeight = tickHeight / 2f;
                yStart = centerY - halfHeight;
                yEnd = centerY + halfHeight;
                break;
        }

        DrawLine(new Vector2(x, yStart), new Vector2(x, yEnd), _markingColor, lineWidth);

        if (level == MarkingLevel.Major && _labelFont != null)
        {
            DrawMarkingLabel(x, logicalValue, baseInterval);
        }
    }

    private void DrawMarkingLabel(float x, float logicalValue, float baseInterval)
    {
        string label = _currentStrategy.FormatLabel(logicalValue, baseInterval);

        Vector2 labelPos;
        var stringSize = _labelFont.GetStringSize(label, HorizontalAlignment.Left, -1, P_LabelFontSize);
        float verticalPosition = Size.Y * 0.5f;
        verticalPosition = Mathf.Min(verticalPosition, Size.Y - stringSize.Y - 2);

        switch (P_LabelAlignment)
        {
            case HorizontalAlignment.Center:
                labelPos = new Vector2(x - stringSize.X / 2, verticalPosition);
                break;
            case HorizontalAlignment.Right:
                labelPos = new Vector2(x - stringSize.X - 2, verticalPosition);
                break;
            case HorizontalAlignment.Left:
            default:
                labelPos = new Vector2(x + 2, verticalPosition);
                break;
        }

        if (labelPos.X >= 0 && labelPos.X + stringSize.X <= Size.X &&
            labelPos.Y >= 0 && labelPos.Y + stringSize.Y <= Size.Y)
        {
            DrawString(_labelFont, labelPos, label, P_LabelAlignment, -1, P_LabelFontSize, P_LabelColor);
        }
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
        _centerCoordinate = Mathf.Clamp(proposedCenter, bounds.leftBound, bounds.rightBound);

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
            float proposedCenter = zoomCenter.X + (_centerCoordinate - zoomCenter.X) / zoomRatio;

            float visibleRange = (_maxCoordinate - _minCoordinate) / newZoom;
            float leftBound = _minCoordinate + visibleRange / 2;
            float rightBound = _maxCoordinate - visibleRange / 2;

            _currentZoom = newZoom;
            _centerCoordinate = Mathf.Clamp(proposedCenter, leftBound, rightBound);
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
        Zoom(zoomFactor, new Vector2(_centerCoordinate, 0));
    }

    /// <summary>
    /// 设置缩放级别，基于当前中心点
    /// </summary>
    public void SetZoom(float zoomLevel)
    {
        SetZoom(zoomLevel, new Vector2(_centerCoordinate, 0));
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
        _centerCoordinate = Mathf.Clamp(newCenter, bounds.leftBound, bounds.rightBound);

        QueueRedraw();
    }

    /// <summary>
    /// 切换到普通模式
    /// </summary>
    public void SwitchToNormalMode()
    {
        P_CurrentMode = RulerMode.Normal;
    }

    /// <summary>
    /// 切换到时间轴毫秒模式
    /// </summary>
    public void SwitchToTimelineMilliseconds()
    {
        P_CurrentMode = RulerMode.TimelineMilliseconds;
    }

    /// <summary>
    /// 切换到时间轴秒模式
    /// </summary>
    public void SwitchToTimelineSeconds()
    {
        P_CurrentMode = RulerMode.TimelineSeconds;
    }

    /// <summary>
    /// 切换到时间轴分钟模式
    /// </summary>
    public void SwitchToTimelineMinutes()
    {
        P_CurrentMode = RulerMode.TimelineMinutes;
    }

    /// <summary>
    /// 切换到时间轴小时模式
    /// </summary>
    public void SwitchToTimelineHours()
    {
        P_CurrentMode = RulerMode.TimelineHours;
    }
    #endregion
}