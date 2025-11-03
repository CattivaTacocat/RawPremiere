using DeadDog.Nodeo.Structures;
using Godot;

namespace RawPremiere.Components.Elements;

public partial class PolarCoordinateComp : Node
{
    #region 属性
    [Notify] public Polar2 Position { get => _position.Get(); set => _position.Set(value); }
    #endregion
    #region 字段
    [Notify,Export]
    private float Radius
    {
        get => _radius.Get();
        set
        {
            _radius.Set(value);
            Position = new Polar2(value, Theta);
        }
    }

    [Notify, Export]
    private float Theta
    {
        get => _theta.Get();
        set
        {
            _theta.Set(value);
            Position = new Polar2(Radius, value);
        }
    }
    #endregion
}