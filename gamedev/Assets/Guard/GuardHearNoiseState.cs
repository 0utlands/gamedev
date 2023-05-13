using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GuardHearNoiseState : GuardBaseState
{
    public override void enterState(GuardStateManager guard)
    {
        Debug.Log("HEARING SOUND STATE ENTERED");
        guard.agent.speed = 2.0f;
        UnityEngine.Debug.Log("Guard hearing updating, going to " + guard.mostRecentSoundHeard.pos);
        guard.agent.SetDestination(guard.mostRecentSoundHeard.pos);
        
        guard.agent.isStopped = false;
    }

    public override void updateState(GuardStateManager guard)
    {

        //Debug.Log("Guard remaining distance:" + guard.agent.remainingDistance);
        bool shoulsGuardStop = (guard.agent.remainingDistance < 0.5);
        Debug.Log("Should guiard stop?" + shoulsGuardStop);
        //coudl move this to a fixedUpdateState function to make it a bit less cumbersome for computer
        if (guard.getIfGuardShouldChasePlayer())
        {
            Debug.Log("Chasing player");
            guard.SwitchState(guard.chasePlayerState);
        }
        else if (guard.getIfGuardCanSeePlayer())
        {
            Debug.Log("seeing player");
            guard.SwitchState(guard.seePlayerState);
        }
        else if (guard.agent.remainingDistance < 0.5) //maybe work out if we want the AI returning to investigating the sound if teyve seen the player?
        {
            Debug.Log("Swtiching state");
            guard.moveFromDefaultToSoundState = false;
            guard.SwitchState(guard.defaultState);
        }
    }
}
