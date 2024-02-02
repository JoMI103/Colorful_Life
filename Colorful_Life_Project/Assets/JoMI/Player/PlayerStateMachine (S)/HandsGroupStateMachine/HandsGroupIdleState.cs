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
    }

    public override void UpdateState()
    {
        if (CheckSwitchStates()) return;
        
        _idleAnimationTimer -= Time.deltaTime;

        if(_idleAnimationTimer < 0) { ResetRandomIdleAnimation(); }
    }

    public override void ExitState()
    {
    }

   

    void ResetRandomIdleAnimation()
    {
        _idleAnimationTimer = Random.Range(10, 30);
    }

    public override bool CheckSwitchStates()
    {
        if (_ctx.IsAttackPressed) return SwitchState(_ctx.HandsGroupStates[HandsGroupState.Attack], ref _ctx.CurrentHandsGroupStateRef);

        if(Input.GetKeyDown(KeyCode.J) && _ctx.PlayerInfo.CurrentMagic != PlayerInfo.Magic.None) return SwitchState(_ctx.HandsGroupStates[HandsGroupState.SpellCast], ref _ctx.CurrentHandsGroupStateRef);

        if(Input.GetKeyDown(KeyCode.E) && _ctx.CurrentIGrabbable != null) return SwitchState(_ctx.HandsGroupStates[HandsGroupState.Grab], ref _ctx.CurrentHandsGroupStateRef);
        return false;
    }
}
