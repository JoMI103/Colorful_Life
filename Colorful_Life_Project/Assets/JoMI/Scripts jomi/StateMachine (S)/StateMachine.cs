using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class StateMachine<EState> : MonoBehaviour where EState : Enum
{
    protected Dictionary<EState, BaseState<EState>> States = new Dictionary<EState, BaseState<EState>>();
    protected BaseState<EState> _currentState;
   


    protected virtual void Start() {
        _currentState.EnterState();
    }

    protected virtual void Update() {

        EState _nextStateKey = _currentState.GetNextState();
        if (_nextStateKey.Equals(_currentState.StateKey)) { _currentState.UpdateState(); }
        else TransitionToState(_nextStateKey);
    }

    public void TransitionToState(EState stateKey) {
        _currentState.ExitState();
        _currentState = States[stateKey];
        _currentState.EnterState();
    }
}
