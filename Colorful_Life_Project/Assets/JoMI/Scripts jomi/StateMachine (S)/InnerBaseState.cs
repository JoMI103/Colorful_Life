using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class InnerBaseState <EState> where EState : Enum
{
    public InnerBaseState(EState key)
    {
        StateKey = key;
    }

    public EState StateKey { get; private set; }

    public abstract void EnterState();
    public abstract void UpdateState();
    public abstract void ExitState();
    public abstract bool CheckSwitchStates();

    protected bool SwitchState(InnerBaseState<EState> newState, ref InnerBaseState<EState> contextCurrentState)
    {
        ExitState(); //Exit current state (this)
        newState.EnterState(); //Enter new state (newState)
        contextCurrentState = newState; 
        return true;
    }
}
