using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowWayPoint : MonoBehaviour
{
    public NavMeshAgent agent;
    public GameObject[] waypoints;
    int currentWaypoint = 0;
    public float speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
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
