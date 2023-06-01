using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;

public class GuardDefaultState : GuardBaseState
{
    private float maxRadiusOfGuardToWaypoint = 2.0f;
    public override void enterState(GuardStateManager guard)
    {
        //UnityEngine.Debug.Log("I am in the default state");
        guard.agent.speed = 2.0f;

        //set the guards destination as their current waypoint.
        guard.agent.SetDestination(guard.waypoints[guard.currentWaypoint].transform.position);
        //UnityEngine.Debug.Log(this.GetType().Name);
    }

    public override void updateState(GuardStateManager guard)
    {
        //UnityEngine.Debug.Log("I am updating in the default state");

        //check if we should switch state.
        if (guard.getIfGuardShouldChasePlayer())
        {
            guard.SwitchState(guard.chasePlayerState);
        }
        else if (guard.getIfGuardCanSeePlayer())
        {
            guard.SwitchState(guard.seePlayerState);
        }
        else if (guard.getIfGuardIsBeingTouchedFromBehind())
        {
            guard.SwitchState(guard.feelsPlayerState);
        }
        else if (guard.getIfShouldReactToSound())
        {
            guard.SwitchState(guard.hearNoiseState);
        }
        else if (guard.objToReturnToNormal != null) 
        { 
            guard.SwitchState(guard.maintainMapState);
        }
         
        //if we shoudlnt switch state, continue moving to the next waypoint.
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


       //this code checks if other guards are going to the same waypoint as this guard, and if so, the guards which are furhter away from the waypoint will slow down (remember, every guard is running this check. only the cloest guard will move fastest)
        GameObject[] guards = GameObject.FindGameObjectsWithTag("Guard");  
        bool isAnotherGuardGoingToMyWayPoint = false;

        foreach (GameObject otherGuard in guards)
        {
            if (otherGuard != guard.gameObject)
            {
                if (otherGuard.GetComponent<GuardStateManager>() != null)
                {
                    GuardStateManager otherGuardManager = otherGuard.GetComponent<GuardStateManager>();
                    if (guard.waypoints[guard.currentWaypoint].transform.position == otherGuardManager.waypoints[otherGuardManager.currentWaypoint].transform.position && (otherGuardManager.currentState.GetType().Name == "GuardDefaultState"))
                    {
                        if (RemainingDistance(guard.agent.path.corners) > RemainingDistance(otherGuardManager.agent.path.corners))
                        {
                            // guard.agent.speed = 1.0f;
                            isAnotherGuardGoingToMyWayPoint = true;
                        }
                        else
                        {
                            // guard.agent.speed = 2.0f;
                            isAnotherGuardGoingToMyWayPoint = false;
                        }
                    }
                    else
                    {
                       // guard.agent.speed = 2.0f;
                    }

                }
            }

        }

        if (isAnotherGuardGoingToMyWayPoint)
        {
            guard.agent.speed = 1.0f;
        }
        else
        {
            guard.agent.speed = 2.0f;
        }


        /*

        foreach (GameObject otherGuard in guards)
        {
            if (otherGuard != guard.gameObject) { 
                if (otherGuard.GetComponent<GuardStateManager>() != null)
                {
                    GuardStateManager otherGuardManager = otherGuard.GetComponent<GuardStateManager>();
                    if (guard.waypoints[guard.currentWaypoint].transform.position == otherGuardManager.waypoints[otherGuardManager.currentWaypoint].transform.position && (otherGuardManager.currentState.GetType().Name == "GuardDefaultState"))
                    {
                        
                        UnityEngine.Debug.Log("Waypoint of other guard:" + otherGuardManager.waypoints[otherGuardManager.currentWaypoint].transform.position.ToString());

                        if (RemainingDistance(guard.agent.path.corners) > RemainingDistance(otherGuardManager.agent.path.corners))
                        {
                            isAnotherGuardGoingToMyWayPoint = true;
                        }
                        
                    }
                    
                }
            }

        }

        */


        //loop thriough the waypoints
        if (guard.waypoints.Length > 1)
        {
            //guard.agent.speed = 2.0f;

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
