using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace old
{
    public class HandsBodyState : HandsBaseState
    {
        public HandsBodyState(HandsStateMachine currentctx, HandsStateFactory factory)
            : base(currentctx, factory) { }

        public override void EnterState()
        {
            //Debug.LogError("Enter Body");
            _ctx.LeftRightHands.Item1.currentHandState = handState.Body;
            _ctx.LeftRightHands.Item2.currentHandState = handState.Body;
            _ctx.BaseLeftRightTransform.Item1.localRotation = _ctx.BaseDefaultRotations.Item1;
            _ctx.BaseLeftRightTransform.Item2.localRotation = _ctx.BaseDefaultRotations.Item2;
            UpdateHandsTarget(_ctx.BaseLeftRightTransform);

        }
        public override void UpdateState()
        {
            if (CheckSwitchStates()) return;
            UpdateHandsPosRot();
        }
        public override void ExitState()
        {
            //Debug.LogError("Exit Body"); 
        }
        public override bool CheckSwitchStates()
        {
            if (_ctx.IsPressingAttackMode) { SwitchState(_factory.Attacking()); return true; }
            return false;
        }

        public override void GrabAction()
        {
            if (!_ctx.CurrentIGrabbable) return;
            _ctx.GrabbedObject = _ctx.CurrentIGrabbable;
            (Quaternion, Quaternion, Vector2) data = (_ctx.CurrentIGrabbable as IGrabbable).Grab();
            GameObject g = _ctx.CurrentIGrabbable.gameObject;
            _ctx.GrabbleLeftRightTransform.Item1.rotation = data.Item1;
            _ctx.GrabbleLeftRightTransform.Item2.rotation = data.Item2;
            _ctx.GrabbleLeftRightTransform.Item1.position = _ctx.transform.rotation * new Vector3(-0.5f, -data.Item3.x, -data.Item3.y)
                + g.transform.position;
            _ctx.GrabbleLeftRightTransform.Item2.position = _ctx.transform.rotation * new Vector3(+0.5f, -data.Item3.x, -data.Item3.y)
                + g.transform.position;
            _ctx.GrabbedOffset = new Vector3(0, data.Item3.x, data.Item3.y);

            SwitchState(_factory.Grabbing());

        }
        public override void AttackAction(Transform TargetAttack) { Debug.Log("Can't Attack because is not in Attack mode."); }

    }
}