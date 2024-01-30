using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;
using static HandStateMachine;

public class HandsGroupSpellCastState : InnerBaseState<HandsGroupState>
{
    protected PlayerContext _ctx;

    public HandsGroupSpellCastState(HandsGroupState key, PlayerContext ctx) : base(key) => _ctx = ctx;


    public override void EnterState()
    {
        _ctx.LeftHand.SwitchState(HandState.Animate);
        _ctx.RightHand.SwitchState(HandState.Animate);
        _ctx.LeftAnimator.enabled = false;
        _ctx.RightAnimator.enabled = false;
        _ctx.LeftRightAnimator.enabled = true;
        _ctx.LeftRightAnimator.Play("RageAttack");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        _ctx.LeftHand.SwitchState(HandState.Free);
        _ctx.RightHand.SwitchState(HandState.Free);
        _ctx.LeftRightAnimator.Play("Nothing");
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.LeftRightAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            return SwitchState(_ctx.HandsGroupStates[HandsGroupState.Idle], ref _ctx.CurrentHandsGroupStateRef);

        return false;
    }
}
