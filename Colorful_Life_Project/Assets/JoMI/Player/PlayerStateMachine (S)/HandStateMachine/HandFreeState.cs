using old;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static HandStateMachine;
public class HandFreeState : InnerBaseState<HandState>
{
    protected HandStateMachine _ctx;

    public HandFreeState(HandState key, HandStateMachine ctx) : base(key) => _ctx = ctx;


    public override void EnterState()
    {
        //Debug.LogWarning("Enter Hand Free State");
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        _ctx.CalculateForBaseTarget();
        _ctx.HandleMovement(1);
        _ctx.FixPositionWithPlayerMovement();
        HandleRotation();
    }


    private void HandleRotation()
    {
        Quaternion farRotation = Quaternion.LookRotation(Vector3.up, _ctx.PlayerBodyInfluence.normalized);
        Quaternion q = Quaternion.Lerp(_ctx.BaseTransform.rotation, farRotation, Mathf.Exp(_ctx.CurrentTargetDistance - _ctx.CurrentTargetDistance / 1.5f) - 1);
        _ctx.transform.rotation = Quaternion.RotateTowards(_ctx.transform.rotation, q, 360 * _ctx.HandBaseStats.RotationSpeed * Time.deltaTime);
    }


    public override void ExitState()
    {
        //Debug.LogWarning("Exit Hand Free State");
    }

   

    public override bool CheckSwitchStates()
    {
        //SwitchState(_ctx.HandStates[HandsGroupState.Idle], ref _ctx.CurrentHandStateRef);
        return false;
    }

   
}
