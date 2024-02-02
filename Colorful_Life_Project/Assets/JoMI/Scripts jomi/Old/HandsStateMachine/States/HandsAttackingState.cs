using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace old
{


public class HandsAttackingState : HandsBaseState
{

    float _attackDuration;
    

    public HandsAttackingState(HandsStateMachine currentctx, HandsStateFactory factory) 
        : base(currentctx, factory) { }

    public override void EnterState() {
        _attackDuration = 4;
        _ctx.LeftRightHands.Item1.AttackMode();
        _ctx.LeftRightHands.Item2.AttackMode();

        _ctx.BaseLeftRightTransform.Item1.localRotation = _ctx.BaseDefaultRotations.Item1;
        _ctx.BaseLeftRightTransform.Item2.localRotation = _ctx.BaseDefaultRotations.Item2;
        UpdateHandsTarget(_ctx.BaseLeftRightTransform);

        leftRight = false;
      

        //Debug.LogError("Enter Attacking"); 
    }
    public override void UpdateState() 
    { 
        if (CheckSwitchStates()) return;

        if (_ctx.LeftRightHands.Item1.isOnTargetPos() && !_ctx.LeftRightHands.Item1._isReturning && _ctx.LeftRightHands.Item1._isAttacking) { 
         
            _ctx.LeftRightHands.Item1.Return(_ctx.BaseLeftRightTransform.Item1);  
        }
        if (_ctx.LeftRightHands.Item2.isOnTargetPos() && !_ctx.LeftRightHands.Item2._isReturning && _ctx.LeftRightHands.Item2._isAttacking) {
            
            _ctx.LeftRightHands.Item2.Return(_ctx.BaseLeftRightTransform.Item2);  
        }


        UpdateHandsPosRot();

    }
    public override void ExitState() 
    {
        //Debug.LogError("Exit Attacking"); 
    }
    public override bool CheckSwitchStates() {
        if (!_ctx.IsPressingAttackMode) { SwitchState(_factory.Body()); return true; }   
        return false; 
    }

    public override void GrabAction() { 
        Debug.Log("Can't grab because is attacking.");
    }

    bool leftRight;

    public override void AttackAction(Transform TargetAttack) 
    {
        if (!leftRight && !_ctx.LeftRightHands.Item1._isAttacking && !_ctx.LeftRightHands.Item1._isReturning)
        {
           
            leftRight = true;
            _ctx.LeftRightHands.Item1.StartAttacking(TargetAttack);
        }
        else
        {
            if (!_ctx.LeftRightHands.Item2._isAttacking && !_ctx.LeftRightHands.Item2._isReturning)
            {
                leftRight = false;
                _ctx.LeftRightHands.Item2.StartAttacking(TargetAttack);
            }
        }
    }

   

   

}
}