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
        if (_ctx.PlayerDistance > _ctx.ChasingDistance)
            return EnemyState.Idle;

        if (_ctx.PlayerDistance < _ctx.AttackingDistance && _ctx.CanSeePlayer())
            return EnemyState.Attacking;

        return EnemyState.Chasing;
    }

    public override void UpdateState()
    {

        
        Vector3 direction =  _ctx.Player.position - _ctx.transform.position;
        direction.y = 0; direction = direction.normalized;
        _ctx.Rb.velocity = new(direction.x, _ctx.Rb.velocity.y, direction.z);
    }


}
