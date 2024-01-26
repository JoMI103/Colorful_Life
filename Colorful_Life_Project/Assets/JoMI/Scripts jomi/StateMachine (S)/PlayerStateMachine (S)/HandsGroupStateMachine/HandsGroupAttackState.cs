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


    public HandsGroupAttackState(HandsGroupState key, PlayerContext ctx) : base(key) => _ctx = ctx;



    public override void EnterState() { 
        Debug.LogWarning("Enter Hands Attack State");
    }

    public override void UpdateState()
    {
        if(CheckSwitchStates()) return;

        _attackModeTimer -= Time.deltaTime;
        if (_ctx.IsAttackPressed) _attackModeTimer = 1f;

        if (_chargingPunchTimer == 0 && _ctx.IsAttackPressed)
        {
            if (_ctx.LeftHand.CurrentHandState.StateKey == HandState.Free) _leftPunchCoroutine = _ctx.StartCoroutine(LeftPunch());
            else if (_ctx.RightHand.CurrentHandState.StateKey == HandState.Free) _rightPunchCoroutine = _ctx.StartCoroutine(RightPunch());
        }
    }

    Coroutine _leftPunchCoroutine = null;

    IEnumerator LeftPunch()
    {
        _ctx.LeftAnimator.Play("LeftPunch");
        _ctx.LeftHand.SwitchState(HandState.Animate);
        while (_ctx.IsAttackPressed)
        {
            

            _chargingPunchTimer += Time.deltaTime;
            yield return 0;
        }
        _ctx.LeftHand.SetFollowTarget(GetAttackPos(),Vector3.zero);
        _ctx.LeftHand.SwitchState(HandState.Burst);
        _chargingPunchTimer = 0;
        _ctx.LeftAnimator.Play("Nothing");

    }

    Coroutine _rightPunchCoroutine = null;

    IEnumerator RightPunch()
    {
        _ctx.RightAnimator.Play("RightPunch");
        _ctx.RightHand.SwitchState(HandState.Animate);
        while (_ctx.IsAttackPressed)
        {
            

            _chargingPunchTimer += Time.deltaTime;
            yield return 0;
        }
        _ctx.RightHand.SetFollowTarget(GetAttackPos(), Vector3.zero);
        _ctx.RightHand.SwitchState(HandState.Burst);
        _chargingPunchTimer = 0;
        _ctx.RightAnimator.Play("Nothing");

    }

    private Vector3 GetAttackPos()
    {
        float min = _ctx.HandBaseStats.MinPunchDistance, max = _ctx.HandBaseStats.MaxPunchDistance;
        float dist = max - min;
        Debug.LogError(min + Mathf.Lerp(0, dist, _chargingPunchTimer * _ctx.HandBaseStats.punchChargTimeMult));
        return _ctx.transform.position + (_ctx.MousePosition - _ctx.transform.position).normalized * 
            (min + Mathf.Lerp(0, dist, _chargingPunchTimer * _ctx.HandBaseStats.punchChargTimeMult)); 
    }

    public override void ExitState()
    {
        if(_leftPunchCoroutine != null) _ctx.StopCoroutine(_leftPunchCoroutine);
        if(_rightPunchCoroutine != null) _ctx.StopCoroutine(_rightPunchCoroutine);
        _ctx.LeftHand.SwitchState(HandState.Free);
        _ctx.RightHand.SwitchState(HandState.Free);
        _ctx.RightAnimator.Play("Nothing");
        _ctx.LeftAnimator.Play("Nothing");

        Debug.LogWarning("Exit Hands Attack State");
    }


    public override bool CheckSwitchStates()
    {
        

        if (!_ctx.IsAttackPressed &&_attackModeTimer < 0 ) SwitchState(_ctx.HandsGroupStates[HandsGroupState.Idle], ref _ctx.CurrentHandsGroupStateRef);

        return false;
    }
}


//Codigo para ajudar
/*
: InnerBaseState<HandsState>
{
    protected PlayerContext _ctx;

    public HandsAttackState(HandsState key, PlayerContext ctx) : base(key) => _ctx = ctx;


    public override void EnterState()
    {
        Debug.LogWarning("Enter Hands Attack State");
    }

    public override void UpdateState()
    {
        CheckSwitchStates();
    }

    public override void ExitState()
    {
        Debug.LogWarning("Exit Hands Attack State");
    }

    public override bool CheckSwitchStates()
    {
        return true;
    }
 */