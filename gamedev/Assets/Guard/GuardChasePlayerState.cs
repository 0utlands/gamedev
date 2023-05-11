using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardChasePlayerState : GuardBaseState
{
    public override void enterState(GuardStateManager guard)
    {
        Debug.Log("CHASING PLAYER STATE ENTERED");
        guard.agent.speed = 3.0f;
        guard.agent.isStopped = false;
    }

    public override void updateState(GuardStateManager guard)
    {

        if (guard.getIfGuardIsTouchingPlayer())
        {
            Debug.Log("Caught the player!");
            GameObject.FindObjectOfType<GameManager>().EndGame();
        }

        guard.agent.SetDestination(guard.guardSenses.player.transform.position);
        if (!guard.getIfGuardCanSeePlayer())
        {
            guard.SwitchState(guard.defaultState);
        }
    }
}
