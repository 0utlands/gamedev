using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public int id;

    //used for making automatic doors. when the player enters the trigger area, the door opens. when the player leaves the trigger area, the door closes.
    private void OnTriggerEnter(Collider other)
    {
        //print("trigger entered");
        GameEvents.instance.DoorwayOpen(id);
    }

    private void OnTriggerExit(Collider other)
    {
        //print("trigger entered");
        GameEvents.instance.DoorwayClose(id);
    }
}
