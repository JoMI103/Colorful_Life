using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static HandStateMachine;
using static PlayerContext;

public class HandBurstState : InnerBaseState<HandState>
{
    protected HandStateMachine _ctx;
    private bool _retreating;

    public HandBurstState(HandState key, HandStateMachine ctx) : base(key) => _ctx = ctx;


    public override void EnterState()
    {
        _retreating = false;
        Debug.LogWarning("Enter Hand Burst State");
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        if (_retreating == false && HandleMovementBurst()) _retreating = true;
        if (_retreating) { _ctx.HandleHandToBaseMovement(); }
    }

    public override void ExitState()
    {
        Debug.LogWarning("Exit Hand Burst State");
    }

    bool HandleMovementBurst()
    {
        float currentDistance = Vector3.Distance(_ctx.TargetFollowPos, _ctx.transform.position);
     
        Vector3 directionToTarget = (_ctx.TargetFollowPos - _ctx.transform.position).normalized;


        _ctx.CurrentVelocity = directionToTarget /*- someStartOffsetDirection)*/ * _ctx.HandBaseStats.MoveSpeed
            * getVelocityFactorFunc2(currentDistance);//+ v3 * f3;

        //someStartOffsetDirection *= 0.8f;

        Vector3 nextPos = _ctx.transform.position + _ctx.CurrentVelocity * Time.deltaTime;

        if (Vector3.Distance(_ctx.transform.position, nextPos) > currentDistance || currentDistance < 0.1f)
        {
            //if(hit)
            return true;

        }

  
        //if(hit) return true;
        //if (checkHits(nextPos)) { _targetTransform.position = transform.position; return; }



        _ctx.transform.position = nextPos;
        return false;
    }




    public override bool CheckSwitchStates()
    {
        if (Vector3.Distance(_ctx.TargetBasePos, _ctx.transform.position) < 0.2f && _retreating) SwitchState(_ctx.HandStates[HandState.Free], ref _ctx.CurrentHandStateRef);
        //SwitchState(_ctx.HandStates[HandsGroupState.Idle], ref _ctx.CurrentHandStateRef);
        return false;
    }

    float getVelocityFactorFunc2(float x)
    {
        float aux1 = _ctx.HandBaseStats.Func2V2 + x * _ctx.HandBaseStats.Func2V1;
        if (aux1 == 0) return 0;
        float y = 1 + 1 / aux1;
        return y;
    }

   
}
