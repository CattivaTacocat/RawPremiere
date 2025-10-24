using System.Collections.Generic;
using DeadDog.Nodeo.Tools;
using Godot;

namespace DeadDog.Nodeo.Components.Cutto;

public static class CuttoFactory
{
    #region 辅助字段
    private static Dictionary<CuttoStyleEnum, string> _cuttoScenePathDic = new();
    #endregion
    #region 创建
    private static void InitDic()
    {
        if (_cuttoScenePathDic is not null && _cuttoScenePathDic.Count > 0) return;
        _cuttoScenePathDic = new()
        {
            { CuttoStyleEnum.Unknown ,"res://Libs/Nodeo/Components/Cutto/SimpleCutto/simple_cutto.tscn"},
            { CuttoStyleEnum.Block ,"res://Libs/Nodeo/Components/Cutto/BlockCutto/block_cutto.tscn"},
            { CuttoStyleEnum.Line ,"res://Libs/Nodeo/Components/Cutto/LineCutto/line_cutto.tscn"},
            { CuttoStyleEnum.Glitch ,"res://Libs/Nodeo/Components/Cutto/GlitchCutto/glitch_cutto.tscn"},
            { CuttoStyleEnum.Count ,"res://Libs/Nodeo/Components/Cutto/SimpleCutto/simple_cutto.tscn"},
        };
    }
    #endregion
    #region 操作
    public static SimpleCutto CreateCutto(CuttoStyleEnum style)
    {
        InitDic();
        return PackageSceneTool.GetInstance<SimpleCutto>(_cuttoScenePathDic.TryGetValue(style, out string path) ?
            path : _cuttoScenePathDic[CuttoStyleEnum.Unknown]);
    }
    #endregion
}