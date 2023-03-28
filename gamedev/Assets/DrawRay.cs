using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
//using System.Diagnostics;
using UnityEngine;

public class DrawRay : MonoBehaviour
{


    [SerializeField] private GameObject guardHead;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject playerHead;
    [SerializeField] private GameObject playerBody;
    [SerializeField] private GameObject playerRightLeg;
    [SerializeField] private GameObject playerLeftLeg;
    [SerializeField] private GameObject playerRightArm;
    [SerializeField] private GameObject playerLeftArm;

    

    [SerializeField] private float guardVisionRange = 8.0f;

    bool playerHeadVisible = false;
    bool playerBodyVisible = false;
    bool playerRightLegVisible = false;
    bool playerLeftLegVisible = false;
    bool playerRightArmVisible = false;
    bool playerLeftArmVisible = false;

    // Start is called before the first frame update
    void Start()
    {
        //GameObject playerHead = player.transform.GetChild(1).gameObject.transform.GetChild(2).gameObject.transform.GetChild(1).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1);
    }

    // Update is called once per frame
    void Update()
    {
        float dist = Vector4.Distance(guardHead.transform.position, playerHead.transform.position);

        if (dist <= guardVisionRange)
        {
            RaycastHit hit;
            Ray landingRay = new Ray(guardHead.transform.position, (playerHead.transform.position - guardHead.transform.position).normalized);

            //currently we are still raycasting if the guard is looking at a wall which is within 8 units. this could be better if we did an if checking if the PLAYEr was within 8 units instead.
            if (Physics.Raycast(landingRay, out hit, 8.0f))
            {
                if (hit.collider.tag == "Player")//make this more efficient by adding whether player head is visible to the if somewhere
                {
                    playerHeadVisible = true;
                    UnityEngine.Debug.Log("Hit PLAYER HEAD");
                }
                else
                {
                    playerHeadVisible = false;
                    //UnityEngine.Debug.Log("Hit something else");
                }
            }


            landingRay = new Ray(guardHead.transform.position, (playerBody.transform.position - guardHead.transform.position).normalized);

            //currently we are still raycasting if the guard is looking at a wall which is within 8 units. this could be better if we did an if checking if the PLAYEr was within 8 units instead.
            if (Physics.Raycast(landingRay, out hit, 8.0f))
            {
                if (hit.collider.tag == "Player")//make this more efficient by adding whether player head is visible to the if somewhere
                {
                    playerBodyVisible = true;
                    UnityEngine.Debug.Log("Hit PLAYER BODY");
                }
                else
                {
                    playerBodyVisible = false;
                    //UnityEngine.Debug.Log("Hit something else");
                }
            }

            landingRay = new Ray(guardHead.transform.position, (playerLeftArm.transform.position - guardHead.transform.position).normalized);

            //currently we are still raycasting if the guard is looking at a wall which is within 8 units. this could be better if we did an if checking if the PLAYEr was within 8 units instead.
            if (Physics.Raycast(landingRay, out hit, 8.0f))
            {
                if (hit.collider.tag == "Player")//make this more efficient by adding whether player head is visible to the if somewhere
                {
                    playerLeftArmVisible = true;
                    UnityEngine.Debug.Log("Hit PLAYER BOLEFT ARM");
                }
                else
                {
                    playerLeftArmVisible = false;
                    //UnityEngine.Debug.Log("Hit something else");
                }
            }

            landingRay = new Ray(guardHead.transform.position, (playerRightArm.transform.position - guardHead.transform.position).normalized);

            //currently we are still raycasting if the guard is looking at a wall which is within 8 units. this could be better if we did an if checking if the PLAYEr was within 8 units instead.
            if (Physics.Raycast(landingRay, out hit, 8.0f))
            {
                if (hit.collider.tag == "Player")//make this more efficient by adding whether player head is visible to the if somewhere
                {
                    playerRightArmVisible = true;
                    UnityEngine.Debug.Log("Hit PLAYER BORight ARM");
                }
                else
                {
                    playerRightArmVisible = false;
                    //UnityEngine.Debug.Log("Hit something else");
                }
            }

            landingRay = new Ray(guardHead.transform.position, (playerLeftLeg.transform.position - guardHead.transform.position).normalized);

            //currently we are still raycasting if the guard is looking at a wall which is within 8 units. this could be better if we did an if checking if the PLAYEr was within 8 units instead.
            if (Physics.Raycast(landingRay, out hit, 8.0f))
            {
                if (hit.collider.tag == "Player")//make this more efficient by adding whether player head is visible to the if somewhere
                {
                    playerLeftLegVisible = true;
                    UnityEngine.Debug.Log("Hit PLAYER BOLEFT Leg");
                }
                else
                {
                    playerLeftLegVisible = false;
                    //UnityEngine.Debug.Log("Hit something else");
                }
            }

            landingRay = new Ray(guardHead.transform.position, (playerRightLeg.transform.position - guardHead.transform.position).normalized);

            //currently we are still raycasting if the guard is looking at a wall which is within 8 units. this could be better if we did an if checking if the PLAYEr was within 8 units instead.
            if (Physics.Raycast(landingRay, out hit, 8.0f))
            {
                if (hit.collider.tag == "Player")//make this more efficient by adding whether player head is visible to the if somewhere
                {
                    playerRightLegVisible = true;
                    UnityEngine.Debug.Log("Hit PLAYER BORight Leg");
                }
                else
                {
                    playerRightLegVisible = false;
                    //UnityEngine.Debug.Log("Hit something else");
                }
            }

        }

        

        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerHead.transform.position, Color.red);
        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerBody.transform.position, Color.red);
        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerRightLeg.transform.position, Color.blue);
        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerLeftLeg.transform.position, Color.green);
        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerRightArm.transform.position, Color.blue);
        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerLeftArm.transform.position, Color.green);
    }
}
