using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
//using System.Diagnostics;
using UnityEngine;

public class DrawRay : MonoBehaviour
{

    //used in editor to mak the guards vision rays appear.

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

    public float currentAlertness;
    public float maxAlertness = 100;
    public AlertBarScript alertBar;
    float rateOfDiscovery; //this is a float representing how quickly a guard discovers the player. It is higher if more body parts of the player are visible to the guard, and if the player is closer to the guard.
    [SerializeField] private float guardFov;

    // Start is called before the first frame update
    void Start()
    {
        currentAlertness = 0;
        alertBar.SetMaxAlertness(maxAlertness);
        /*UnityEngine.Debug.Log(GetGameObjectPath(playerHead));
        UnityEngine.Debug.Log(GetGameObjectPath(playerBody));
        UnityEngine.Debug.Log(GetGameObjectPath(playerLeftArm));
        UnityEngine.Debug.Log(GetGameObjectPath(playerRightArm));
        UnityEngine.Debug.Log(GetGameObjectPath(playerLeftLeg));
        UnityEngine.Debug.Log(GetGameObjectPath(playerRightLeg));*/
        
    }

    // Update is called once per frame
    void Update()
    {
        updateRateOfDiscoveryIfPlayerVisible();
    }

    void FixedUpdate()
    {
        updateAlertnessUsingRateOfDiscovery();
    }

    public void SetPlayer(GameObject player)
    {
        playerHead = player.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/HEAD_CONTAINER").gameObject;
        playerBody = player.transform.Find("Body_09_jacket").gameObject;
        playerLeftArm = player.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand").gameObject;
        playerRightArm = player.transform.Find("Bip001 Spine/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand").gameObject;
        playerLeftLeg = player.transform.Find("Bip001/Bip001 Pelvis/Bip001 L Thigh/Bip001 L Calf").gameObject;
        playerRightLeg = player.transform.Find("Bip001/Bip001 Pelvis/Bip001 R Thigh/Bip001 R Calf").gameObject;
    }

    public static string GetGameObjectPath(GameObject obj)
    {
        string path = "/" + obj.name;
        while (obj.transform.parent != null)
        {
            obj = obj.transform.parent.gameObject;
            path = "/" + obj.name + path;
        }
        return path;
    }

    public void updateRateOfDiscoveryIfPlayerVisible()
    {

        float distanceBetweenGuardAndPlayer = Vector4.Distance(guardHead.transform.position, playerHead.transform.position);
        float angleToPlayerFromGuardsEye = Vector3.Angle(transform.forward, (player.transform.position - transform.position));
        bool playerIsVisible = isPlayerVisibleToGuard(distanceBetweenGuardAndPlayer, angleToPlayerFromGuardsEye);

        
        if (playerIsVisible)
        {
            updateWhichPlayerBodyPartsAreVisible();

        }
        else
        {
            setVisibilityOfAllBodyPartsToFalse();
        }

        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerHead.transform.position, Color.red);
        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerBody.transform.position, Color.red);
        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerRightLeg.transform.position, Color.blue);
        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerLeftLeg.transform.position, Color.green);
        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerRightArm.transform.position, Color.blue);
        UnityEngine.Debug.DrawLine(guardHead.transform.position, playerLeftArm.transform.position, Color.green);

        int visibleBodyParts = calculateNumOfVisibleBodyParts();
        updateRateOfDiscovery(visibleBodyParts, distanceBetweenGuardAndPlayer);
    }

    public void updateAlertnessUsingRateOfDiscovery()
    {
        if ((currentAlertness < maxAlertness) && (rateOfDiscovery > 0))
        {
            currentAlertness += rateOfDiscovery;
        }
        else if (currentAlertness > 0)
        {
            currentAlertness -= 1;
        }
        //UnityEngine.Debug.Log("Current alertness: " + currentAlertness);
        alertBar.SetAlertness(currentAlertness);
    }

    public bool isPlayerVisibleToGuard(float distance, float angle)
    {
        UnityEngine.Debug.DrawLine(transform.position, transform.forward * 20 + transform.position, Color.white);
        //UnityEngine.Debug.Log("Angle between guard and player: " + angle);
        //UnityEngine.Debug.Log("guardhead direction vec: " + guardHead.transform.forward);

        return isPlayerWithinGuardFov(angle) && isPlayerWithinGuardsVisionRange(distance);
    }

    public bool isPlayerWithinGuardFov(float angle)
    {
        return angle < (guardFov / 2);
    }

    public bool isPlayerWithinGuardsVisionRange(float distance)
    {
        return distance <= guardVisionRange;
    }

    public void updateWhichPlayerBodyPartsAreVisible()
    {
        RaycastHit hit;
        Ray landingRay = new Ray(guardHead.transform.position, (playerHead.transform.position - guardHead.transform.position).normalized);

        //currently we are still raycasting if the guard is looking at a wall which is within 8 units. this could be better if we did an if checking if the PLAYEr was within 8 units instead.
        if (Physics.Raycast(landingRay, out hit, 8.0f))
        {
            if (hit.collider.tag == "Player")//make this more efficient by adding whether player head is visible to the if somewhere
            {
                playerHeadVisible = true;
                //UnityEngine.Debug.Log("Hit PLAYER HEAD");
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
                //UnityEngine.Debug.Log("Hit PLAYER BODY");
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

    private void setVisibilityOfAllBodyPartsToFalse()
    {
        playerHeadVisible = false;
        playerBodyVisible = false;
        playerRightArmVisible = false;
        playerLeftArmVisible = false;
        playerRightLegVisible = false;
        playerLeftLegVisible = false;
    }

    private int calculateNumOfVisibleBodyParts()
    {
        //create a value from 0 to 6 depending on how many out of the 6 body parts are visible
        int visibleBodyParts = 0;
        if (playerHeadVisible)
        {
            visibleBodyParts++;
        }
        if (playerBodyVisible)
        {
            visibleBodyParts++;
        }
        if (playerLeftArmVisible)
        {
            visibleBodyParts++;
        }
        if (playerRightArmVisible)
        {
            visibleBodyParts++;
        }
        if (playerLeftLegVisible)
        {
            visibleBodyParts++;
        }
        if (playerRightLegVisible)
        {
            visibleBodyParts++;
        }
        return visibleBodyParts;
    }

    private void updateRateOfDiscovery(int visibleBodyParts, float distanceBetweenGuardAndPlayer)
    {
        rateOfDiscovery = 0.5f * visibleBodyParts * (8 - distanceBetweenGuardAndPlayer);
        UnityEngine.Debug.Log(rateOfDiscovery);
    }


   


}
