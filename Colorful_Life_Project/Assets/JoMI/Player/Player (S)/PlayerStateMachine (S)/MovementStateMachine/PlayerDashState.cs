using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;

public class PlayerDashState : HierarchicalBaseState<MovementState>
{

    protected PlayerContext _ctx;

    private float _dashDuration;
    private Vector2 _enterStateDirection;

    public PlayerDashState(MovementState key, PlayerContext playerStateMachine) : base(key)
    {
        _isRootState = false;
        _ctx = playerStateMachine;
    }

    public override void EnterState()
    {
        _dashDuration = _ctx.PlayerBaseStats.DashDuration;
        _enterStateDirection = _ctx.CurrentMovementInput;
        float speed = _ctx.PlayerBaseStats.DashSpeedMultiplier * _ctx.PlayerBaseStats.MovementSpeed;
        _ctx.AppliedMovementX = _enterStateDirection.x * speed;
        _ctx.AppliedMovementZ = _enterStateDirection.y * speed;
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;

        _dashDuration -= Time.deltaTime;

        
    }

    public override void ExitState()
    {
        _ctx.RequireNewDashPress = true;
    }

    public override bool CheckSwitchStates()
    {
        if(_dashDuration > 0) return false;
        if (!_ctx.IsMovementPressed) return SwitchState(_ctx.MovementStates[MovementState.Idle], ref _ctx.CurrentMovementStateRef);
        if (_ctx.IsMovementPressed) return SwitchState(_ctx.MovementStates[MovementState.Walk], ref _ctx.CurrentMovementStateRef);
        return false;
    }
}
