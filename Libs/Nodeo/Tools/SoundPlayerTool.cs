using Godot;
using DeadDog.Nodeo.Structures;

namespace DeadDog.Nodeo.Tools;

/// <summary>
/// 声音播放器工具（来自Nodeo库）
/// </summary>
public static class SoundPlayerTool
{
    #region 辅助字段
    private static NodePool<AudioStreamPlayer> _pool;
    private static bool _disposed;
    private static string _streamPath = string.Empty;
    private static AudioStream _stream;
    private static AudioStreamPlayer _originPlayer;
    #endregion
    #region 创建
    private static void Init()
    {
        _originPlayer ??= new AudioStreamPlayer();
        _pool ??= NodePool<AudioStreamPlayer>.CreateFromDup(_originPlayer,
            player =>
            {
                player.Stop();
                player.Stream = _stream;
            }, null, 5);
        _disposed = true;
    }
    #endregion
    #region 操作
    /// <summary>
    /// 播放
    /// </summary>
    /// <param name="source">播放源节点，生成的音频播放节点和播放源同生命周期</param>
    /// <param name="snd">声音流文件路径</param>
    public static void Play(Node source, string snd)
    {
        if (!_disposed) Init();
        if (ErrForPlayOfSndInvalid(snd)) return;
        if (!_streamPath.Equals(snd))
        {
            _streamPath = snd;
            _stream = GD.Load<AudioStream>(snd);
        }

        var player = _pool.Take();

        void ReturnCallback()
        {
            if (player is null) return;
            player.Finished -= ReturnCallback;
            _pool.Return(player);
        }

        source.AddChild(player);
        player.Play();

        player.Finished += ReturnCallback;
    }

    /// <summary>
    /// 恢复到初始状态
    /// 可以清除所有该工具所带来的孤儿节点
    /// </summary>
    public static void Recover()
    {
        if (!_disposed) return;
        _pool.ClearAll();
        _originPlayer.QueueFree();
        _originPlayer = null;
        _pool = null;
        _disposed = false;
    }
    #endregion
    #region 异常处理
    private static bool ErrForPlayOfSndInvalid(string snd)
    {
        if (snd is null || string.IsNullOrEmpty(snd))
        {
            GD.PushError($"{nameof(Play)}:参数不应该为空");
            return true;
        }
        if (!FileAccess.FileExists(snd))
        {
            GD.PushError($"{nameof(Play)}:音频流文件\"{snd}\"不存在");
            return true;
        }
        return false;
    }
    #endregion
}