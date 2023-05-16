using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    public void Awake()
    {
        instance = this; 
        this.GetComponent<StandaloneInputModule>().enabled = true;
    }

    public event Action<int> onDoorwayTriggerEnter;
    public void DoorwayOpen(int id)
    {
        if (onDoorwayTriggerEnter != null)
        {
            onDoorwayTriggerEnter(id);
        }
    }

    public event Action<int> onDoorwayTriggerExit;
    public void DoorwayClose(int id)
    {
        if (onDoorwayTriggerExit != null)
        {
            onDoorwayTriggerExit(id);
        }
    }

    public event Action<int> onDoorwayToggle;
    public void DoorwayToggle(int id)
    {
        if (onDoorwayToggle != null)
        {
            onDoorwayToggle(id);
        }
    }
}
