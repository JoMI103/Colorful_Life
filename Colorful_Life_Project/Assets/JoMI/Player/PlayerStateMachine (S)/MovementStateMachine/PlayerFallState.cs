using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;

public class PlayerFallState : HierarchicalBaseState<MovementState> { 

    protected PlayerContext _ctx;

    public PlayerFallState(MovementState key, PlayerContext playerStateMachine) : base(key) {
        _isRootState = true;
        _ctx = playerStateMachine;
    }

    public override void EnterState() {
        //Debug.Log("Enter Fall State");
    }

    public override void UpdateState() {
        if (CheckSwitchStates()) return;
        HandleGravity();
    }

    public override void ExitState() {
        //Debug.Log("Exit Fall State"); 
    }

    void HandleGravity() {
        float previousYVelocity = _ctx.CurrentMovementY;
        _ctx.CurrentMovementY += _ctx.CurrentMovementY + _ctx.Gravity * Time.deltaTime;
        _ctx.AppliedMovementY = Mathf.Max((previousYVelocity + _ctx.CurrentMovementY) * 0.5f, -20.0f);
    }

    public override bool CheckSwitchStates() {
        if (_ctx.CharacterController.isGrounded) return SwitchState(_ctx.MovementStates[MovementState.Grounded], ref _ctx.CurrentMovementStateRef);
        return false;
    }
}
