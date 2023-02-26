using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewMovement : MonoBehaviour
{

    private PlayerInput controls;
    private Vector3 moveVec = Vector3.zero;
    private CharacterController charController;
    private float movementSpeed = 0.1f;

    private void Awake()
    {
        controls = new PlayerInput();
        //controls.Player.Interact.performed += _ => Interact();
        charController = GetComponent<CharacterController>();
    }

  

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Move.performed += OnMovementPerformed;
        controls.Player.Move.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        controls.Disable();
        controls.Player.Move.performed -= OnMovementPerformed;
        controls.Player.Move.canceled -= OnMovementCancelled;
    }

    private void FixedUpdate()
    {
        Debug.Log(moveVec);
        charController.Move(moveVec * movementSpeed);
    }

    private void OnMovementPerformed(InputAction.CallbackContext value)
    {
        //we used a vector2 in our action map so we read a vector2 here
        //SOMETHING LIKE THIS
        moveVec.x = value.ReadValue<Vector2>().x;
        moveVec.z = value.ReadValue<Vector2>().y;
    }

    private void OnMovementCancelled(InputAction.CallbackContext value)
    {
        moveVec = Vector3.zero;
    }

    void Interact()
    {
        Debug.Log("Interacting");
    }
}
