using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardChasePlayerState : GuardBaseState
{
    public override void enterState(GuardStateManager guard)
    {
        //entered if the guard is at max alertness. the guard will chase the player.
        guard.agent.speed = 3.0f;
        guard.agent.isStopped = false;
    }

    public override void updateState(GuardStateManager guard)
    {
        //if the guard touches the player, the player is caught, the level restarts.
        if (guard.getIfGuardIsTouchingPlayer())
        {
            //Debug.Log("Caught the player!");
            GameObject.FindObjectOfType<GameManager>().EndGame();
        }

        //else, keep moving to the players location.
        guard.agent.SetDestination(guard.guardSenses.player.transform.position);
        
        //if alertness drops below max, enter the hunting player state.
        if (!guard.getIfGuardShouldChasePlayer())
        {
            guard.SwitchState(guard.huntPlayerState);
        }

    }
}
