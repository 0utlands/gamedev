using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents instance;

    public void Awake()
    {
        instance = this; 
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
