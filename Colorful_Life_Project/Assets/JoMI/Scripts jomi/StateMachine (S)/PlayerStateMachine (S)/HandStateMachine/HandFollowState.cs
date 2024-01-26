using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandStateMachine;

public class HandFollowState : InnerBaseState<HandState>
{
    protected HandStateMachine _ctx;

    public HandFollowState(HandState key, HandStateMachine ctx) : base(key) => _ctx = ctx;


    public override void EnterState()
    {
        Debug.LogWarning("Enter Hand Follow State");
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        HandleMovement();
    }

    public override void ExitState()
    {
        Debug.LogWarning("Exit Hand Follow State");
    }

    void HandleMovement()
    {
        float currentDistance = Vector3.Distance(_ctx.TargetFollowPos, _ctx.transform.position);

        Vector3 playerBodyInfluenceForce = (_ctx.PlayerBody.position - _ctx.transform.position).normalized *
                Mathf.Pow(Vector3.Distance(_ctx.transform.position, _ctx.PlayerBody.position), -1);


        Vector3 directionToTarget = (_ctx.TargetFollowPos - _ctx.transform.position).normalized;
        _ctx.CurrentVelocity = (directionToTarget - playerBodyInfluenceForce) *
            _ctx.HandBaseStats.MoveSpeed * getVelocityFactorFunc(currentDistance); //+ v3 * f3;


        Vector3 nextPos = _ctx.transform.position + _ctx.CurrentVelocity * Time.deltaTime;

        if (Vector3.Distance(_ctx.transform.position, nextPos) > currentDistance)
        {
            Debug.LogError("1");
            _ctx.CurrentVelocity = Vector3.zero; playerBodyInfluenceForce = Vector3.zero;
            _ctx.transform.position = _ctx.TargetFollowPos;
            return;
        }


        _ctx.transform.position = nextPos;
    }

    public override bool CheckSwitchStates()
    {
        //SwitchState(_ctx.HandStates[HandsGroupState.Idle], ref _ctx.CurrentHandStateRef);
        return false;
    }

    float getVelocityFactorFunc(float x)
    {
        float aux1 = 1 + x * _ctx.HandBaseStats.Func1;
        if (aux1 == 0) return 0;
        float y = 1 - 1 / aux1;
        return y;
    }
}
