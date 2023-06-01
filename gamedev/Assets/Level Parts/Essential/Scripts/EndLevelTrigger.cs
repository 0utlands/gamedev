using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevelTrigger : MonoBehaviour
{
    public GameManager gameManager;

    //used or working out if the level is complete.

    public bool playerInside = false;
    public bool itemToStealInside;

    //if the player and the final item to steal are in this collider (located at the end of every level) at the same time, the level completes.
    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player" && playerInside == false)
        {
            playerInside = true;
            Debug.Log("Player in the area");
        }

        if(other.tag == "ToSteal" && itemToStealInside == false)
        {
            itemToStealInside = true;
            Debug.Log("Obj in the area");
        }
        if (itemToStealInside && playerInside)
        {
            gameManager.CompleteLevel();
        }
        
    }

    private void OnTriggerExit(Collider other)
    {

        if (other.tag == "Player" && playerInside == true)
        {
            playerInside = false;
        }

        if (other.tag == "ToSteal" && itemToStealInside == true)
        {
            itemToStealInside = false;
        }

    }
}
