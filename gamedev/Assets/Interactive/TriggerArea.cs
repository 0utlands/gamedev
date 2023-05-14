using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerArea : MonoBehaviour
{
    public int id;
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
