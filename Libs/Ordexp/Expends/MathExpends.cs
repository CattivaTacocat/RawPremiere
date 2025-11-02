using System;
using Godot;

namespace DeadDog.Ordexp;

public static class MathExpends
{
    #region 决策
    public static bool IsEqualsApprox(this float origin, float value, float tolerance = 0.00001f) 
        => Math.Abs(origin - value) < tolerance;
    
    public static bool IsEqualsApprox(this double origin, double value, double tolerance = 0.00001) 
        => Math.Abs(origin - value) < tolerance;

    public static bool CantAssignValue(this float origin, float value) => origin.IsEqualsApprox(value);
    
    public static bool CantAssignValue(this double origin, double value) => origin.IsEqualsApprox(value);
    
    public static bool CantAssignValue(this int origin, int value) => origin.Equals(value);
    
    public static bool CantAssignValue(this long origin, long value) => origin.Equals(value);
    #endregion
    #region 处理
    #region 循环计算
    /// <summary>
    /// 获取循环计算结果。当增量超过循环限制时会从头开始返回计算结果
    /// 即循环为10时，原数字为9，当增量为2时，会返回1，因为(9+2)%10=1
    /// 当增量为1时，会返回0，因为(9+1)%10=0
    /// </summary>
    /// <param name="origin">原数值</param>
    /// <param name="loop">循环数</param>
    /// <param name="add">增量</param>
    /// <returns>计算结果</returns>
    public static int LoopAdd(this int origin, int loop, int add)
    {
        add %= loop;
        return (origin + add + loop) % loop;
    }
    /// <summary>
    /// 获取循环计算结果。当增量超过循环限制时会从头开始返回计算结果
    /// 即循环为10时，原数字为9，当增量为2时，会返回1，因为(9+2)%10=1
    /// 当增量为1时，会返回0，因为(9+1)%10=0
    /// </summary>
    /// <param name="origin">原数值</param>
    /// <param name="loop">循环数</param>
    /// <param name="add">增量</param>
    /// <returns>计算结果</returns>
    public static long LoopAdd(this long origin, long loop, long add)
    {
        add %= loop;
        return (origin + add + loop) % loop;
    }
    /// <summary>
    /// 获取循环计算结果。当增量超过循环限制时会从头开始返回计算结果
    /// 即循环为10时，原数字为9，当增量为2时，会返回1，因为(9+2)%10=1
    /// 当增量为1时，会返回0，因为(9+1)%10=0
    /// </summary>
    /// <param name="origin">原数值</param>
    /// <param name="loop">循环数</param>
    /// <param name="add">增量</param>
    /// <returns>计算结果</returns>
    public static float LoopAdd(this float origin, float loop, float add)
    {
        add %= loop;
        return (origin + add + loop) % loop;
    }
    /// <summary>
    /// 获取循环计算结果。当增量超过循环限制时会从头开始返回计算结果
    /// 即循环为10时，原数字为9，当增量为2时，会返回1，因为(9+2)%10=1
    /// 当增量为1时，会返回0，因为(9+1)%10=0
    /// </summary>
    /// <param name="origin">原数值</param>
    /// <param name="loop">循环数</param>
    /// <param name="add">增量</param>
    /// <returns>计算结果</returns>
    public static double LoopAdd(this double origin, double loop, double add)
    {
        add = add % loop;
        return (origin + add + loop) % loop;
    }
    
    /// <summary>
    /// 获取循环计算结果。当增量超过循环限制时会从头开始返回计算结果
    /// 即循环为1-10时，原数字为9，当增量为2时，会返回1，因为(9+2)%(10-1)=2
    /// 当增量为1时，会返回1，因为(9+1)%(10-1)=1
    /// </summary>
    /// <param name="origin">原数值</param>
    /// <param name="loopStart">循环开始数</param>
    /// <param name="loopEnd">循环结束数</param>
    /// <param name="add">增量</param>
    /// <returns>计算结果</returns>
    public static int LoopAdd(this int origin,int loopStart, int loopEnd, int add)
    {
        var loop = loopEnd - loopStart;
        add %= loop;
        return (origin + add + loop) % loop + loopStart;
    }
    /// <summary>
    /// 获取循环计算结果。当增量超过循环限制时会从头开始返回计算结果
    /// 即循环为1-10时，原数字为9，当增量为2时，会返回1，因为(9+2)%(10-1)=2
    /// 当增量为1时，会返回1，因为(9+1)%(10-1)=1
    /// </summary>
    /// <param name="origin">原数值</param>
    /// <param name="loopStart">循环开始数</param>
    /// <param name="loopEnd">循环结束数</param>
    /// <param name="add">增量</param>
    /// <returns>计算结果</returns>
    public static long LoopAdd(this long origin, long loopStart, long loopEnd, long add)
    {
        var loop = loopEnd - loopStart;
        add = add % loop;
        return (origin + add + loop) % loop + loopStart;
    }
    /// <summary>
    /// 获取循环计算结果。当增量超过循环限制时会从头开始返回计算结果
    /// 即循环为1-10时，原数字为9，当增量为2时，会返回1，因为(9+2)%(10-1)=2
    /// 当增量为1时，会返回1，因为(9+1)%(10-1)=1
    /// </summary>
    /// <param name="origin">原数值</param>
    /// <param name="loopStart">循环开始数</param>
    /// <param name="loopEnd">循环结束数</param>
    /// <param name="add">增量</param>
    /// <returns>计算结果</returns>
    public static float LoopAdd(this float origin, float loopStart, float loopEnd, float add)
    {
        var loop = loopEnd - loopStart;
        add %= loop;
        return (origin + add + loop) % loop + loopStart;
    }
    /// <summary>
    /// 获取循环计算结果。当增量超过循环限制时会从头开始返回计算结果
    /// 即循环为1-10时，原数字为9，当增量为2时，会返回1，因为(9+2)%(10-1)=2
    /// 当增量为1时，会返回1，因为(9+1)%(10-1)=1
    /// </summary>
    /// <param name="origin">原数值</param>
    /// <param name="loopStart">循环开始数</param>
    /// <param name="loopEnd">循环结束数</param>
    /// <param name="add">增量</param>
    /// <returns>计算结果</returns>
    public static double LoopAdd(this double origin, double loopStart, double loopEnd, double add)
    {
        var loop = loopEnd - loopStart;
        add %= loop;
        return (origin + add + loop) % loop + loopStart;
    }

    /// <summary>
    /// 环计算
    /// 把输入的值假想是放入一串数字环中，当原值超过最大值，超出的部分会从最小值开始重新加入超出值，反之从最大值开始计算
    /// </summary>
    /// <param name="origin">原数据</param>
    /// <param name="ringStart">环开始</param>
    /// <param name="ringEnd">环结束</param>
    /// <returns>环内值</returns>
    public static int Ring(this int origin, int ringStart, int ringEnd)
    {
        if (ringEnd < ringStart) (ringStart, ringEnd) = (ringEnd, ringStart);
        var ring = ringEnd - ringStart;
        var offset = (origin - ringStart + ring) % ring;
        return offset > 0 ? ringStart + offset : ringEnd + offset;
    }

    /// <summary>
    /// 环计算
    /// 把输入的值假想是放入一串数字环中，当原值超过最大值，超出的部分会从最小值开始重新加入超出值，反之从最大值开始计算
    /// </summary>
    /// <param name="origin">原数据</param>
    /// <param name="ringStart">环开始</param>
    /// <param name="ringEnd">环结束</param>
    /// <returns>环内值</returns>
    public static float Ring(this float origin, float ringStart, float ringEnd)
    {
        if (ringEnd < ringStart) (ringStart, ringEnd) = (ringEnd, ringStart);
        var ring = ringEnd - ringStart;
        var offset = (origin - ringStart + ring) % ring;
        return offset > 0 ? ringStart + offset : ringEnd + offset;
    }
    #endregion
    #region 数值转换
    public static float RadToDeg(this float rad) => rad * 180 / MathF.PI;
    
    public static float DegToRad(this float deg) => deg * MathF.PI / 180;

    public static string MsToTime(this int ms)
    {
        var timeSpan = TimeSpan.FromMilliseconds(ms);
        return $"{timeSpan.Hours:D2}:{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
    }

    public static int Random(int min, int max)
    {
        var rnd = new Random();
        return rnd.Next(min, max);
    }
    
    public static long Random(long min, long max)
    {
        var rnd = new Random();
        return rnd.NextInt64(min, max);
    }

    public static float Random(float min, float max)
    {
        var rnd = new Random();
        return rnd.NextSingle() * (max - min) + min;
    }
    
    public static double Random(double min, double max)
    {
        var rnd = new Random();
        return rnd.NextDouble() * (max - min) + min;
    }

    public static float Clamp01(this float value) => Math.Clamp(value, 0, 1);

    public static Vector2 RadToVector2(float rad, float length)
    {
        var util = Vector2.FromAngle(rad);
        return util * length;
    }
    #endregion
    #region 其他计算
    /// <summary>
    /// 考虑范围和步长的计算
    /// </summary>
    /// <param name="value">原数值</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="step">步长</param>
    /// <returns>返回结果</returns>
    public static int CalcWithRangeStep(this int value, int min, int max, int step)
    {
        if (min > max) (min, max) = (max, min);
        
        if (step == 0 || value < min || value > max) return Math.Clamp(value, min, max);
        
        if (min * max > 0) return value / step * step;
        var @base = value - min;
        return @base / step * step + min;
    }
    
    /// <summary>
    /// 考虑范围和步长的计算
    /// </summary>
    /// <param name="value">原数值</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="step">步长</param>
    /// <returns>返回结果</returns>
    public static long CalcWithRangeStep(this long value, long min, long max, long step)
    {
        if (min > max) (min, max) = (max, min);
        
        if (step == 0 || value < min || value > max) return Math.Clamp(value, min, max);
        
        if (min * max > 0) return value / step * step;
        var @base = value - min;
        return @base / step * step + min;
    }
    
    /// <summary>
    /// 考虑范围和步长的计算
    /// </summary>
    /// <param name="value">原数值</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="step">步长</param>
    /// <returns>返回结果</returns>
    public static float CalcWithRangeStep(this float value, float min, float max, float step)
    {
        if (min > max) (min, max) = (max, min);
        
        if (step == 0 || value < min || value > max) return Math.Clamp(value, min, max);
        
        if (min * max > 0) return MathF.Truncate(value / step) * step;
        var @base = value - min;
        return MathF.Truncate(@base / step) * step + min;
    }
    
    /// <summary>
    /// 考虑范围和步长的计算
    /// </summary>
    /// <param name="value">原数值</param>
    /// <param name="min">最小值</param>
    /// <param name="max">最大值</param>
    /// <param name="step">步长</param>
    /// <returns>返回结果</returns>
    public static double CalcWithRangeStep(this double value, double min, double max, double step)
    {
        if (min > max) (min, max) = (max, min);
        
        if (step == 0 || value < min || value > max) return Math.Clamp(value, min, max);
        
        if (min * max > 0) return Math.Truncate(value / step) * step;
        var @base = value - min;
        return Math.Truncate(@base / step) * step + min;
    }
    #endregion
    #endregion
}