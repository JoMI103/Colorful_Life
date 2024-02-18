using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageEnemyStateMachine : EnemyStateMachine
{
    [SerializeField] private Transform model;

    protected override void setStates()
    {
        States.Add(EnemyState.Idle, new EnemyIdleState(EnemyState.Idle, this));
        States.Add(EnemyState.Chasing, new EnemyChasingState(EnemyState.Chasing, this));
        States.Add(EnemyState.Attacking, new RageEnemyAttackingState(EnemyState.Attacking, this));
        States.Add(EnemyState.Hitted, new EnemyHittedState(EnemyState.Hitted, this));
        _currentState = States[EnemyState.Idle];
    }

    protected override void Update()
    {
        base.Update();
        model.transform.forward = (new Vector3(Player.transform.position.x,transform.position.y, Player.transform.position.z) - transform.position).normalized;
    }
}
