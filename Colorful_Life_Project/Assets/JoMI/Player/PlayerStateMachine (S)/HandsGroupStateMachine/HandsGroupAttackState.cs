using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;
using static HandStateMachine;
using Unity.VisualScripting;
using Unity.Mathematics;

public class HandsGroupAttackState : InnerBaseState<HandsGroupState>
{
    protected PlayerContext _ctx;
    private float _attackModeTimer;
    private float _chargingPunchTimer;
    private bool _leftRightLastUsed;

    public HandsGroupAttackState(HandsGroupState key, PlayerContext ctx) : base(key) => _ctx = ctx;



    public override void EnterState() {
        _attackModeTimer = 1;
        _ctx.LeftRightAnimator.enabled = false;
        _ctx.LeftAnimator.enabled = true;
        _ctx.RightAnimator.enabled = true;
        //Debug.LogWarning("Enter Hands Attack State");
    }

    public override void UpdateState()
    {
        if(CheckSwitchStates()) return;

        _attackModeTimer -= Time.deltaTime;
        if (_ctx.IsAttackPressed) _attackModeTimer = 1f;

        if (_chargingPunchTimer == 0 && _ctx.IsAttackPressed)
        {
            if (!_leftRightLastUsed && _ctx.LeftHand.CurrentHandState.StateKey == HandState.Free) { LeftPunch(); return; }
            if (_leftRightLastUsed && _ctx.RightHand.CurrentHandState.StateKey == HandState.Free) { RightPunch(); return; }

            if (_ctx.LeftHand.CurrentHandState.StateKey == HandState.Free) { LeftPunch(); return;  }
            if (_ctx.RightHand.CurrentHandState.StateKey == HandState.Free) { RightPunch(); return; }
           
        }
    }

    void LeftPunch()
    {
        _leftPunchCoroutine = _ctx.StartCoroutine(Punch(_ctx.LeftHand, _ctx.LeftAnimator, "LeftPunch"));
        _leftRightLastUsed = true; // true = rightPunch
    }

    void RightPunch()
    {
        _rightPunchCoroutine = _ctx.StartCoroutine(Punch(_ctx.RightHand, _ctx.RightAnimator, "RightPunch"));
        _leftRightLastUsed = false; // false = leftPunch
    }

    Coroutine _leftPunchCoroutine = null;

    Coroutine _rightPunchCoroutine = null;

    IEnumerator Punch(HandStateMachine hand, Animator handAnimator, string animation)
    {
        handAnimator.Play(animation);
        hand.SwitchState(HandState.Animate);

        while (_ctx.IsAttackPressed)
        {
            _chargingPunchTimer += Time.deltaTime;
            _chargingPunchTimer = Mathf.Clamp(_chargingPunchTimer, 0, _ctx.HandBaseStats.TimeToChargeMaxPunch);

            handAnimator.speed = 1 + _chargingPunchTimer / _ctx.HandBaseStats.TimeToChargeMaxPunch * 4;
            yield return 0;
        }

        hand.PunchPower = _chargingPunchTimer / _ctx.HandBaseStats.TimeToChargeMaxPunch;
        hand.FollowTransform.position = GetAttackPos();
        hand.FollowTransform.rotation = _ctx.transform.rotation;

        hand.SwitchState(HandState.Burst);

        _chargingPunchTimer = 0;
        handAnimator.speed = 1;
        handAnimator.Play("Nothing");
    }


    private Vector3 GetAttackPos()
    {
        float min = _ctx.HandBaseStats.MinMaxPunchDistance.x;
        float distLenght = _ctx.HandBaseStats.PunchDistanceLength;
        float finalDistance = (min + Mathf.Lerp(0, distLenght, _chargingPunchTimer / _ctx.HandBaseStats.TimeToChargeMaxPunch));
        Vector3 direction = (_ctx.MousePosition - _ctx.transform.position).normalized;

        return (_ctx.transform.position + new Vector3(0, 1.5f, 0)) + direction * finalDistance;
    }

    public override void ExitState()
    {
        if(_leftPunchCoroutine != null) _ctx.StopCoroutine(_leftPunchCoroutine);
        if(_rightPunchCoroutine != null) _ctx.StopCoroutine(_rightPunchCoroutine);
        _ctx.LeftHand.SwitchState(HandState.Free);
        _ctx.RightHand.SwitchState(HandState.Free);
        _ctx.RightAnimator.speed = 1;
        _ctx.LeftAnimator.speed = 1;
        _ctx.RightAnimator.Play("Nothing");
        _ctx.LeftAnimator.Play("Nothing");
    }


    public override bool CheckSwitchStates()
    {
        if ( _ctx.LeftHand.CurrentHandState.StateKey != HandState.Burst && _ctx.RightHand.CurrentHandState.StateKey != HandState.Burst &&
            _attackModeTimer < 0 ) return SwitchState(_ctx.HandsGroupStates[HandsGroupState.Idle], ref _ctx.CurrentHandsGroupStateRef);

        return false;
    }
}

