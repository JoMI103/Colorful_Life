using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandsGrabbingState : HandsBaseState
{
    public HandsGrabbingState(HandsStateMachine currentctx, HandsStateFactory factory)
        : base(currentctx, factory) { }

    public override void EnterState(){
        _ctx.LeftRightHands.Item1.StartGrabbing();
        _ctx.LeftRightHands.Item2.StartGrabbing();
        UpdateHandsTarget(_ctx.GrabbleLeftRightTransform);
        Debug.LogError("Enter Grabbing");
        _ctx.Grabbed = false;
    }

    public override void UpdateState(){
        if(CheckSwitchStates()) return;

        //check if hands are in pos to grabbed = true;
        if (_ctx.Grabbed) _ctx.GrabbedObject.transform.position = GetHandsCenterPos();


        if (_ctx.LeftRightHands.Item1._onTarget && _ctx.LeftRightHands.Item2._onTarget && !_ctx.Grabbed) {
            Debug.LogError("GGGGGGGGGGGG");
            _ctx.BaseLeftRightTransform.Item1.localRotation = _ctx.GrabbleLeftRightTransform.Item1.transform.rotation;
            _ctx.BaseLeftRightTransform.Item2.localRotation = _ctx.GrabbleLeftRightTransform.Item2.transform.rotation;
            UpdateHandsTarget(_ctx.BaseLeftRightTransform);
            _ctx.Grabbed = true;
        }


        UpdateHandsPosRot();
      
    }

    private Vector3 GetHandsCenterPos()
    {
        Vector3 p1 = _ctx.LeftRightHands.Item1.transform.position;
        Vector3 p2 = _ctx.LeftRightHands.Item2.transform.position;
        return (p1 - p2) / 2 + p2;
    }


    public override void ExitState()
    {
        Debug.LogError("Exit Grabbing"); 
        if (_ctx.GrabbedObject) { 
            (_ctx.GrabbedObject as IGrabbable).UnGrab(); 
            _ctx.GrabbedObject = null; 
        }
    }
    public override bool CheckSwitchStates(){
        if (!_ctx.GrabbedObject) { SwitchState(_factory.Body()); return true; }
        return false;
    }

    public override void GrabAction() {
        SwitchState(_factory.Body());
    }

    public override void AttackAction(Transform TargetAttack) { Debug.Log("Can't attack because is grabbing."); }
}
