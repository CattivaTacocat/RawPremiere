using Godot;

namespace RawPremiere.Systems.Inputs;

public partial class MouseChangeKeyObserver
{
    #region 属性
    [Notify] public bool IsPressed { get => _isPressed.Get(); private set => _isPressed.Set(value); }
    [Notify] public Key CurrentKey { get => _currentKey.Get(); private set => _currentKey.Set(value); }
    #endregion
    #region 操作
    public void ObserveKey(InputEvent @event)
    {
        if (@event is not InputEventKey key) return;
        IsPressed = key.Pressed;
        if (key.Keycode == CurrentKey && !IsPressed)
        {
            CurrentKey = Key.None;
            return;
        }
        CurrentKey = key.Keycode;
    }
    #endregion
}