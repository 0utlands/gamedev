using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFeelsPlayerState : GuardBaseState
{
    public override void enterState(GuardStateManager guard)
    {
        if (!guard.huhSound.isPlaying)
        {

            guard.huhSound.Play();
        }

        guard.agent.speed = 2.0f;
        UnityEngine.Debug.Log("Guard feeling player, going to " + guard.playerNotNullWhenTouched.transform.position);
        guard.agent.SetDestination(guard.playerNotNullWhenTouched.transform.position);

        guard.agent.isStopped = false;
    }

    public override void updateState(GuardStateManager guard)
    {
        if (guard.getIfGuardShouldChasePlayer())
        {
            guard.SwitchState(guard.chasePlayerState);
        }
        else if (guard.getIfGuardCanSeePlayer())
        {
            guard.SwitchState(guard.seePlayerState);
        }
        else if (guard.agent.remainingDistance == 0) //maybe work out if we want the AI returning to investigating the sound if teyve seen the player?
        {
            Debug.Log("Swtiching state");
            //guard.moveFromDefaultToSoundState = false;
            guard.isGuardBeingTouchedFromBehind = false;
            guard.playerNotNullWhenTouched = null;
            guard.SwitchState(guard.defaultState);
        }
    }

    
}
