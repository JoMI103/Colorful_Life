using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;

public class PlayerIdleState : HierarchicalBaseState<MovementState> {

    protected PlayerContext _ctx;

    public PlayerIdleState(MovementState key, PlayerContext playerStateMachine) : base(key) {
        _isRootState = false;
        _ctx = playerStateMachine;
    }

    public override void EnterState() {
        //Debug.LogWarning("Enter Idle State");
        _ctx.AppliedMovementX = 0;
        _ctx.AppliedMovementZ = 0;
    }
    public override void UpdateState() {
        if(CheckSwitchStates()) return;
    }

    public override void ExitState() {
        //Debug.LogWarning("Exit Idle State");
    }

    public override bool CheckSwitchStates() {
        if (_ctx.IsMovementPressed) return SwitchState(_ctx.MovementStates[MovementState.Walk], ref _ctx.CurrentMovementStateRef);
        return false;
    }
}
    

