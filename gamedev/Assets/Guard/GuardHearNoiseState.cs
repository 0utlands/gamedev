using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class GuardHearNoiseState : GuardBaseState
{

    private bool retry = false;
    private float frame = 0;
    private float maxFrames = 24;

    public override void enterState(GuardStateManager guard)
    {

        //if we have heard a sound, go to the location of the sound.
        Debug.Log("HEARING SOUND STATE ENTERED");
        guard.agent.speed = 2.0f;
        UnityEngine.Debug.Log("Guard hearing updating, going to " + guard.mostRecentSoundHeard.pos);
        guard.agent.SetDestination(guard.mostRecentSoundHeard.pos);
        
        guard.agent.isStopped = false;
    }

    public override void updateState(GuardStateManager guard)
    {
        //stop moving when we have found the sounds origin.
        bool shouldGuardStop = (RemainingDistance(guard.agent.path.corners) < 0.5);

        //Debug.Log("Should guiard stop?" + shouldGuardStop);
        if(shouldGuardStop)
        {
            //Debug.Log("Guard stopping. remaining distance: " + RemainingDistance(guard.agent.path.corners));
            //Debug.Log("Number of corners: " + guard.agent.path.corners.Length);
            retry = true;
        }

        //sometimes the remaining distances is incorrect on the first few frames, even with cole slaters function. this code keeps checking for a few frames to mitigate this.

        if (frame < maxFrames && retry == true)
        {
            frame += 1;
        }

        if (frame >= maxFrames && retry == true)
        {
            retry = false;
            frame = 0;
        }


        //if we should switch states, switch states.
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
        else if (guard.agent.remainingDistance < 0.5 && retry == false) 
        {
            Debug.Log("Swtiching state");
            guard.moveFromDefaultToSoundState = false;
            guard.SwitchState(guard.defaultState);
        }
    }

    //unitys agent.renmainindDistance can sometime be unreliable - it sometimes returns a remaining distance of zero on the first call. this function mitigates that issue.
    //author: cole slater at https://forum.unity.com/threads/unity-navmesh-get-remaining-distance-infinity.415814/
    public float RemainingDistance(Vector3[] points)
    {
        if (points.Length < 2) return 0;
        float distance = 0;
        for (int i = 0; i < points.Length - 1; i++)
            distance += Vector3.Distance(points[i], points[i + 1]);
        return distance;
    }
}


