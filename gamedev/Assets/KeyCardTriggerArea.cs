using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCardTriggerArea : MonoBehaviour
{
    public int id;
    [SerializeField] private AudioSource openSound;
    [SerializeField] private AudioSource closeSound;
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "KeyCard")
        {
            print("Keycard found");
            GameEvents.instance.DoorwayOpen(id);
            openSound.Play();
        }
        
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "KeyCard")
        {
            print("Keycard found");
            print("Keycard gone");
            GameEvents.instance.DoorwayClose(id);
            closeSound.Play();
        }
    }
}
