using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
public class GuardMovement : MonoBehaviour
{
    NavMeshAgent agent;
    Animator guardAnimator;

    public GameObject[] waypoints;
    int currentWaypoint = 0;
    public float speed = 3.5f;
    bool movingToNoise = false;
    [SerializeField] private float maxRadiusOfGuardToNoise = 2.0f;
    [SerializeField] private float rotationSpeed = 100.0f;

    private Camera mainCamera;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {
        guardAnimator = GetComponent<Animator>();
        Debug.Log(guardAnimator);
        mainCamera = Camera.main;
        agent.SetDestination(waypoints[0].transform.position);
    }


    // Update is called once per frame
    void Update()
    {
        updateGuardPosition();
    }

    public void updateGuardPosition()
    {
        goToNoiseIfOneIsAudible();

        if (movingToNoise == false)
        {
            goToNextWaypoint();
        }
    }

    public void goToNoiseIfOneIsAudible()
    {
        bool isPathFinding = guardAnimator.GetBool("IsFindingPath");

        if (Input.GetMouseButtonDown(1))
        {
            agent.speed = 4.0f;
            movingToNoise = true;
            //we are storing the move position to be a ray, which is where abouts on the main camera view we have the mouse
            Ray movePosition = mainCamera.ScreenPointToRay(Input.mousePosition);

            //if our ray hit something, set this as the destination
            if (Physics.Raycast(movePosition, out var hitInfo))
            {
                agent.SetDestination(hitInfo.point);
            }
        }

        if (!isPathFinding && agent.remainingDistance >= maxRadiusOfGuardToNoise)
        {
            guardAnimator.SetBool("IsFindingPath", true);
            Debug.Log(agent.remainingDistance);
        }

        if (isPathFinding && agent.remainingDistance < maxRadiusOfGuardToNoise)
        {
            guardAnimator.SetBool("IsFindingPath", false);
            Debug.Log(agent.remainingDistance);
            movingToNoise = false;
            agent.SetDestination(waypoints[currentWaypoint].transform.position);
        }
    }

    public void goToNextWaypoint()
    {
        if (waypoints.Length > 1)
        {
            agent.speed = 2.0f;
            if (Vector3.Distance(this.transform.position, waypoints[currentWaypoint].transform.position) < 1.5f)
            {
                if (currentWaypoint >= waypoints.Length - 1)
                {
                    currentWaypoint = 0;
                }
                else
                {
                    currentWaypoint++;
                }

                agent.SetDestination(waypoints[currentWaypoint].transform.position);
            }
        }
        else if (agent.remainingDistance < 0.5)
        {
            //agent.steeringTarget = waypoints[currentWaypoint].transform.position;

            //ROTATING THE CHARACTER TO LOOK WHERE THEY ARE GOING

            //Quaternion is a type of variable specifically for storing rotations. Lookrotation creates a rotation looking in a desired direction, in this case moveVec.
            //make a rotation define by the direction we are moving
            //Quaternion toRotation = Quaternion.LookRotation(, Vector3.up);

            //RoatteTowards rortates from current rotation to the desired direction.
            //rotate from where character currently is, to the direction theyre moving at, at the rotation speed
            transform.rotation = Quaternion.RotateTowards(transform.rotation, waypoints[currentWaypoint].transform.rotation, rotationSpeed * Time.deltaTime);
            if (Quaternion.Angle(transform.rotation, waypoints[currentWaypoint].transform.rotation) > 90)
            {
                guardAnimator.SetBool("IsFindingPath", true);
            }
            else
            {
                guardAnimator.SetBool("IsFindingPath", false);
            }

        }
    }
}
