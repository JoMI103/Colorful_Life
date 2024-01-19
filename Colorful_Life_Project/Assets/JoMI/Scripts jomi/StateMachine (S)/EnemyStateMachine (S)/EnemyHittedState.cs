using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateMachine;

public class EnemyHittedState : BaseState<EnemyState>
{
    public EnemyHittedState(EnemyState key, EnemyStateMachine ctx) : base(key) { _ctx = ctx; }

    protected EnemyStateMachine _ctx;

    private float _timer;

    public override void EnterState()
    {
        Debug.LogError("Enter Hitted");
        _timer = 0;   
    }

    public override void ExitState()
    {
        Debug.LogError("Exit Hitted");
        _ctx.Rb.velocity = new Vector3(0, _ctx.Rb.velocity.y, 0);
    }

    public override EnemyState GetNextState()
    {
        if (_timer > 0.5f) return EnemyState.Idle;

        return EnemyState.Hitted;
    }

    public override void UpdateState()
    {
        
        _timer += Time.deltaTime;
       
    }
}
