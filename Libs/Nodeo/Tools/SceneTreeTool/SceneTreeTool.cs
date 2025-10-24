using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DeadDog.Nodeo.Components.Cutto;
using Godot;

namespace DeadDog.Nodeo.Tools;

/// <summary>
/// 场景树工具（来自Nodeo库）
/// 注意！使用该工具前先将它挂载到"自动加载"中！脚本或场景文件都行
/// 且使用它时别直接调用，而是通过其单例调用内部的方法。
/// </summary>
public partial class SceneTreeTool : Node
{
    #region 单例
    public static SceneTreeTool Instance { get; private set; }

    private SceneTreeTool() { }

    public void InitInstance() => Instance ??= this;
    #endregion
    #region 属性
    public Node CurrentRoom { get; private set; }

    public Node PreviousRoom => _historyPathStack.Count > 1 ? PackageSceneTool.GetInstance(_historyPathStack.Peek()) : null;
    public Node NextRevokeRoom => _revokedPathStack.Count > 0 ? PackageSceneTool.GetInstance(_revokedPathStack.Peek()) : null;
    #endregion
    #region 辅助字段
    private readonly Stack<string> _historyPathStack = new();
    private readonly Stack<string> _revokedPathStack = new();

    private readonly object _lock = new();
    
    private CuttoController _cuttoController;
    private readonly CuttoDto _defaultCuttoDto = new()
    {
        Duration = 1f,
        ModulateColor = Colors.White,
        OverlayTexturePath = "res://Libs/Nodeo/Assets/Textures/white.tres",
        IsTiled = false,
        CuttoLayerIndex = 1
    };
    #endregion
    #region 创建
    public override void _Ready()
    {
        InitInstance();
        InitClazz();
        InitNodes();
    }
    
    private void InitClazz() => _cuttoController ??= new();

    private void InitNodes()
    {
        CurrentRoom = GetTree().CurrentScene;
        HandleGoto(CurrentRoom.SceneFilePath);
    }
    #endregion
    #region 操作
    /// <summary>
    /// 改变房间
    /// 和ChangeSceneToFile不同的是，会记录当前房间，并加入历史记录栈中
    /// </summary>
    /// <param name="newRoomPath">新场景路径</param>
    public void GotoRoom(string newRoomPath)
    {
        var newRoom = PackageSceneTool.GetInstance(newRoomPath);
        if (ErrForChangeRoomOfRoomExist(newRoom)) return;
        ReplaceNode(CurrentRoom, newRoom);
        HandleGoto(newRoomPath);
        SetCurrentRoom(newRoom);
    }

    /// <summary>
    /// 回到之前浏览的房间
    /// </summary>
    public void BackRoom()
    {
        var oldRoomPath = HandleBack();
        var oldRoom = PackageSceneTool.GetInstance(oldRoomPath);
        if(ErrForChangeRoomOfRoomExist(oldRoom)) return;
        ReplaceNode(CurrentRoom, oldRoom);
        SetCurrentRoom(oldRoom);
    }

    /// <summary>
    /// 撤销回退房间
    /// 即从A房间回到B后，使用该方法可以从B房间回到A房间
    /// </summary>
    public void RevokeRoom()
    {
        var newRoomPath = HandleRevoke();
        var newRoom = PackageSceneTool.GetInstance(newRoomPath);
        if (ErrForChangeRoomOfRoomExist(newRoom)) return;
        ReplaceNode(CurrentRoom, newRoom);
        SetCurrentRoom(newRoom);
    }

    /// <summary>
    /// 替换节点，和ReplaceBy作用类似，但是会将旧节点队列释放掉
    /// </summary>
    /// <param name="oldRoom">旧节点</param>
    /// <param name="newRoom">新节点</param>
    public void ReplaceNode(Node oldRoom, Node newRoom)
    {
        if (ErrForChangeRoomOfRoomExist(newRoom)) return;
        GetTree().Root.AddChild(newRoom);
        GetTree().Root.RemoveChild(oldRoom);
        oldRoom.QueueFree();
    }

    /// <summary>
    /// 带有转场的改变房间
    /// </summary>
    /// <param name="newRoomPath"></param>
    /// <param name="style"></param>
    /// <param name="awaitTimeMs"></param>
    /// <param name="cuttoParams"></param>
    public async Task GotoRoomWithCutto(
        string newRoomPath,
        CuttoStyleEnum style = CuttoStyleEnum.Block,
        int awaitTimeMs = 300,
        CuttoDto cuttoParams = null)
    {
        ModifyCuttoController(cuttoParams);
        _cuttoController.P_CuttoAwaitTimeMs = awaitTimeMs;
        _cuttoController.P_CuttoStyle = style;
        await _cuttoController.Cutto(this, () =>
        {
            GotoRoom(newRoomPath);
            return Task.CompletedTask;
        });
    }

    public async Task BackRoomWithCutto(
        CuttoStyleEnum style = CuttoStyleEnum.Block,
        int awaitTimeMs = 300,
        CuttoDto cuttoParams = null
    )
    {
        ModifyCuttoController(cuttoParams);
        _cuttoController.P_CuttoAwaitTimeMs = awaitTimeMs;
        _cuttoController.P_CuttoStyle = style;
        await _cuttoController.Cutto(this, () =>
        {
            BackRoom();
            return Task.CompletedTask;
        });
    }

    public async Task RevokeRoomWithCutto(
        CuttoStyleEnum style = CuttoStyleEnum.Block,
        int awaitTimeMs = 300,
        CuttoDto cuttoParams = null
    )
    {
        ModifyCuttoController(cuttoParams);
        _cuttoController.P_CuttoAwaitTimeMs = awaitTimeMs;
        _cuttoController.P_CuttoStyle = style;
        await _cuttoController.Cutto(this, () =>
        {
            RevokeRoom();
            return Task.CompletedTask;
        });
    }
    #endregion
    #region 处理
    private void HandleGoto(string roomPath)
    {
        if (string.IsNullOrEmpty(roomPath)) return;
        _historyPathStack.Push(roomPath);
        _revokedPathStack.Clear();
    }

    private string HandleBack()
    {
        if (_historyPathStack.Count <= 1) return null;
        var last = _historyPathStack.Pop();
        _revokedPathStack.Push(last);
        return _historyPathStack.Peek();
    }

    private string HandleRevoke()
    {
        if (_revokedPathStack.Count <= 0) return null;
        var last = _revokedPathStack.Pop();
        _historyPathStack.Push(last);
        return _historyPathStack.Peek();
    }
    
    private void ModifyCuttoController(CuttoDto cuttoParams)
    {
        cuttoParams ??= _defaultCuttoDto;
        _cuttoController.P_CuttoDto = cuttoParams;
    }
    
    private void SetCurrentRoom(Node room)
    {
        lock (_lock) CurrentRoom = room;
    }
    #endregion
    #region 决策
    public bool HasInvalid(params Node[] nodes) => nodes.Any(node => !IsInstanceValid(node));
    #endregion
    #region 异常处理
    private bool ErrForChangeRoomOfRoomExist(Node room)
    {
        if (room is not null) return false;
        GD.PushWarning($"{nameof(GotoRoom)}:未找到房间{nameof(room)}");
        return true;
    }
    #endregion
}