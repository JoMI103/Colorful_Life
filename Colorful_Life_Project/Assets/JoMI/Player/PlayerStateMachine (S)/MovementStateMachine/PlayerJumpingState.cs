using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;

public class PlayerJumpingState : HierarchicalBaseState<MovementState> {

    protected PlayerContext _ctx;
    float minJumpTime;

    public PlayerJumpingState(MovementState key, PlayerContext playerStateMachine) : base(key) {
        _isRootState = true;
        _ctx = playerStateMachine;
    }

 

    public override void EnterState() {
        //Debug.Log("Enter Jumping");
        minJumpTime = 0.1f;
        HandleJump();
    }

    public override void UpdateState() {
        if (CheckSwitchStates()) return;
        minJumpTime -= Time.deltaTime;
        HandleGravity();
    }

    public override void ExitState() {
        if (_ctx.IsJumpPressed) _ctx.RequireNewJumpPress = true;
    }

    public override bool CheckSwitchStates() {
        if(_ctx.CharacterController.isGrounded && minJumpTime < 0) return SwitchState(_ctx.MovementStates[MovementState.Grounded], ref _ctx.CurrentMovementStateRef);
        return false;
    }

    void HandleJump() {
       
        _ctx.CurrentMovementY = _ctx.JumpVelocity;
        _ctx.AppliedMovementY = _ctx.JumpGravity;
    }

    void HandleGravity() {
        bool isFalling = _ctx.CurrentMovementY <= 0.0f || !_ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if (isFalling) {
            float previousYVelocity = _ctx.CurrentMovementY;
            _ctx.CurrentMovementY += (_ctx.JumpGravity * fallMultiplier * Time.deltaTime);
            _ctx.AppliedMovementY = Mathf.Max((previousYVelocity + _ctx.CurrentMovementY) * 0.5f, -20.0f);
        }
        else {
            float previousYVelocity = _ctx.CurrentMovementY;
            _ctx.CurrentMovementY += (_ctx.JumpGravity  * Time.deltaTime);
            _ctx.AppliedMovementY = (previousYVelocity + _ctx.CurrentMovementY) * 0.5f;
        }
    }
}
