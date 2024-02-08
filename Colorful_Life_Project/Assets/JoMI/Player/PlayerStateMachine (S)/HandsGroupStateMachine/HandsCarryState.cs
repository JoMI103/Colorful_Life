using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static HandStateMachine;
using static PlayerContext;

public class HandsCarryState : InnerBaseState<HandsGroupState>
{
    protected PlayerContext _ctx;

    public HandsCarryState(HandsGroupState key, PlayerContext ctx) : base(key) => _ctx = ctx;

  

    public override void EnterState()
    {
        if (CheckSwitchStates()) return;
        _ctx.GrabbedObject?.Grab();
        _ctx.LeftHand.SwitchState(HandState.Free);
        _ctx.RightHand.SwitchState(HandState.Free);
    }

    public override void ExitState()
    {
        _ctx.GrabbedObject?.UnGrab();
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        float offset = _ctx.GrabbedObject.Offset;
        _ctx.GrabbedObject.updatePosWithHandsPos( GetHandsCenterPos() + _ctx.transform.rotation * new Vector3( 0, offset, offset));
    }

    private Vector3 GetHandsCenterPos()
    {
        Vector3 p1 = _ctx.LeftHand.transform.position;
        Vector3 p2 = _ctx.RightHand.transform.position;
        return (p1 - p2) / 2 + p2;
    }

    public override bool CheckSwitchStates()
    {
        if (Input.GetKeyDown(KeyCode.E) || _ctx.GrabbedObject == null) return SwitchState(_ctx.HandsGroupStates[HandsGroupState.Idle], ref _ctx.CurrentHandsGroupStateRef);
        return false;
    }
}
