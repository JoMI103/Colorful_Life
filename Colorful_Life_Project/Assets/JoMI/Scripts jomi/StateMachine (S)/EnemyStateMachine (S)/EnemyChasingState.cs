using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateMachine;

public class EnemyChasingState : BaseState<EnemyState>
{
    public EnemyChasingState(EnemyState key, EnemyStateMachine ctx) : base(key) { _ctx = ctx; }

    protected EnemyStateMachine _ctx;

    public override void EnterState()
    {
        Debug.LogError("Enter Chasing");
    }

    public override void ExitState()
    {
        Debug.LogError("Exit Chasing");
    }

    public override EnemyState GetNextState()
    {
        if (Input.GetKeyDown(KeyCode.V)) { return EnemyState.Idle; }
        if (Input.GetKeyDown(KeyCode.X)) { return EnemyState.Attacking; }

        return EnemyState.Chasing;
    }

    public override void UpdateState()
    {
        //_ctx.Rb.velocity = new Vector3(_ctx.pl);
    }


}
