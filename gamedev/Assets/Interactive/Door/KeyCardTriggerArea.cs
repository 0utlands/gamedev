using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardTriggerArea : MonoBehaviour
{
    public int id;
    [SerializeField] private AudioSource openSound;
    [SerializeField] private AudioSource closeSound;
    [SerializeField] private string keyCardTag = "KeyCard";

    //if the keycard with the correct tag enters the area, open the door. (using gameEvents)
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == keyCardTag)
        {
            print("Keycard found");
            GameEvents.instance.DoorwayOpen(id);
            openSound.Play();
        }
        
        
    }

    //if the keycard with the correct tag leaves the area, close the door.(using gameEvents)
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == keyCardTag)
        {
            print("Keycard found");
            print("Keycard gone");
            GameEvents.instance.DoorwayClose(id);
            closeSound.Play();
        }
    }
}
