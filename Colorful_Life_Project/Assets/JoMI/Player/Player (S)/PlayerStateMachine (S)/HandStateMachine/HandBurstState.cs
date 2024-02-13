using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static HandStateMachine;
using static PlayerContext;

public class HandBurstState : InnerBaseState<HandState>
{
    protected HandStateMachine _ctx;

    //State Start Variables
    private Quaternion initialRotation;
    private bool _retreating;
    private float _retreatingTimer;
    private float _burstVelocity;
    private int _AttackDmg;
    private float _impactForce;
    private Vector3 _randomOffsetDirection;

    public HandBurstState(HandState key, HandStateMachine ctx) : base(key) => _ctx = ctx;

    public override void EnterState() {
        _ctx.Trail.emitting = true;
        _retreating = false;
        _retreatingTimer = 1.5f;
        initialRotation = _ctx.transform.rotation;
        //Debug.LogWarning("Enter Hand Burst State");
        _randomOffsetDirection = Quaternion.Euler(0, 0, Random.Range(-45f, 45f))  *  _ctx.transform.forward;
        _randomOffsetDirection *= _ctx.PunchPower;
        _burstVelocity = Mathf.Lerp(_ctx.HandBaseStats.MinMaxPunchVelocity.x, _ctx.HandBaseStats.MinMaxPunchVelocity.y, _ctx.PunchPower);
        _AttackDmg = Mathf.CeilToInt(Mathf.Lerp(_ctx.HandBaseStats.MinMaxPunchDamage.x, _ctx.HandBaseStats.MinMaxPunchDamage.y, _ctx.PunchPower));
        _impactForce = Mathf.CeilToInt(Mathf.Lerp(_ctx.HandBaseStats.MinMaxPunchImpactForce.x, _ctx.HandBaseStats.MinMaxPunchImpactForce.y, _ctx.PunchPower));

    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        
        if (_retreating == false) { _ctx.CalculateForFollowTarget(); FastRotate(); if (HandleMovementBurst()) _retreating = true; }
        if (_retreating) { _ctx.CalculateForBaseTarget(); _ctx.HandleMovement(3); _ctx.HandleRotation(3); _retreatingTimer -= Time.deltaTime; }
    }

    public override void ExitState() {
        _ctx.Trail.emitting = false;
        /*Debug.LogWarning("Exit Hand Burst State");*/
    }

    bool HandleMovementBurst() { 
 
        _ctx.CurrentVelocity = _burstVelocity * getVelocityFactorFunc2(_ctx.CurrentTargetDistance) * (_ctx.CurrentTargetDirection + _randomOffsetDirection);//+ v3 * f3;

        _randomOffsetDirection *= 0.8f;

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
        _ctx.transform.forward = _ctx.CurrentTargetDirection;

        return;
        float dist = _ctx.CurrentTargetDistance / Vector3.Distance(_ctx.PlayerBody.position, _ctx.FollowTransform.position);
        Quaternion q = Quaternion.Lerp(_ctx.FollowTransform.rotation, initialRotation, dist);
        _ctx.transform.rotation = Quaternion.RotateTowards(_ctx.transform.rotation, q, 360 * _ctx.HandBaseStats.RotationSpeed * 10 * Time.deltaTime);
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
                if (mono is IHittable && mono.transform != _ctx.PlayerBody.transform)
                {
                   

                    (mono as IHittable).Hit(
                        _ctx.gameObject,
                        (nextpos - _ctx.transform.position).normalized * _impactForce, 
                        hit.point,
                        _AttackDmg
                        );
                    hitted = true;
                }
           
        }
        return hitted;
    }



    public override bool CheckSwitchStates()
    {
        if (_retreating && (Vector3.Distance(_ctx.BaseTransform.position, _ctx.transform.position) < 0.2f || _retreatingTimer < 0) ) 
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