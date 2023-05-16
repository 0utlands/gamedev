using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class DoorButton : MonoBehaviour, IInteractable
{
    
    [SerializeField] private int id;
    [SerializeField] private AudioSource buttonSound;
    private GameObject DoorText;
    void Start() {
        DoorText = GameObject.FindObjectOfType<GameManager>().DoorText;
    }
    public void Interact()
    {
        
        buttonSound.Play();
        
        
        GameEvents.instance.DoorwayToggle(id);
       
        
        UnityEngine.Debug.Log("Interacting with DoorButton");
        string callingFuncName = new StackFrame(1).GetMethod().Name;
        UnityEngine.Debug.Log(callingFuncName);
        //Debug.Log("this")

    }
    void OnTriggerEnter(Collider other)
    {
        if (DoorText.activeInHierarchy == false)
        {
            DoorText.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (DoorText.activeInHierarchy == true)
        {
            DoorText.SetActive(false);
        }

    }
}

