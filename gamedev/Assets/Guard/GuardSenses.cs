using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class GuardSenses
{


    //guard vision stuff
    public GameObject player;
    private GameObject playerHead;
    private GameObject playerBody;
    private GameObject playerRightLeg;
    private GameObject playerLeftLeg;
    private GameObject playerRightArm;
    private GameObject playerLeftArm;

    private GameObject guardHead;
    private GameObject guard;

    private float currentAlertness;
    
    private AlertBarScript alertBar;
    private float rateOfDiscovery; //this is a float representing how quickly a guard discovers the player. It is higher if more body parts of the player are visible to the guard, and if the player is closer to the guard.
    
    //should probably work out a better way of setting these - maybe there could be a guardSensesProfile object which comes in, and passes these to it??
    private float guardFov;
    private float guardVisionRange;
    private float maxAlertness;

    bool playerHeadVisible = false;
    bool playerBodyVisible = false;
    bool playerRightLegVisible = false;
    bool playerLeftLegVisible = false;
    bool playerRightArmVisible = false;
    bool playerLeftArmVisible = false;
    public GuardSenses(GuardStateManager guardStateManager)
    {
        //finding the players body parts (they are children of the gameobject named "Player")
        player = GameObject.Find("Player");
        playerLeftArm = GameObject.Find("Player/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 L Clavicle/Bip001 L UpperArm/Bip001 L Forearm/Bip001 L Hand");
        playerRightArm = GameObject.Find("Player/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 R Clavicle/Bip001 R UpperArm/Bip001 R Forearm/Bip001 R Hand");
        playerHead = GameObject.Find("Player/Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/HEAD_CONTAINER");
        playerBody = GameObject.Find("Player/Body_09_jacket");
        playerLeftLeg = GameObject.Find("Player/Bip001/Bip001 Pelvis/Bip001 L Thigh/Bip001 L Calf");
        playerRightLeg = GameObject.Find("Player/Bip001/Bip001 Pelvis/Bip001 R Thigh/Bip001 R Calf");

        //find the guard's head and alert bar using the gameobject passed in through the guardStateManager
        guard = guardStateManager.gameObject;
        guardHead = guard.transform.Find("Bip001/Bip001 Pelvis/Bip001 Spine/Bip001 Neck/Bip001 Head/HEAD_CONTAINER").gameObject;
        alertBar = guard.transform.Find("Canvas/AlertBar").gameObject.GetComponent<AlertBarScript>();

        //get the guards vision properties from the guard state manager.
        guardFov = guardStateManager.guardFov;
        guardVisionRange = guardStateManager.guardVisionRange;
        maxAlertness = guardStateManager.maxAlertness;
        alertBar.SetMaxAlertness(maxAlertness);
    }

    public void updateGuardSenses() 
    {
        updateRateOfDiscoveryIfPlayerVisible();
    }

    public void fixedUpdateGuardAlertness(GuardStateManager guardStateManager) 
    {
        updateAlertnessUsingRateOfDiscovery();
        updateWhetherObjInWrongState(guardStateManager);


    }

    public void updateWhetherObjInWrongState(GuardStateManager guardStateManager)
    {
        //Debug.Log("checking default time");
        Collider[] col = Physics.OverlapSphere(guard.transform.position, guardVisionRange);
        //need to find a way from transitioning from 

        List<GameObject> objsToReturnToNormal = new List<GameObject>();

        for (int i = 0; i < col.Length; i++)
        {
            //Debug.Log(col[i]);
            if (col[i].TryGetComponent(out HasDefault objWithDefault))
            {
                //Debug.Log("obj with default state responding to AI");
                if (!objWithDefault.GetIfInDefaultState())
                {

                    float distanceBetweenGuardAndObj = Vector4.Distance(guard.transform.position, col[i].transform.position);
                    float angleToObjFromGuardsEye = Vector3.Angle(guard.transform.forward, (col[i].transform.position - guard.transform.position));
                    bool objIsVisible = isObjectVisibleToGuard(distanceBetweenGuardAndObj, angleToObjFromGuardsEye);

                    if (objIsVisible)
                    {
                        objsToReturnToNormal.Add(col[i].gameObject);
                        guardStateManager.objToReturnToNormal = col[i].gameObject;
                        //GameObject[] interactors = objWithDefault.GetInteractors();
                    }
                }
            }
        }
    }

    public float GetGuardAlertness()
    {
        return currentAlertness;
    }

    public bool canGuardSeePlayer()
    {
        return currentAlertness > 0;
    }

    public bool isGuardAtMaxAlertness()
    {
        return (currentAlertness > 0) && (currentAlertness >= maxAlertness);
    }

    public void updateRateOfDiscoveryIfPlayerVisible()
    {

        float distanceBetweenGuardAndPlayer = Vector4.Distance(guard.transform.position, player.transform.position);
        float angleToPlayerFromGuardsEye = Vector3.Angle(guard.transform.forward, (player.transform.position - guard.transform.position));
        bool playerIsVisible = isObjectVisibleToGuard(distanceBetweenGuardAndPlayer, angleToPlayerFromGuardsEye);

        
        //UnityEngine.Debug.Log("Distance from guard to player: " + distanceBetweenGuardAndPlayer);
        //UnityEngine.Debug.Log("angle between guard and player: " + angleToPlayerFromGuardsEye);

        if (playerIsVisible)
        {
            //UnityEngine.Debug.Log("Player is visible FROM GUARDSENSES");
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
        //UnityEngine.Debug.Log("ROD:  " + rateOfDiscovery);
    }

    public void updateAlertnessUsingRateOfDiscovery()
    {
        //Debug.Log("Max alertness: " + maxAlertness);
        if ((currentAlertness < maxAlertness) && (rateOfDiscovery > 0))
        {
            currentAlertness += rateOfDiscovery;
        }
        else if (currentAlertness > 0)
        {
            currentAlertness -= 0.5f;
        }
        //UnityEngine.Debug.Log("Current alertness: " + currentAlertness);
        alertBar.SetAlertness(currentAlertness);
    }

    public bool isObjectVisibleToGuard(float distance, float angle)
    {
        UnityEngine.Debug.DrawLine(guard.transform.position, guard.transform.forward * 20 + guard.transform.position, Color.white);
        //UnityEngine.Debug.Log("Angle between guard and player: " + angle);
        //UnityEngine.Debug.Log("guardhead direction vec: " + guardHead.transform.forward);

        return isObjectWithinGuardFov(angle) && isObjectWithinGuardsVisionRange(distance);
    }

    public bool isObjectWithinGuardFov(float angle)
    {
        return angle < (guardFov / 2);
    }

    public bool isObjectWithinGuardsVisionRange(float distance)
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
                //UnityEngine.Debug.Log("Hit PLAYER BOLEFT ARM");
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
                //UnityEngine.Debug.Log("Hit PLAYER BORight ARM");
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
                //UnityEngine.Debug.Log("Hit PLAYER BOLEFT Leg");
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
                //UnityEngine.Debug.Log("Hit PLAYER BORight Leg");
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
        //UnityEngine.Debug.Log(rateOfDiscovery);
    }
}
