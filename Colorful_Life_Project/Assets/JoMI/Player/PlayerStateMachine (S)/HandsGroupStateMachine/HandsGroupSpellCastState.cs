using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;
using static HandStateMachine;
using static PlayerInfo;

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

        switch (_ctx.PlayerInfo.CurrentMagic )
        {
            case Magic.Rage:  _ctx.LeftRightAnimator.Play("RageAttack"); _ctx.PlayerInfo.CurrentMagic = Magic.None; break;
            case Magic.Guilt:  _ctx.PlayerInfo.CurrentMagic = Magic.None; break;
            case Magic.Sadness:  _ctx.PlayerInfo.CurrentMagic = Magic.None; break;
        }
        
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
