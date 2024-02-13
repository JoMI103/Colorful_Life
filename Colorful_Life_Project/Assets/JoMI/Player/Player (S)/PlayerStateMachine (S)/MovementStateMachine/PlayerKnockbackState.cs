using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;

public class PlayerKnockbackState : HierarchicalBaseState<MovementState>
{

    protected PlayerContext _ctx;
    private float effectTime;

    public PlayerKnockbackState(MovementState key, PlayerContext playerStateMachine) : base(key)
    {
        _isRootState = true;
        _ctx = playerStateMachine;
        SetSubState(_ctx.MovementStates[MovementState.Idle]);
    }

    public override void EnterState()
    {
        Debug.Log("Enter KnockBack State");
        HandleGravity();
        effectTime = _ctx.PlayerBaseStats.HittedKnockbackDuration;
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        _ctx.AppliedMovementX = _ctx.HitDirection.x * 8;
        _ctx.AppliedMovementZ = _ctx.HitDirection.z * 8;
        HandleGravity();
        effectTime -= Time.deltaTime;
    }

    public override void ExitState()
    {
        Debug.Log("Exit KnockBack State");
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
        if (effectTime > 0) return false;
        
        if (!_ctx.CharacterController.isGrounded) return SwitchState(_ctx.MovementStates[MovementState.Fall], ref _ctx.CurrentMovementStateRef);
        return SwitchState(_ctx.MovementStates[MovementState.Grounded], ref _ctx.CurrentMovementStateRef);
    }
}
