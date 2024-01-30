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
    private Quaternion initialRotation;

    public HandBurstState(HandState key, HandStateMachine ctx) : base(key) => _ctx = ctx;


    public override void EnterState() {
        _retreating = false;
        initialRotation = _ctx.transform.rotation;
        Debug.LogWarning("Enter Hand Burst State");
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        
        if (_retreating == false) { _ctx.CalculateForFollowTarget(); FastRotate(); if (HandleMovementBurst()) _retreating = true; }
        if (_retreating) { _ctx.CalculateForBaseTarget(); _ctx.HandleMovement(3); }
    }

    public override void ExitState() {
        Debug.LogWarning("Exit Hand Burst State");
    }

    bool HandleMovementBurst() { 
 
        _ctx.CurrentVelocity = _ctx.HandBaseStats.MoveSpeed * getVelocityFactorFunc2(_ctx.CurrentTargetDistance) *_ctx.CurrentTargetDirection;//+ v3 * f3;

        //someStartOffsetDirection *= 0.8f;

        Vector3 nextPos = _ctx.transform.position + _ctx.CurrentVelocity * Time.deltaTime;
        if (Vector3.Distance(_ctx.transform.position, nextPos) > _ctx.CurrentTargetDistance || _ctx.CurrentTargetDistance < 0.1f) {
            CheckHits(nextPos);
            return true;
        }

        if (CheckHits(nextPos)) return true;
        _ctx.transform.position = nextPos;
        return false;
    }

    private void FastRotate()
    {
        float dist = _ctx.CurrentTargetDistance / Vector3.Distance(_ctx.PlayerBody.position, _ctx.FollowTransform.position);
        Quaternion q = Quaternion.Lerp(_ctx.FollowTransform.rotation, initialRotation, dist);
        _ctx.transform.rotation = Quaternion.RotateTowards(_ctx.transform.rotation, q, 360 * _ctx.HandBaseStats.RotationSpeed * Time.deltaTime);
    }


    private bool CheckHits(Vector3 nextpos)  {
        bool hitted = false;

        RaycastHit[] hits = Physics.SphereCastAll(
            _ctx.transform.position,
            0.2f,
            nextpos - _ctx.transform.position,
            Vector3.Distance(_ctx.transform.position, nextpos));

        foreach (RaycastHit hit in hits) {
            MonoBehaviour[] allScripts = hit.collider.gameObject.GetComponentsInChildren<MonoBehaviour>();
            foreach (MonoBehaviour mono in allScripts)
                if (mono is IHittable)
                {
                    (mono as IHittable).Hit(
                        _ctx.gameObject,
                        (nextpos - _ctx.transform.position).normalized * 10 , //_ctx.CurrentPunchForce
                        hit.point,
                        5);
                    hitted = true;
                }
           
        }
        return hitted;
    }



    public override bool CheckSwitchStates()
    {
        if (Vector3.Distance(_ctx.BaseTransform.position, _ctx.transform.position) < 0.2f && _retreating) 
            return SwitchState(_ctx.HandStates[HandState.Free], ref _ctx.CurrentHandStateRef);

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