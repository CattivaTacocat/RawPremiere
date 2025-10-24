using Godot;
using System;

namespace DeadDog;

public interface IRulerStrategy
{
    string FormatLabel(float value, float baseInterval);
    float CalculateBaseInterval(float pixelsPerUnit);
    float GetMinorInterval(float baseInterval);
    float GetMinimalInterval();
    float GetMaximalInterval();
    float[] GetNiceIntervals();
}

public enum MarkingLevel
{
    Major,
    Medium,
    Minor
}

public enum MarkingDirection
{
    Up,
    Down,
    Both
}

public enum RulerMode
{
    Normal,
    TimelineMilliseconds,
    TimelineSeconds,
    TimelineMinutes,
    TimelineHours
}
