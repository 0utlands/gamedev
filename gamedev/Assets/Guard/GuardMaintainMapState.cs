using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.AI;

public class GuardMaintainMapState : GuardBaseState
{
    public List<GameObject> interactorsToCheck;
    public IInteractable closestInteractorToSwitch;
    public override void enterState(GuardStateManager guard)
    {
        guard.agent.speed = 3.0f;
        guard.agent.isStopped = false;
        UnityEngine.Debug.Log("Entering maintain map state!");
        UnityEngine.Debug.Log("Object to maintain:" + guard.objToReturnToNormal.name);

        //check if the object it is trying tos wtich back to default subscribes to the hasdefault interface
        if (guard.objToReturnToNormal.TryGetComponent(out HasDefault objWithDefault))
        {

            //find all the buttons / things that toggle the object to default. The code below loops
            //through these buttons, and finds the closest one to the guard. It then sets the guard's
            //destination to that button, and when it reaches it, it will switch the object back to default.

            interactorsToCheck = objWithDefault.GetInteractors();
            Vector3 closestInteractor = new Vector3(99999,99999,99999);
            float closestDistance = 99999999;

            //loop through the buttons/interactors
            foreach (GameObject interactor in interactorsToCheck) 
            { 
                NavMeshPath path = new NavMeshPath();
                guard.agent.CalculatePath(interactor.transform.position, path);

                //if the guard cant get to this button, ignore it
                if(path.status == NavMeshPathStatus.PathPartial)
                {
                    UnityEngine.Debug.Log("Partial path.");
                } 
                else
                {
                    //if the button is the closest button, set this as the new closest button.
                    guard.agent.SetDestination(interactor.transform.position);
                    UnityEngine.Debug.Log("Remaining distance: " + RemainingDistance(guard.agent.path.corners));
                    UnityEngine.Debug.Log("Remaining distance: " + guard.agent.remainingDistance);
                    if (RemainingDistance(guard.agent.path.corners) < closestDistance)
                    {
                        if (interactor.TryGetComponent(out IInteractable interactivePart))
                        {
                            UnityEngine.Debug.Log("New Closest distance: " + RemainingDistance(guard.agent.path.corners));
                            closestDistance = RemainingDistance(guard.agent.path.corners);
                            closestInteractor = interactor.transform.position;
                            closestInteractorToSwitch = interactivePart;
                        }
                    }
                }
            }

        } else
        {
            guard.objToReturnToNormal = null;
        }
        
        //guard.agent.SetDestination();
    }

    public override void updateState(GuardStateManager guard)
    {
        //throw new System.NotImplementedException();

        if (guard.objToReturnToNormal != null)
        {
            if (guard.agent.remainingDistance < 0.5)
            {
                closestInteractorToSwitch.Interact();
                guard.objToReturnToNormal = null;
            }
        }


        if (guard.objToReturnToNormal == null)
        {
            guard.SwitchState(guard.defaultState);
        }

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
    }

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
