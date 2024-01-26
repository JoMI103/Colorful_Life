using UnityEngine;
using static PlayerContext;

public class HandsGroupIdleState : InnerBaseState<HandsGroupState>
{
    protected PlayerContext _ctx;
    float _idleAnimationTimer;

    public HandsGroupIdleState(HandsGroupState key, PlayerContext ctx) : base(key) => _ctx = ctx;
    

    public override void EnterState()
    {
        ResetRandomIdleAnimation();
        
        Debug.LogWarning("Enter Hands Group Idle State");
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        
        _idleAnimationTimer -= Time.deltaTime;

        if(_idleAnimationTimer < 0) { ResetRandomIdleAnimation(); }
    }

    public override void ExitState()
    {
        Debug.LogWarning("Exit Hands Group Idle State");
    }

   

    void ResetRandomIdleAnimation()
    {
        _idleAnimationTimer = Random.Range(10, 30);
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.IsAttackPressed) SwitchState(_ctx.HandsGroupStates[HandsGroupState.Attack], ref _ctx.CurrentHandsGroupStateRef);

        return false;
    }
}
