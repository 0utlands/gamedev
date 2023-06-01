using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardHuntPlayerState : GuardBaseState
{
    public override void enterState(GuardStateManager guard)
    {
        //this state is entered if the guard been chasing the player, but their alertness has dropped below 100, measning theyve lost sight.
        guard.agent.isStopped = false;
    }

    public override void updateState(GuardStateManager guard)
    {
        
        //keep hunting the player until alertness reaches zero.
        guard.agent.SetDestination(guard.guardSenses.player.transform.position);
        
        //if alertness reaches zero, go back to default state. if alertness reaches 100, chase the player again.
        if (guard.getIfGuardShouldStopHuntingPlayer())
        {
            guard.SwitchState(guard.defaultState);
        }
        if (guard.getIfGuardShouldChasePlayer())
        {
            guard.SwitchState(guard.chasePlayerState);
        }
    }
}
