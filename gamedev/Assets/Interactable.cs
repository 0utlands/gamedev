using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public bool isInRange;
    public KeyCode interactKey;
    private bool isItemOnHead = false;
    private Vector3 playerPos;
    public float height = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (isItemOnHead) {
            playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
            transform.position = new Vector3(playerPos.x, playerPos.y + height, playerPos.z);
            transform.forward = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().forward;
        }
        if (isInRange) {

            if (Input.GetKeyDown(interactKey) && isItemOnHead == false)
            {
                putItemOnHead();
            }
            else if (Input.GetKeyDown(interactKey) && isItemOnHead == true) {
                dropItem();
            }
        }
    }

    public void putItemOnHead() {
        isItemOnHead = true;
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position;
        transform.position = new Vector3(playerPos.x, playerPos.y + height, playerPos.z);
        transform.forward = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().forward;
        GetComponent<BoxCollider>().enabled = false;
    }

    public void dropItem() {
        isItemOnHead = false;
        GetComponent<Rigidbody>().velocity = new Vector3(0.0f,0.0f,0.0f);
        GetComponent<BoxCollider>().enabled = true;
    }
}
