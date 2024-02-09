using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandStateMachine;
using static PlayerContext;

public class HandsGroupGrabState : InnerBaseState<HandsGroupState>
{
    protected PlayerContext _ctx;

    public HandsGroupGrabState(HandsGroupState key, PlayerContext ctx) : base(key) => _ctx = ctx;
    

   

    public override void EnterState()
    {
        _ctx.GrabbedObject = _ctx.CurrentIGrabbable;
        _ctx.LeftHand.SwitchState(HandState.Follow);
        _ctx.RightHand.SwitchState(HandState.Follow);
     
        UpdateGrabTransforms();

    }

    public override void ExitState()
    {

    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        UpdateGrabTransforms();

    }

    void UpdateGrabTransforms()
    {
        float offset = _ctx.GrabbedObject.Offset;
        (Quaternion, Quaternion) data = _ctx.GrabbedObject.HandsRotations;
       
        _ctx.LeftHand.FollowTransform.rotation = data.Item1;
        _ctx.RightHand.FollowTransform.rotation = data.Item2;
        _ctx.LeftHand.FollowTransform.position = _ctx.transform.rotation * new Vector3(- 0.5f, -offset, -offset)
            + _ctx.GrabbedObject.Position;
        _ctx.RightHand.FollowTransform.position = _ctx.transform.rotation * new Vector3(+ 0.5f, -offset, -offset)
            + _ctx.GrabbedObject.Position;
        
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.InteractPressed && !_ctx.RequireNewInteractPress) {
            _ctx.RequireNewInteractPress = true;
            return SwitchState(_ctx.HandsGroupStates[HandsGroupState.Idle], ref _ctx.CurrentHandsGroupStateRef); 
        }

        if(_ctx.LeftHand.CurrentTargetDistance < 0.1f && _ctx.RightHand.CurrentTargetDistance < 0.1f) 
            return SwitchState(_ctx.HandsGroupStates[HandsGroupState.Carry], ref _ctx.CurrentHandsGroupStateRef);
        return false;
    }

}
