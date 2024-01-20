using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateMachine;

public class EnemyIdleState : BaseState<EnemyState>
{
    public EnemyIdleState(EnemyState key, EnemyStateMachine ctx) : base(key) { _ctx = ctx; }

    protected EnemyStateMachine _ctx;

    public override void EnterState()
    {
        Debug.LogError("Enter Idle");
    }

    public override void ExitState()
    {
        Debug.LogError("Exit Idle");
    }

    public override EnemyState GetNextState()
    {
        if (_ctx.PlayerDistance < _ctx.SearchDistance)
            if (_ctx.CanSeePlayer()) {
                if (_ctx.PlayerDistance < _ctx.ChasingDistance) return EnemyState.Chasing;
                if (_ctx.PlayerDistance < _ctx.AttackingDistance) return EnemyState.Attacking;
            }

        return EnemyState.Idle;
    }

    public override void UpdateState()
    {

        
    }
}
