using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSeePlayerState : GuardBaseState
{
    public override void enterState(GuardStateManager guard)
    {
        Debug.Log("SEE PLAYER STATE ENTERED");
    }

    public override void updateState(GuardStateManager guard)
    {
        Debug.Log("SEE PLAYER STATE UPDATING");

        guard.agent.SetDestination(guard.guardSenses.player.transform.position);
        guard.agent.speed = 1.0f;

        if (guard.currentAlertness <= 0.0f)
        {
            guard.SwitchState(guard.defaultState);
        }
    }
}
