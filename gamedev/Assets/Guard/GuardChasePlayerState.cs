using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardChasePlayerState : GuardBaseState
{
    public override void enterState(GuardStateManager guard)
    {
        Debug.Log("CHASING PLAYER STATE ENTERED");
        guard.agent.speed = 3.0f;
    }

    public override void updateState(GuardStateManager guard)
    {
        if (!guard.getIfGuardShouldChasePlayer())
        {
            guard.SwitchState(guard.seePlayerState);
        }
    }
}
