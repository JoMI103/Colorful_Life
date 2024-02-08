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
        _ctx.HandleRotation(1);
        _ctx.FixPositionWithPlayerMovement();
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
