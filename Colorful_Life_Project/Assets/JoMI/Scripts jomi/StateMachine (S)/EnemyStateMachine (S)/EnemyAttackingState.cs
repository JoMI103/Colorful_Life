using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateMachine;

public class EnemyAttackingState : BaseState<EnemyState>
{
    public EnemyAttackingState(EnemyState key, EnemyStateMachine ctx) : base(key) { _ctx = ctx; }

    protected EnemyStateMachine _ctx;


    public override void EnterState()
    {
        Debug.LogError("Enter Attacking");
    }

    public override void ExitState()
    {
        Debug.LogError("Exit Attacking");
    }

    public override EnemyState GetNextState()
    {
        if (Input.GetKeyDown(KeyCode.V)) { return EnemyState.Idle; }
        if (Input.GetKeyDown(KeyCode.C)) { return EnemyState.Chasing; }

        return EnemyState.Attacking;
    }

    public override void UpdateState()
    {
        
    }
}
