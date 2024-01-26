using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;

public class PlayerGroundedState : HierarchicalBaseState<MovementState> {

    protected PlayerContext _ctx;

    public PlayerGroundedState(MovementState key, PlayerContext playerStateMachine) : base(key) {
        _isRootState = true;
        _ctx = playerStateMachine;
        SetSubState(_ctx.MovementStates[MovementState.Idle]);
    }

    public override void EnterState() {
        HandleGravity();
        //Debug.Log("Enter Grounded State");
    }

    public override void UpdateState() {
        if (CheckSwitchStates()) return;
    }

    public override void ExitState() {
        //Debug.Log("Exit Grounded State");
    }

    void HandleGravity() {
        _ctx.CurrentMovementY = _ctx.Gravity;
        _ctx.AppliedMovementY = _ctx.Gravity;
    }

    public override bool CheckSwitchStates() {
        if(_ctx.IsJumpPressed && !_ctx.RequireNewJumpPress) return SwitchState(_ctx.MovementStates[MovementState.Jumping], ref _ctx.CurrentMovementStateRef);
        if(!_ctx.CharacterController.isGrounded) return SwitchState(_ctx.MovementStates[MovementState.Fall], ref _ctx.CurrentMovementStateRef);

        return false;
    }
}
