using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static HandStateMachine;

public class HandAnimatedState : InnerBaseState<HandState>
{
    protected HandStateMachine _ctx;

    public HandAnimatedState(HandState key, HandStateMachine ctx) : base(key) => _ctx = ctx;


    public override void EnterState()
    {
        //Debug.LogWarning("Enter Hand Animated State");
    }

    public override void UpdateState()
    {
        if(CheckSwitchStates()) return;
        _ctx.transform.position = Vector3.MoveTowards(_ctx.transform.position, _ctx.AnimationTransform.position, Time.deltaTime * _ctx.HandBaseStats.AnimationMovementSpeed);
        _ctx.transform.rotation = Quaternion.RotateTowards(_ctx.transform.rotation, _ctx.AnimationTransform.rotation, Time.deltaTime * 10 * _ctx.HandBaseStats.AnimationRotationSpeed) ;
    }

    public override void ExitState()
    {
        //Debug.LogWarning("Exit Hand Animated State");
    }

    public override bool CheckSwitchStates()
    {
        //SwitchState(_ctx.HandStates[HandsGroupState.Idle], ref _ctx.CurrentHandStateRef);
        return false;
    }
}
