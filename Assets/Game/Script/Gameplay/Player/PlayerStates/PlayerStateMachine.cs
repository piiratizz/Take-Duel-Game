﻿using System.Collections.Generic;
using Mirror;
using Zenject;

public class PlayerStateMachine : NetworkBehaviour
{
    [Inject] private DiContainer _container;
    
    private Dictionary<States, State> _allState;
    private State _currentState;
    public State Current => _currentState;

    private bool _initialized;
    
    public void Initialize(States start)
    {
        _allState = new Dictionary<States, State>
        {
            { States.Fight, new FightState(_container, this) },
            { States.Dead, new DeadState(_container, this) },
            { States.Wait, new WaitState(_container, this) }
        };
        
        SetState(start);
        _initialized = true;
    }
    
    private void Update()
    {
        if(!_initialized) return;
        
        _currentState.UpdateState();
    }

    public void SetState(States newState)
    {
        _currentState?.Exit();
        _currentState = _allState[newState];
        _currentState.Enter();
    }

    [Command]
    public void CommandCall()
    {
        _currentState.OnCommandCall();
    }
    
    [ClientRpc]
    public void ClientRpcCall()
    {
        _currentState.OnClientRPCCall();
    }
    
    [TargetRpc]
    public void TargetRPCCall(NetworkConnection target)
    {
        _currentState.OnTargetRPCCall(target);
    }
}