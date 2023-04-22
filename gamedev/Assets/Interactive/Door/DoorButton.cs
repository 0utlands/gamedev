using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour, IInteractable
{
    
    [SerializeField] private int id;
    public void Interact()
    {
        
        
        GameEvents.instance.DoorwayToggle(id);
       
        
        Debug.Log("Interacting with DoorButton");
        
    }
}

