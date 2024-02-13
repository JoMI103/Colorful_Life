using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;

public class PlayerWalkState : HierarchicalBaseState<MovementState> {

    protected PlayerContext _ctx;

    public PlayerWalkState(MovementState key, PlayerContext playerStateMachine) : base(key) {
        _isRootState = false;
        _ctx = playerStateMachine;
    }

    public override void EnterState() {
        //Debug.LogWarning("Enter Walk State");
    }
    
    public override void UpdateState() {
        if (CheckSwitchStates()) return;
        _ctx.AppliedMovementX = _ctx.CurrentMovementInput.x * _ctx.PlayerBaseStats.MovementSpeed;
        _ctx.AppliedMovementZ = _ctx.CurrentMovementInput.y * _ctx.PlayerBaseStats.MovementSpeed;
    }

    public override void ExitState() {
        //Debug.LogWarning("Exit Walk State");
    }

    public override bool CheckSwitchStates() {
        if (!_ctx.IsMovementPressed) return SwitchState(_ctx.MovementStates[MovementState.Idle], ref _ctx.CurrentMovementStateRef);
        if (_ctx.IsDashPressed && !_ctx.RequireNewDashPress) return SwitchState(_ctx.MovementStates[MovementState.Dash], ref _ctx.CurrentMovementStateRef);
        return false;
    }
}