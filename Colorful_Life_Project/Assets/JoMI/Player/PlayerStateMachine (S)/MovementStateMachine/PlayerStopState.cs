using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;

public class PlayerStopState : HierarchicalBaseState<MovementState>
{

    protected PlayerContext _ctx;


    public PlayerStopState(MovementState key, PlayerContext playerStateMachine) : base(key)
    {
        _isRootState = true;
        _ctx = playerStateMachine;
        SetSubState(_ctx.MovementStates[MovementState.Idle]);
    }

    public override void EnterState()
    {
        Debug.Log("Enter Stop State");
        HandleGravity();
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        HandleGravity();
    }

    public override void ExitState()
    {
        Debug.Log("Exit Stop State");
        _ctx.AppliedMovementX = 0;
        _ctx.AppliedMovementZ = 0;
        //Debug.Log("Exit Grounded State");
    }

    void HandleGravity()
    {

        _ctx.CurrentMovementY = _ctx.Gravity;
        _ctx.AppliedMovementY = _ctx.Gravity;
    }

    public override void UpdateStates()
    {
        UpdateState();
        //_currentSubState?.UpdateStates();  not updating substates so he cant move;
    }

    public override bool CheckSwitchStates()
    {
        return false;
    }
}
