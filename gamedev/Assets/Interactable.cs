using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public KeyCode interactKey = KeyCode.E;
    private bool isItemOnHead = false;
    private Vector3 playerPos;
    public float height = 2.0f;
    public float distanceCutoff = 0.8f;
    [SerializeField] private Vector3 optionalRotation = Vector3.zero;
    GameObject player;

    private bool interactedWith = false;


    //debugging variables
    private float frame = 0;
    private float maxFrames = 24;
    private bool printObjectVelocity = false;


    // Start is called before the first frame update
    void Start()
    {
    }

    public void Interact()
    {
        Debug.Log("Interracting with box!");
        if (isItemOnHead)
        {
            Debug.Log("Dropping item");
            dropItem();
        }
        else
        {
            Debug.Log("putting item on head");
            if (Vector3.Distance(player.GetComponent<Transform>().position, transform.position) < distanceCutoff)
            {
                if (!player.GetComponent<NewMovement>().hasItemOnHead)
                {
                    putItemOnHead();
                }
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        if (isItemOnHead) {
            playerPos = player.GetComponent<Transform>().position;
            transform.position = new Vector3(playerPos.x, playerPos.y + height, playerPos.z);
            transform.forward = player.GetComponent<Transform>().forward;
           
        };

        //code for debugging - why are boxes pinging out of the map?
        if (frame < maxFrames && printObjectVelocity == true) 
        { 
            Debug.Log("Obj velocity after dropping: " + GetComponent<Rigidbody>().velocity);
            frame += 1;
        }

        if (frame >= maxFrames && printObjectVelocity == true)
        {
            printObjectVelocity = false;
            frame = 0;
        }
        


        /*if (Input.GetKeyDown(interactKey) || interactedWith) {
            if (isItemOnHead)
            {
                dropItem();
            }
            else{
                if (Vector3.Distance(player.GetComponent<Transform>().position, transform.position) < distanceCutoff) {
                    if (!player.GetComponent<NewMovement>().hasItemOnHead) {
                        putItemOnHead();
                    }
                }
            }
        }*/
    }

    public void putItemOnHead() {
        isItemOnHead = true;
        player.GetComponent<NewMovement>().hasItemOnHead = true;
        //GetComponent<BoxCollider>().enabled = false;
        if (this.TryGetComponent(out DropSound ds))
        {

            ds.canMakeSound = false;

        }
    }

    public void dropItem() {
        isItemOnHead = false;
        player.GetComponent<NewMovement>().hasItemOnHead = false;
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f,0.0f,0.0f);
        if (this.TryGetComponent(out DropSound ds))
        {
            
            ds.canMakeSound = true;
            
        }

        //can remove when debugging is done
        printObjectVelocity = true;



        //GetComponent<BoxCollider>().enabled = true;
    }
}
