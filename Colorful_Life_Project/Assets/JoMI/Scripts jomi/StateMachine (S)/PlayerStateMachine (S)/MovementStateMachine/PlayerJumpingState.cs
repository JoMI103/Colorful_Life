using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;

public class PlayerJumpingState : HierarchicalBaseState<MovementState> {

    protected PlayerContext _ctx;

    public PlayerJumpingState(MovementState key, PlayerContext playerStateMachine) : base(key) {
        _isRootState = true;
        _ctx = playerStateMachine;
    }

    IEnumerator IJumpResetRoutine() {
        yield return new WaitForSeconds(.5f); 
        _ctx.JumpCount = 0;
    }

    public override void EnterState() {
        HandleJump();
        //Debug.Log("Enter Jumping State");
    }

    public override void UpdateState() {
        if (CheckSwitchStates()) return;
        HandleGravity();
    }

    public override void ExitState() {
        //Debug.Log("Exit Jumping State");
        if (_ctx.IsJumpPressed) _ctx.RequireNewJumpPress = true;

        _ctx.CurrentJumpResetRoutine = _ctx.StartCoroutine(IJumpResetRoutine());

        if(_ctx.JumpCount == 3)  _ctx.JumpCount = 0;
    }

    public override bool CheckSwitchStates() {
        if(_ctx.CharacterController.isGrounded) return SwitchState(_ctx.MovementStates[MovementState.Grounded], ref _ctx.CurrentMovementStateRef);
        return false;
    }

    void HandleJump() {
        if (_ctx.JumpCount < 3 && _ctx.CurrentJumpResetRoutine != null) _ctx.StopCoroutine(_ctx.CurrentJumpResetRoutine);
        _ctx.IsJumping = true;
        _ctx.JumpCount++;
        _ctx.CurrentMovementY = _ctx.InitialJumpVelocities[_ctx.JumpCount];
        _ctx.AppliedMovementY = _ctx.InitialJumpVelocities[_ctx.JumpCount];
    }

    void HandleGravity() {
        bool isFalling = _ctx.CurrentMovementY <= 0.0f || !_ctx.IsJumpPressed;
        float fallMultiplier = 2.0f;

        if (isFalling) {
            float previousYVelocity = _ctx.CurrentMovementY;
            _ctx.CurrentMovementY += (_ctx.JumpGravities[_ctx.JumpCount] * fallMultiplier * Time.deltaTime);
            _ctx.AppliedMovementY = Mathf.Max((previousYVelocity + _ctx.CurrentMovementY) * 0.5f, -20.0f);
        }
        else {
            float previousYVelocity = _ctx.CurrentMovementY;
            _ctx.CurrentMovementY += (_ctx.JumpGravities[_ctx.JumpCount]  * Time.deltaTime);
            _ctx.AppliedMovementY = (previousYVelocity + _ctx.CurrentMovementY) * 0.5f;
        }
    }
}
