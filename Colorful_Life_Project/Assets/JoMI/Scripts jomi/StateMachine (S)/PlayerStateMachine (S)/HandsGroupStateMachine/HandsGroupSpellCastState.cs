using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerContext;

public class HandsGroupSpellCastState : InnerBaseState<HandsGroupState>
{
    protected PlayerContext _ctx;

    public HandsGroupSpellCastState(HandsGroupState key, PlayerContext ctx) : base(key) => _ctx = ctx;


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
}
