using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static EnemyStateMachine;

public abstract class HierarchicalBaseState<EState> where EState : Enum 
{
    protected HierarchicalBaseState(EState key)
    {
    }

    public abstract void EnterState();

    public abstract void ExitState();

    public abstract EState GetNextState();

    public abstract void UpdateState();
}
