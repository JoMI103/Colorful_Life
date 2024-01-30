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
        _ctx.CalculateForFollowTarget();
        _ctx.HandleMovement(1);
        _ctx.FixPositionWithPlayerMovement();
        HandleRotation();
    }

    public override void ExitState()
    {
        Debug.LogWarning("Exit Hand Follow State");
    }

    private void HandleRotation()
    {
        Quaternion farRotation = Quaternion.LookRotation(Vector3.up, _ctx.PlayerBodyInfluence.normalized);
        Quaternion q = Quaternion.Lerp(_ctx.FollowTransform.rotation, farRotation, Mathf.Exp(_ctx.CurrentTargetDistance - _ctx.CurrentTargetDistance / 1.5f) - 1);
        _ctx.transform.rotation = Quaternion.RotateTowards(_ctx.transform.rotation, q, 360 * _ctx.HandBaseStats.RotationSpeed * Time.deltaTime);
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
