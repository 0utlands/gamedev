using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;
    //game events system to allow doors, buttons, trigger areas, etc. to communicate with each other.
    public void Awake()
    {
        instance = this; 
        this.GetComponent<StandaloneInputModule>().enabled = true;
    }

    //an id is used to ensure that the correct door opens/closes/is toggled. doors and door interactors (buttons, trigger areas, etc.) have ids. the interactors linked to specific doors should have matching ids.
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
