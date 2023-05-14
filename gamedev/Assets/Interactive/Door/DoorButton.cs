using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class DoorButton : MonoBehaviour, IInteractable
{
    
    [SerializeField] private int id;
    [SerializeField] private AudioSource buttonSound;
    public void Interact()
    {
        
        buttonSound.Play();
        
        
        GameEvents.instance.DoorwayToggle(id);
       
        
        UnityEngine.Debug.Log("Interacting with DoorButton");
        string callingFuncName = new StackFrame(1).GetMethod().Name;
        UnityEngine.Debug.Log(callingFuncName);
        //Debug.Log("this")

    }
}

