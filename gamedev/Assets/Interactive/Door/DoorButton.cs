using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorButton : MonoBehaviour, IInteractable
{
    
    [SerializeField] private int id;
    [SerializeField] private AudioSource buttonSound;
    public void Interact()
    {
        
        buttonSound.Play();
        
        
        GameEvents.instance.DoorwayToggle(id);
       
        
        Debug.Log("Interacting with DoorButton");
        
    }
}

