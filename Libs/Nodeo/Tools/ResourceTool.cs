using System.Linq;
using Godot;
using Godot.Collections;
using static Godot.ResourceLoader;

namespace DeadDog.Nodeo.Tools;

public class ResourceTool
{
    #region 操作
    public Resource GetThreadResource<T>(
        string path, out float progress,
        bool useSubThread = false,
        CacheMode cacheMode = CacheMode.Reuse) where T : Resource
    {
        var type = typeof(T).ToString();
        LoadThreadedRequest(path, type, useSubThread, cacheMode);
        Array p = [];
        var status = LoadThreadedGetStatus(path, p);
        progress = (float)p[0];
        return status == ResourceLoader.ThreadLoadStatus.Loaded
            ? LoadThreadedGet(path) as T
            : null;
    }
    #endregion
}