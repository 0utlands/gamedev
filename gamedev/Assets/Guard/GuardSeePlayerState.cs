using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSeePlayerState : GuardBaseState
{
    public override void enterState(GuardStateManager guard)
    {
        //Debug.Log("SEE PLAYER STATE ENTERED");
        guard.agent.speed = 1.0f;
        guard.agent.isStopped = false;
    }

    public override void updateState(GuardStateManager guard)
    {
        
        //the gaurd is alert to the player. set the guards Ai destination to be the players location, and make their speed a bit slower so they creep towards them.
        guard.agent.SetDestination(guard.guardSenses.player.transform.position);
        //coudl move this to a fixedUpdateState function to make it a bit less cumbersome for computer
        if (!guard.getIfGuardCanSeePlayer())
        {
            guard.SwitchState(guard.defaultState);
        }
        if (guard.getIfGuardShouldChasePlayer())
        {
            guard.SwitchState(guard.chasePlayerState);
        }
    }
}
