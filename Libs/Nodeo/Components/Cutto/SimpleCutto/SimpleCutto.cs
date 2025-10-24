using System;
using Godot;
using static Godot.TextureRect;

namespace DeadDog.Nodeo.Components.Cutto;

public partial class SimpleCutto : CanvasLayer
{
    #region 属性字段
    private Color _modulateColor = Colors.White;
    private Texture2D _overlayTexture = ResourceLoader.Load<Texture2D>("res://Libs/Nodeo/Assets/Textures/white.tres");
    private bool _isTiled = false;
    private int _cuttoLayerIndex = 1;
    #endregion
    #region 属性
    [Export] public virtual float P_Duration { get; set; } = 0.5f;

    [Export] public virtual bool P_IsTiled
    {
        get => _isTiled;
        set
        {
            if (_isTiled == value)return;
            _isTiled = value;
            UpdateIsTiledView();
        }
    }

    [Export] public virtual Color P_ModulateColor
    {
        get => _modulateColor;
        set
        {
            if (_modulateColor.Equals(value)) return;
            _modulateColor = value;
            UpdateModulateColorView();
        }
    }

    [Export] public virtual Texture2D P_OverlayTexture
    {
        get => _overlayTexture;
        set
        {
            if (_overlayTexture.Equals(value)) return;
            _overlayTexture = value;
            UpdateOverlayTextureView();
        }
    }
    
    [Export] public virtual int P_CuttoLayerIndex
    {
        get => _cuttoLayerIndex;
        set
        {
            if(_cuttoLayerIndex.Equals(value)) return;
            _cuttoLayerIndex = value;
            UpdateCuttoLayerIndexView();
        }
    }
    #endregion
    #region 事件
    public virtual event Action OnTransInFinished;
    public virtual event Action OnTransOutFinished;
    #endregion
    #region 节点
    [Export] public TextureRect N_CuttoContainer { get; private set; }
    #endregion
    #region 数据
    public virtual void LoadFromDto(CuttoDto dto)
    {
        if(dto is null) return;
        P_Duration = dto.Duration;
        P_ModulateColor = dto.ModulateColor;
        SetTexture(dto.OverlayTexturePath);
        P_IsTiled = dto.IsTiled;
        P_CuttoLayerIndex = dto.CuttoLayerIndex;
    }

    public virtual CuttoDto ExportAsDto()
    {
        return new()
        {
            Duration = P_Duration,
            ModulateColor = P_ModulateColor,
            OverlayTexturePath = P_OverlayTexture?.ResourcePath,
            IsTiled = P_IsTiled,
            CuttoLayerIndex = P_CuttoLayerIndex
        };
    }
    #endregion
    #region 创建
    public override void _Ready()
    {
        UpdateView();
    }
    #endregion
    #region 视图
    public void UpdateView()
    {
        UpdateModulateColorView();
        UpdateOverlayTextureView();
        UpdateIsTiledView();
        UpdateCuttoLayerIndexView();
    }

    protected virtual void UpdateModulateColorView()
    {
        if (!IsInstanceValid(N_CuttoContainer)) return;
        N_CuttoContainer.Modulate = P_ModulateColor;
    }

    protected virtual void UpdateOverlayTextureView()
    {
        if (!IsInstanceValid(N_CuttoContainer)) return;
        N_CuttoContainer.Texture = P_OverlayTexture;
    }
    
    protected virtual void UpdateIsTiledView()
    {
        if (!IsInstanceValid(N_CuttoContainer)) return;
        N_CuttoContainer.StretchMode = P_IsTiled ? 
            StretchModeEnum.Tile : StretchModeEnum.Scale;
    }
    
    protected virtual void UpdateCuttoLayerIndexView()
    {
        if (!IsInstanceValid(N_CuttoContainer)) return;
        N_CuttoContainer.ZIndex = P_CuttoLayerIndex;
    }
    #endregion
    #region 动画
    public virtual void TransIn()
    {
        OnTransInFinished?.Invoke();
    }

    public virtual void TransOut()
    {
        OnTransOutFinished?.Invoke();
    }
    #endregion
    #region 处理
    protected virtual void SetTexture(string path)
    {
        if (FileAccess.FileExists(path))
            P_OverlayTexture = GD.Load<Texture2D>(path) ?? P_OverlayTexture;
    }
    #endregion
}
