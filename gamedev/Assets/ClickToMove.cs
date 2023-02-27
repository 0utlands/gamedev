using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class ClickToMove : MonoBehaviour
{
    public NavMeshAgent agent;
    Animator guardAnimator;

    // Start is called before the first frame update
    void Start()
    {
        guardAnimator = GetComponent<Animator>();
        Debug.Log(guardAnimator);
    }


    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(1))
        {
            //we are storing the move position to be a ray, which is where abouts on the main camera view we have the mouse
            Ray movePosition = Camera.main.ScreenPointToRay(Input.mousePosition);

            //if our ray hit something, set this as the destination
            if(Physics.Raycast(movePosition, out var hitInfo))
            {
                agent.SetDestination(hitInfo.point);
            }
        }

        if ( agent.remainingDistance >= 0.5 )
        {
            guardAnimator.SetBool("IsFindingPath", true);
            Debug.Log(agent.remainingDistance);
        }

        if (agent.remainingDistance < 0.5)
        {
            guardAnimator.SetBool("IsFindingPath", false);
            Debug.Log(agent.remainingDistance);
        }
    }
}
