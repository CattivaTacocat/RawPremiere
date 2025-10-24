using Godot;
using System;
using Lombok.NET;

namespace DeadDog.RawPremiere.Player;

[NotifyPropertyChanged]
public partial class Player : Node2D
{
    [Property]
    private bool _isDead;
    [Property(PropertyChangeType = PropertyChangeType.PropertyChanged)]
    private int _score;
}
