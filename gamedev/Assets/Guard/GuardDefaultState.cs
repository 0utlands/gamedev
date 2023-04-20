using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardDefaultState : GuardBaseState
{
    private float maxRadiusOfGuardToWaypoint = 2.0f;
    public override void enterState(GuardStateManager guard)
    {
        UnityEngine.Debug.Log("I am in the default state");
        guard.agent.speed = 2.0f;
        guard.agent.SetDestination(guard.waypoints[guard.currentWaypoint].transform.position);
    }

    public override void updateState(GuardStateManager guard)
    {
        //UnityEngine.Debug.Log("I am updating in the default state");

        if (guard.getIfGuardShouldChasePlayer())
        {
            guard.SwitchState(guard.chasePlayerState);
        }
        else if (guard.getIfGuardCanSeePlayer())
        {
            guard.SwitchState(guard.seePlayerState);
        }
        else if (guard.getIfShouldReactToSound())
        {
            guard.SwitchState(guard.hearNoiseState);
        }
         
        goToNextWaypoint(guard);
        //guard.alertness = helper.updateAlertness(guard)

        //guard.transform.localScale += new Vector3(1.1f, 1.1f, 1.1f) * Time.deltaTime;

    }

    private void goToNextWaypoint(GuardStateManager guard)
    {
        bool isPathFinding = guard.guardAnimator.GetBool("IsFindingPath");

        if (!isPathFinding && guard.agent.remainingDistance >= maxRadiusOfGuardToWaypoint)
        {
            guard.guardAnimator.SetBool("IsFindingPath", true);
            //Debug.Log(guard.agent.remainingDistance);
        }

        if (isPathFinding && guard.agent.remainingDistance < maxRadiusOfGuardToWaypoint)
        {
            guard.guardAnimator.SetBool("IsFindingPath", false);
            //Debug.Log(guard.agent.remainingDistance);
            guard.agent.SetDestination(guard.waypoints[guard.currentWaypoint].transform.position);
        }



        if (guard.waypoints.Length > 1)
        {
            guard.agent.speed = 2.0f;
            if (Vector3.Distance(guard.transform.position, guard.waypoints[guard.currentWaypoint].transform.position) < 1.5f)
            {
                if (guard.currentWaypoint >= guard.waypoints.Length - 1)
                {
                    guard.currentWaypoint = 0;
                }
                else
                {
                    guard.currentWaypoint++;
                }

                guard.agent.SetDestination(guard.waypoints[guard.currentWaypoint].transform.position);
            }
        }
        else if (guard.agent.remainingDistance < 0.5)
        {
            //agent.steeringTarget = waypoints[currentWaypoint].transform.position;

            //ROTATING THE CHARACTER TO LOOK WHERE THEY ARE GOING

            //Quaternion is a type of variable specifically for storing rotations. Lookrotation creates a rotation looking in a desired direction, in this case moveVec.
            //make a rotation define by the direction we are moving
            //Quaternion toRotation = Quaternion.LookRotation(, Vector3.up);

            //RoatteTowards rortates from current rotation to the desired direction.
            //rotate from where character currently is, to the direction theyre moving at, at the rotation speed
            guard.transform.rotation = Quaternion.RotateTowards(guard.transform.rotation, guard.waypoints[guard.currentWaypoint].transform.rotation, guard.rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(guard.transform.rotation, guard.waypoints[guard.currentWaypoint].transform.rotation) > 90)
            {
                guard.guardAnimator.SetBool("IsFindingPath", true);
            }
            else
            {
                guard.guardAnimator.SetBool("IsFindingPath", false);
            }

        }
    }

}
