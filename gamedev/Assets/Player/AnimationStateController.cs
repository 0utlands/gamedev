using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    private PlayerInput controls;
    private Vector3 moveVec = Vector3.zero;//this is a vec3 because we are moving in the x and z axis

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        //Debug.Log(animator);
    }

    private void Awake()
    {
        controls = new PlayerInput();
        //controls.Player.Interact.performed += _ => Interact();
    }

    //called when object becomes enabled and active
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

    // Update is called once per frame
    void Update()
    {
        //should change this to actually link to the input system in future versions
        //bool moving = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d");
        bool isWalking = animator.GetBool("IsWalking");
        if (!isWalking && moveVec.magnitude > 0)
        {
            animator.SetBool("IsWalking", true);
            //Debug.Log("W pressed in animatorstatecontroller");
        }

        if (isWalking && moveVec.magnitude == 0)
        {
            animator.SetBool("IsWalking", false);
            
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
}
