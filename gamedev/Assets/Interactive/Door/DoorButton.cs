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
        //if interacted with, play a sound and toggle the door.
        buttonSound.Play();
        GameEvents.instance.DoorwayToggle(id);
       
        
        UnityEngine.Debug.Log("Interacting with DoorButton");
        string callingFuncName = new StackFrame(1).GetMethod().Name;
        UnityEngine.Debug.Log(callingFuncName);
        //Debug.Log("this")

    }
    void OnTriggerEnter(Collider other)
    {
        //if the player comes near the button, display text prompting them to press it.
        if (DoorText.activeInHierarchy == false && other.tag == "Player")
        {
            DoorText.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }

    private void OnTriggerExit(Collider other)
    {
        if (DoorText.activeInHierarchy == true && other.tag == "Player")
        {
            DoorText.SetActive(false);
        }

    }
}

