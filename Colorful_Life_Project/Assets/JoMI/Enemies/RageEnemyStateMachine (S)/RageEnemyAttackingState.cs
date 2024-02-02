using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyStateMachine;

public class RageEnemyAttackingState : BaseState<EnemyState>
{
    public RageEnemyAttackingState(EnemyState key, EnemyStateMachine ctx) : base(key) { _ctx = ctx; }

    protected EnemyStateMachine _ctx;

    private float _attackTimer;
    private bool _preparedAtack;
    private Vector3 _preparedDirection;

    public override void EnterState()
    {
        _attackTimer = 0;
        //Debug.LogError("Enter Rage Attacking");
    }

    public override void ExitState()
    {
        //Debug.LogError("Exit Rage Attacking");
    }

    public override EnemyState GetNextState()
    {
        if (_ctx.PlayerDistance > _ctx.ChasingDistance)
            return EnemyState.Idle;
        if (_ctx.PlayerDistance > _ctx.AttackingDistance)
            return EnemyState.Chasing;



        return EnemyState.Attacking;
    }

    public override void UpdateState()
    {
        _attackTimer += Time.deltaTime;


        if (!_preparedAtack && _attackTimer > _ctx.PreparingAttackTimer)
        {
            _preparedAtack = true;
            Vector3 direction = _ctx.Player.position - _ctx.transform.position;
            direction.y = 0; direction = direction.normalized;
            _preparedDirection = direction;

        }

        if (_attackTimer > _ctx.AttackTimer)
        {
            _preparedAtack = false;
            _attackTimer = -0.25f;


            _ctx.Rb.AddForce(new Vector3(_preparedDirection.x, 0.5f/*jump force*/, _preparedDirection.z) * _ctx.AttackForce, ForceMode.Impulse);


        }
    }
}
