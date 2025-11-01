using Godot;

namespace RawPremiere.Components.Elements;

public partial class CameraComp : Node
{
    #region 属性
    [Notify,Export] public bool HasEdgeCollision { get => _hasEdgeCollision.Get(); set => _hasEdgeCollision.Set(value); }
    [Notify,Export] public float ShakeAmplitude { get => _shakeAmplitude.Get(); set => _shakeAmplitude.Set(value); }
    [Notify,Export] public float ShakeFrequency { get => _shakeFrequency.Get(); set => _shakeFrequency.Set(value); }
    #endregion
}