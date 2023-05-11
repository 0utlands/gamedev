using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class NewMovement : MonoBehaviour
{

    private PlayerInput controls;
    private Vector3 moveVec = Vector3.zero;//this is a vec3 because we are moving in the x and z axis
    private CharacterController charController;
    [SerializeField] private float movementSpeed = 0.05f;
    [SerializeField] private float sprintSpeed = 0.1f;
    public bool hasItemOnHead = false;
    private bool isSprinting = false;
    public float rotationSpeed;

    [SerializeField] private float interactionRange;

    private void Awake()
    {
        controls = new PlayerInput();
        //controls.Player.Interact.performed += _ => Interact();
        charController = GetComponent<CharacterController>();
    }

  

    //called when object becomes enabled and active
    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += OnMovementPerformed;
        controls.Player.Move.canceled += OnMovementCancelled;
        controls.Player.Interact.performed += OnInteractionPerformed;
        controls.Player.SprintStart.performed += OnSprintStartPerformed;
        controls.Player.SprintEnd.performed += OnSprintEndPerformed;
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Move.performed -= OnMovementPerformed;
        controls.Player.Move.canceled -= OnMovementCancelled;
    }

    private void FixedUpdate()
    {
        //Debug.Log(moveVec);
        //move the character in the direction specified by WASD
        if (isSprinting)
        {
            charController.Move(moveVec * sprintSpeed);
        } else
        {
            charController.Move(moveVec * movementSpeed);
        }

        

        //if we are moving
        if(moveVec != Vector3.zero)
        {
            //ROTATING THE CHARACTER TO LOOK WHERE THEY ARE GOING
            
            //Quaternion is a type of variable specifically for storing rotations. Lookrotation creates a rotation looking in a desired direction, in this case moveVec.
            //make a rotation define by the direction we are moving
            Quaternion toRotation = Quaternion.LookRotation(moveVec, Vector3.up);

            //RoatteTowards rortates from current rotation to the desired direction.
            //rotate from where character currently is, to the direction theyre moving at, at the rotation speed
            transform.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
        }

    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        //we used a vector2 in our action map so we read a vector2 here
        //SOMETHING LIKE THIS
        //get the values of te vec2 showing which directionnwe're moving using WASD from the input system - we are specifiying 
        moveVec.x = value.ReadValue<Vector2>().x;
        moveVec.z = value.ReadValue<Vector2>().y;
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVec = Vector3.zero;
    }

    private void OnInteractionPerformed(InputAction.CallbackContext value)
    {
        Debug.Log("Interaction time");
        Collider[] col = Physics.OverlapSphere(this.transform.position, interactionRange);

        for (int i = 0; i < col.Length; i++)
        {
            //Debug.Log(col[i]);
            if (col[i].TryGetComponent(out IInteractable interactor))
            {
                Debug.Log("Interactor responding to interaction");
                interactor.Interact();
            }
        }
    }

    private void OnSprintStartPerformed(InputAction.CallbackContext value)
    {
        Debug.Log("Sprint time");
        isSprinting = true;
    }

    private void OnSprintEndPerformed(InputAction.CallbackContext value)
    {
        Debug.Log("Sprint stop time");
        isSprinting = false;
    }

    void Interact()
    {
        Debug.Log("Interacting");
    }
}
