using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsBodyState : HandsBaseState
{
    public HandsBodyState(HandsStateMachine currentctx, HandsStateFactory factory) 
        : base(currentctx, factory) { }

    public override void EnterState() {
        _ctx.LeftRightHands.Item1.currentHandState = handState.Body;
        _ctx.LeftRightHands.Item2.currentHandState = handState.Body;
        Debug.LogError("Enter Body");
        _ctx.BaseLeftRightTransform.Item1.localRotation = _ctx.BaseDefaultRotations.Item1;
        _ctx.BaseLeftRightTransform.Item2.localRotation = _ctx.BaseDefaultRotations.Item2;
        UpdateHandsTarget(_ctx.BaseLeftRightTransform);

    }
    public override void UpdateState() {
        if(CheckSwitchStates()) return;
        UpdateHandsPosRot();
    }
    public override void ExitState() { Debug.LogError("Exit Body"); }
    public override bool CheckSwitchStates() {
        if (_ctx.IsPressingAttackMode) { SwitchState(_factory.Attacking()); return true; }
        return false; 
    }

    public override void GrabAction() {
        if (!_ctx.CurrentIGrabbable) return;
        _ctx.GrabbedObject = _ctx.CurrentIGrabbable;
        _ctx.GrabbleLeftRightTransform = (_ctx.CurrentIGrabbable as IGrabbable).Grab();
        SwitchState(_factory.Grabbing()); 
    
    }
    public override void AttackAction(Transform TargetAttack) { Debug.Log("Can't Attack because is not in Attack mode."); }

}
