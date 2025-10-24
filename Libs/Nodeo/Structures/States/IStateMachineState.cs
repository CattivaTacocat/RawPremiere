using Godot;

namespace DeadDog.Nodeo.Structures.States;

public interface IStateMachineState
{
    public void Enter();
    public void Update(double delta);
    public void Update(InputEvent @event);
    public void Exit();
}