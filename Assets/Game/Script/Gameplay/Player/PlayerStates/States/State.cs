using Mirror;
using Zenject;

public abstract class State
{
    protected PlayerStateMachine StateMachine;

    public State(DiContainer container, PlayerStateMachine stateMachine)
    {
        StateMachine = stateMachine;
    }
    
    public virtual void Initialize() {}
    public abstract void Enter();
    public virtual void UpdateState() {}
    public virtual void OnCommandCall() {}
    public virtual void OnClientRPCCall() {}
    public virtual void OnTargetRPCCall(NetworkConnection target) {}
    public abstract void Exit();
}