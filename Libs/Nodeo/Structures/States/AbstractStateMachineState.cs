using Godot;

namespace DeadDog.Nodeo.Structures.States;

public abstract class AbstractStateMachineState<T>(T OC) : IStateMachineState where T : class
{
    public abstract void Enter();
    public abstract void Update(double delta);
    public abstract void Update(InputEvent @event);
    public abstract void Exit();
}