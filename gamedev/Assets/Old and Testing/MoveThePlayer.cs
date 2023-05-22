using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveThePlayer : MonoBehaviour
{
    public Vector3 updatedPlayerPos;
    public float movementSpeed = 0.3f;
    private CharacterController charController;

    private void Start()
    {
        charController = GetComponent<CharacterController>();
    }

    //Might want this in fixedupdate
    // Update is called once per frame
    void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.W))
         {
             updatedPlayerPos = transform.position;
             updatedPlayerPos.z += movementSpeed;
             transform.position = updatedPlayerPos;

             //transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + movementSpeed);

             charController.Move(new Vector3(transform.position.x, transform.position.y, transform.position.z + movementSpeed));
         }*/
        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        Debug.Log(Input.GetAxis("Horizontal"));
        //charController.Move(move * Time.deltaTime * movementSpeed);
        charController.Move(move * movementSpeed);

    }
}
