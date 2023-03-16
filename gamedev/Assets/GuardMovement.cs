using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class GuardMovement : MonoBehaviour
{
    public NavMeshAgent agent;
    Animator guardAnimator;

    public GameObject[] waypoints;
    int currentWaypoint = 0;
    public float speed = 3.5f;
    bool movingToNoise = false;

    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        guardAnimator = GetComponent<Animator>();
        Debug.Log(guardAnimator);
        mainCamera = Camera.main;
    }


    // Update is called once per frame
    void Update()
    {
        
        bool isPathFinding = guardAnimator.GetBool("IsFindingPath");

        if (Input.GetMouseButtonDown(1))
        {
            agent.speed = 4.0f;
            movingToNoise = true;
            //we are storing the move position to be a ray, which is where abouts on the main camera view we have the mouse
            Ray movePosition = mainCamera.ScreenPointToRay(Input.mousePosition);

            //if our ray hit something, set this as the destination
            if(Physics.Raycast(movePosition, out var hitInfo))
            {
                agent.SetDestination(hitInfo.point);
            }
        }

        if ( !isPathFinding && agent.remainingDistance >= 0.5 )
        {
            guardAnimator.SetBool("IsFindingPath", true);
            Debug.Log(agent.remainingDistance);
        }

        if (isPathFinding && agent.remainingDistance < 0.5)
        {
            guardAnimator.SetBool("IsFindingPath", false);
            Debug.Log(agent.remainingDistance);
            movingToNoise = false;
        }

        //when not investigating a noise, move in a loop
        if(movingToNoise == false)
        {
            agent.speed = 2.0f;
            if (Vector3.Distance(this.transform.position, waypoints[currentWaypoint].transform.position) < 0.5f)
            {
                currentWaypoint++;
            }

            if (currentWaypoint >= waypoints.Length)
            {
                currentWaypoint = 0;
            }

            agent.SetDestination(waypoints[currentWaypoint].transform.position);
        }

    }
}
