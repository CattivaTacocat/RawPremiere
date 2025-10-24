using System;
using Godot;

namespace DeadDog.Nodeo.Components.Cutto;

public partial class GlitchCutto : SimpleCutto
{
    #region 属性
    public override float P_Duration { get; set; } = 1f;
    #endregion
    #region 事件
    public override event Action OnTransInFinished;
    public override event Action OnTransOutFinished;
    #endregion
    #region 节点
    [Export] public AnimationPlayer N_Player { get; private set; }
    #endregion
    #region 动画
    public override void TransIn()
    {
        N_Player.SpeedScale = P_Duration;
        N_Player.Play("trans-in");

        N_Player.AnimationFinished += ReturnCallback;
        return;

        void ReturnCallback(StringName name)
        {
            if ("trans-in".Equals(name))
                OnTransInFinished?.Invoke();
            N_Player.AnimationFinished -= ReturnCallback;
        }
    }

    public override void TransOut()
    {
        N_Player.SpeedScale = P_Duration;
        N_Player.Play("trans-out");

        N_Player.AnimationFinished += ReturnCallback;
        return;

        void ReturnCallback(StringName name)
        {
            if ("trans-out".Equals(name))
                OnTransOutFinished?.Invoke();
            N_Player.AnimationFinished -= ReturnCallback;
        }
    }
    #endregion
}
