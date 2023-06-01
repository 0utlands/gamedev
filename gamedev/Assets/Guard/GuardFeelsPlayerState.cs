using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFeelsPlayerState : GuardBaseState
{
    public override void enterState(GuardStateManager guard)
    {

        //this state is entered if the player gets very close behind the guard, while the guard cant see them. it makes the guard turn towarsd the player.

        //originally, we had the guards say "huh!?" when the player touched them, as an audio cue. this felt wrong when game testing, so the sound was changed to a stinger instead.
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
        //if we see the player, deal with them.
        if (guard.getIfGuardShouldChasePlayer())
        {
            guard.SwitchState(guard.chasePlayerState);
        }
        else if (guard.getIfGuardCanSeePlayer())
        {
            guard.SwitchState(guard.seePlayerState);
        }
        else if (guard.agent.remainingDistance == 0) //if weve investigated the touch, return to default state.
        {
            Debug.Log("Swtiching state");
            //guard.moveFromDefaultToSoundState = false;
            guard.isGuardBeingTouchedFromBehind = false;
            guard.playerNotNullWhenTouched = null;
            guard.SwitchState(guard.defaultState);
        }
    }

    
}
