using Godot;
using System;

namespace DeadDog;

public partial class TimelineRuler : HorizontalRuler
{
    #region 属性字段
    private bool _showMilliseconds = true;
    private bool _showTimeLabels = true;
    private Color _currentTimeIndicatorColor = Colors.Red;
    private float _currentTimePosition = 0.0f;
    private float _minLabelSpacing = 1f;
    #endregion

    #region 属性
    [ExportGroup("时间轴设置")]
    [Export]
    public bool P_ShowMilliseconds
    {
        get => _showMilliseconds;
        set { _showMilliseconds = value; UpdateStrategy(); QueueRedraw(); }
    }

    [Export]
    public bool P_ShowTimeLabels
    {
        get => _showTimeLabels;
        set { _showTimeLabels = value; QueueRedraw(); }
    }

    [Export]
    public Color P_CurrentTimeIndicatorColor
    {
        get => _currentTimeIndicatorColor;
        set { _currentTimeIndicatorColor = value; QueueRedraw(); }
    }

    [Export]
    public float P_CurrentTimePosition
    {
        get => _currentTimePosition;
        set { _currentTimePosition = value; QueueRedraw(); }
    }

    [Export]
    public float P_MinLabelSpacing
    {
        get => _minLabelSpacing;
        set { _minLabelSpacing = Mathf.Max(value, 20f); QueueRedraw(); }
    }
    #endregion

    #region 初始化
    public override void _Ready()
    {
        base._Ready();
        P_BackgroundColor = Colors.White;
        P_MarkingColor = Colors.Black;
        P_LabelColor = Colors.Black;
    }

    protected override void OnModeChanged()
    {
        base.OnModeChanged();

        switch (P_CurrentMode)
        {
            case RulerMode.TimelineMilliseconds:
                P_MinCoordinate = -1000.0f;
                P_MaxCoordinate = 1000.0f;
                break;
            case RulerMode.TimelineSeconds:
                P_MinCoordinate = -60.0f;
                P_MaxCoordinate = 60.0f;
                break;
            case RulerMode.TimelineMinutes:
                P_MinCoordinate = -60.0f;
                P_MaxCoordinate = 60.0f;
                break;
            case RulerMode.TimelineHours:
                P_MinCoordinate = -24.0f;
                P_MaxCoordinate = 24.0f;
                break;
        }

        P_BackgroundColor = Colors.White;
        P_MarkingColor = Colors.Black;
        P_LabelColor = Colors.Black;

        QueueRedraw();
    }
    #endregion

    #region 重写策略创建
    protected override IRulerStrategy CreateStrategyForMode(RulerMode mode)
    {
        return mode switch
        {
            RulerMode.TimelineMilliseconds => new MillisecondsTimelineStrategy(),
            RulerMode.TimelineSeconds => new SecondsTimelineStrategy(_showMilliseconds),
            RulerMode.TimelineMinutes => new MinutesTimelineStrategy(),
            RulerMode.TimelineHours => new HoursTimelineStrategy(),
            _ => new NormalRulerStrategy()
        };
    }
    #endregion

    #region 绘制
    public override void _Draw()
    {
        DrawRulerBackground();
        DrawScale();
        DrawBoundaryIndicators();

        if (P_CurrentMode != RulerMode.Normal)
        {
            DrawCurrentTimeIndicator();
        }
    }

    private void DrawScale()
    {
        if (Size.X <= 0 || Size.Y <= 0) return;

        float visibleRange = (P_MaxCoordinate - P_MinCoordinate) / P_CurrentZoom;

        if (visibleRange <= 0.001f)
        {
            DrawCenterLine(Size.X, Size.Y);

            // 只有当显示时间标签时才绘制中心标签
            if (P_LabelFont != null && P_ShowTimeLabels)
            {
                DrawMarkingLabel(Size.X / 2, P_CenterCoordinate, 1.0f);
            }
            return;
        }

        float visibleStart = P_CenterCoordinate - visibleRange / 2;
        float visibleEnd = P_CenterCoordinate + visibleRange / 2;
        float sizeX = Size.X;
        float sizeY = Size.Y;

        float pixelsPerUnit = sizeX / visibleRange;

        float baseInterval = CurrentStrategy.CalculateBaseInterval(pixelsPerUnit);

        baseInterval = Mathf.Clamp(baseInterval,
            Mathf.Max(CurrentStrategy.GetMinimalInterval(), visibleRange / 100f),
            Mathf.Min(CurrentStrategy.GetMaximalInterval(), visibleRange / 2f));

        float majorInterval = baseInterval;
        float mediumInterval = baseInterval / 2f;
        float minorInterval = CurrentStrategy.GetMinorInterval(baseInterval);

        minorInterval = Mathf.Max(minorInterval, CurrentStrategy.GetMinimalInterval());

        float startPos = FindFirstMarkingProtected(visibleStart, minorInterval);

        int tickCount = 0;
        bool hasVisibleTicks = false;
        float lastLabelX = -_minLabelSpacing;

        for (float logicalPos = startPos; logicalPos <= visibleEnd + minorInterval; logicalPos += minorInterval)
        {
            if (tickCount++ > 2000) break;

            if (logicalPos < P_MinCoordinate || logicalPos > P_MaxCoordinate) continue;

            float normalized = (logicalPos - visibleStart) / visibleRange;
            float pixelX = normalized * sizeX;

            if (pixelX < -10f || pixelX > sizeX + 10f) continue;

            var level = DetermineTickLevelProtected(logicalPos, majorInterval, mediumInterval, minorInterval);
            DrawMarking(pixelX, level, logicalPos, sizeY, baseInterval);

            // 只有当显示时间标签时才绘制标签
            if (level == MarkingLevel.Major && P_ShowTimeLabels &&
                P_LabelFont != null &&
                Mathf.Abs(pixelX - lastLabelX) >= _minLabelSpacing)
            {
                DrawMarkingLabel(pixelX, logicalPos, baseInterval);
                lastLabelX = pixelX;
            }

            hasVisibleTicks = true;
        }

        if (!hasVisibleTicks)
        {
            DrawCenterLine(sizeX, sizeY);
        }
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
        switch (P_MarkingDirection)
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

        DrawLine(new Vector2(x, yStart), new Vector2(x, yEnd), P_MarkingColor, lineWidth);

        // 注释掉在DrawMarking中绘制标签的代码，因为标签绘制已经在DrawScale中处理
        // 这样可以通过P_ShowTimeLabels属性控制是否显示标签
    }

    private void DrawMarkingLabel(float x, float logicalValue, float baseInterval)
    {
        string label = CurrentStrategy.FormatLabel(logicalValue, baseInterval);

        var stringSize = P_LabelFont.GetStringSize(label, HorizontalAlignment.Left, -1, P_LabelFontSize);
        float verticalPosition = Size.Y * 0.5f;
        verticalPosition = Mathf.Min(verticalPosition, Size.Y - stringSize.Y - 2);

        Vector2 labelPos = P_LabelAlignment switch
        {
            HorizontalAlignment.Center => new Vector2(x - stringSize.X / 2, verticalPosition),
            HorizontalAlignment.Right => new Vector2(x - stringSize.X - 2, verticalPosition),
            _ => new Vector2(x + 2, verticalPosition)
        };

        if (labelPos.X >= 0 && labelPos.X + stringSize.X <= Size.X &&
            labelPos.Y >= 0 && labelPos.Y + stringSize.Y <= Size.Y)
        {
            DrawString(P_LabelFont, labelPos, label, P_LabelAlignment, -1, P_LabelFontSize, P_LabelColor);
        }
    }

    private void DrawCurrentTimeIndicator()
    {
        if (Mathf.Abs(_currentTimePosition) < 0.001f) return;

        var visibleRange = GetVisibleRange();
        float normalized = (_currentTimePosition - visibleRange.start) / (visibleRange.end - visibleRange.start);
        float pixelX = normalized * Size.X;

        if (pixelX < 0 || pixelX > Size.X) return;

        DrawLine(
            new Vector2(pixelX, 0),
            new Vector2(pixelX, Size.Y),
            _currentTimeIndicatorColor,
            2.0f
        );

        float triangleSize = 8f;
        Vector2[] trianglePoints = {
            new Vector2(pixelX, 0),
            new Vector2(pixelX - triangleSize, triangleSize),
            new Vector2(pixelX + triangleSize, triangleSize)
        };

        DrawColoredPolygon(trianglePoints, _currentTimeIndicatorColor);
    }
    #endregion

    #region 操作
    /// <summary>
    /// 设置当前时间位置
    /// </summary>
    public void SetCurrentTime(float time)
    {
        P_CurrentTimePosition = time;
    }

    /// <summary>
    /// 将视图居中到当前时间位置
    /// </summary>
    public void CenterToCurrentTime()
    {
        P_CenterCoordinate = _currentTimePosition;
    }

    /// <summary>
    /// 设置显示的时间范围（基于当前时间单位）
    /// </summary>
    public void SetTimeRange(float start, float end)
    {
        P_MinCoordinate = start;
        P_MaxCoordinate = end;
    }

    /// <summary>
    /// 获取当前时间位置的像素坐标
    /// </summary>
    public float GetCurrentTimePixelPosition()
    {
        var visibleRange = GetVisibleRange();
        float normalized = (_currentTimePosition - visibleRange.start) / (visibleRange.end - visibleRange.start);
        return normalized * Size.X;
    }

    /// <summary>
    /// 根据像素坐标获取对应的时间值
    /// </summary>
    public float GetTimeAtPixel(float pixelX)
    {
        var visibleRange = GetVisibleRange();
        float normalized = pixelX / Size.X;
        return visibleRange.start + normalized * (visibleRange.end - visibleRange.start);
    }

    /// <summary>
    /// 切换到毫秒模式
    /// </summary>
    public void SwitchToMillisecondsMode()
    {
        P_CurrentMode = RulerMode.TimelineMilliseconds;
    }

    /// <summary>
    /// 切换到秒模式
    /// </summary>
    public void SwitchToSecondsMode()
    {
        P_CurrentMode = RulerMode.TimelineSeconds;
    }

    /// <summary>
    /// 切换到分钟模式
    /// </summary>
    public void SwitchToMinutesMode()
    {
        P_CurrentMode = RulerMode.TimelineMinutes;
    }

    /// <summary>
    /// 切换到小时模式
    /// </summary>
    public void SwitchToHoursMode()
    {
        P_CurrentMode = RulerMode.TimelineHours;
    }

    /// <summary>
    /// 设置时间轴显示选项
    /// </summary>
    public void SetTimelineDisplayOptions(bool showMilliseconds, bool showTimeLabels)
    {
        P_ShowMilliseconds = showMilliseconds;
        P_ShowTimeLabels = showTimeLabels;
    }

    /// <summary>
    /// 设置最小标签间距（像素）
    /// </summary>
    public void SetMinLabelSpacing(float spacing)
    {
        P_MinLabelSpacing = Mathf.Max(spacing, 20f);
    }

    /// <summary>
    /// 获取当前时间单位描述
    /// </summary>
    public string GetCurrentTimeUnitDescription()
    {
        return P_CurrentMode switch
        {
            RulerMode.TimelineMilliseconds => "毫秒模式",
            RulerMode.TimelineSeconds => "秒模式",
            RulerMode.TimelineMinutes => "分钟模式",
            RulerMode.TimelineHours => "小时模式",
            _ => "普通模式"
        };
    }
    #endregion
}