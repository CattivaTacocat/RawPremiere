using Godot;
using System;

namespace DeadDog;

public class NormalRulerStrategy : IRulerStrategy
{
    public string FormatLabel(float value, float baseInterval)
    {
        return value.ToString("0.##");
    }

    public float CalculateBaseInterval(float pixelsPerUnit)
    {
        const float targetPixelSpacing = 80f;
        float baseInterval = targetPixelSpacing / pixelsPerUnit;

        float[] niceIntervals = [1, 5, 10, 50, 100, 500, 1000, 5000, 10000];

        float minDifference = float.MaxValue;
        float bestInterval = 100f;

        foreach (float interval in niceIntervals)
        {
            float difference = Mathf.Abs(interval - baseInterval);
            if (difference < minDifference)
            {
                minDifference = difference;
                bestInterval = interval;
            }
        }

        return Mathf.Clamp(bestInterval, 0.1f, 10000f);
    }

    public float GetMinorInterval(float baseInterval)
    {
        return baseInterval / 10f;
    }

    public float GetMinimalInterval()
    {
        return 0.1f;
    }

    public float GetMaximalInterval()
    {
        return 10000f;
    }

    public float[] GetNiceIntervals()
    {
        return [1, 5, 10, 50, 100, 500, 1000, 5000, 10000];
    }
}

public abstract class TimelineRulerStrategy : IRulerStrategy
{
    public abstract string FormatLabel(float value, float baseInterval);

    public float CalculateBaseInterval(float pixelsPerUnit)
    {
        float targetPixelSpacing = GetTargetPixelSpacing();
        float baseInterval = targetPixelSpacing / pixelsPerUnit;
        float[] intervals = GetNiceIntervals();

        float minDifference = float.MaxValue;
        float bestInterval = intervals[0];

        foreach (float interval in intervals)
        {
            if (!(interval <= baseInterval * 2.0f)) continue;
            float difference = Mathf.Abs(interval - baseInterval);
            if (!(difference < minDifference)) continue;
            minDifference = difference;
            bestInterval = interval;
        }

        if (minDifference > baseInterval * 1.0f)
        {
            bestInterval = intervals[0];
        }

        return Mathf.Clamp(bestInterval, GetMinimalInterval(), GetMaximalInterval());
    }

    protected virtual float GetTargetPixelSpacing()
    {
        return 80f;
    }

    public virtual float GetMinorInterval(float baseInterval)
    {
        return baseInterval / 5f;
    }

    public virtual float GetMinimalInterval()
    {
        return 0.1f;
    }

    public abstract float GetMaximalInterval();
    public abstract float[] GetNiceIntervals();
}

public class MillisecondsTimelineStrategy : TimelineRulerStrategy
{
    public override string FormatLabel(float value, float baseInterval)
    {
        return $"{value:0}ms";
    }

    public override float GetMinorInterval(float baseInterval)
    {
        return baseInterval / 10f;
    }

    public override float GetMinimalInterval()
    {
        return 1f;
    }

    public override float GetMaximalInterval()
    {
        return 1000f;
    }

    public override float[] GetNiceIntervals()
    {
        return [1, 5, 10, 50, 100, 500, 1000];
    }
}

public class SecondsTimelineStrategy : TimelineRulerStrategy
{
    private bool _showMilliseconds;

    public SecondsTimelineStrategy(bool showMilliseconds = true)
    {
        _showMilliseconds = showMilliseconds;
    }

    public override string FormatLabel(float value, float baseInterval)
    {
        int seconds = (int)Mathf.Abs(value);
        int milliseconds = (int)((Mathf.Abs(value) - seconds) * 1000);

        string sign = value < 0 ? "-" : "";
        return $"{sign}{seconds:D2}:{milliseconds:D3}";
    }

    protected override float GetTargetPixelSpacing()
    {
        return 100f; 
    }

    public override float GetMaximalInterval()
    {
        return 60f;
    }

    public override float[] GetNiceIntervals()
    {
        return [1, 5, 10, 15, 30, 60];
    }

    public override float GetMinimalInterval()
    {
        return 0.5f;
    }
}
public class MinutesTimelineStrategy : TimelineRulerStrategy
{
    public override string FormatLabel(float value, float baseInterval)
    {
        int totalSeconds = (int)(Mathf.Abs(value) * 60);
        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;
        int milliseconds = (int)((Mathf.Abs(value) * 60 - totalSeconds) * 1000);

        string sign = value < 0 ? "-" : "";
        return $"{sign}{minutes:D2}:{seconds:D2}:{milliseconds:D3}";
    }

    protected override float GetTargetPixelSpacing()
    {
        return 120f;
    }

    public override float GetMaximalInterval()
    {
        return 60f;
    }

    public override float[] GetNiceIntervals()
    {
        return [1, 5, 10, 15, 30, 60];
    }

    public override float GetMinimalInterval()
    {
        return 0.5f;
    }
}

public class HoursTimelineStrategy : TimelineRulerStrategy
{
    public override string FormatLabel(float value, float baseInterval)
    {
        int totalSeconds = (int)(Mathf.Abs(value) * 3600);
        int hours = totalSeconds / 3600;
        int minutes = (totalSeconds % 3600) / 60;
        int seconds = totalSeconds % 60;

        string sign = value < 0 ? "-" : "";
        return $"{sign}{hours:D2}:{minutes:D2}:{seconds:D2}";
    }

    protected override float GetTargetPixelSpacing()
    {
        return 150f;
    }

    public override float GetMaximalInterval()
    {
        return 24f;
    }

    public override float[] GetNiceIntervals()
    {
        return [1, 2, 4, 6, 8, 12, 24];
    }

    public override float GetMinimalInterval()
    {
        return 0.5f;
    }

    public override float GetMinorInterval(float baseInterval)
    {
        return baseInterval / 4f;
    }
}