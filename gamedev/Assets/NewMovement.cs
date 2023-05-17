using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class NewMovement : MonoBehaviour
{

    private PlayerInput controls;
    private Vector3 moveVec = Vector3.zero;//this is a vec3 because we are moving in the x and z axis
    private CharacterController charController;
    [SerializeField] private float movementSpeed = 0.05f;
    [SerializeField] private float sprintSpeed = 0.1f;
    public bool hasItemOnHead = false;
    public float rotationSpeed;

    public GameObject objectOnHead;
    [SerializeField] private float interactionRange;
   
    private bool isSprinting = false;
    [SerializeField] private float maxStamina = 100f;
    private float stamina;
    [SerializeField] private float staminaReductionRate = 1.0f;
    [SerializeField] private float staminaRegenRate = 0.25f;
    public StaminaBar staminaBar;

    private void Awake()
    {
        controls = new PlayerInput();
        //controls.Player.Interact.performed += _ => Interact();
        charController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        stamina = maxStamina;
        staminaBar.setMaxStamina(stamina);
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
        controls.Player.Exit.performed += OnExitPerformed;
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
        if (isSprinting && moveVec != Vector3.zero)
        {
            if (stamina <= 0)
            {
                //player is trying to sprint, but has no stamina left
                //- move the character at walking speed
                //- keep sprint meter at zero
                stamina = 0;
                charController.Move(moveVec * movementSpeed);
            } else
            {
                //Player is sprinting:
                //- remove one from sprint meter every fixed update
                //- move player at sprint speed
                charController.Move(moveVec * sprintSpeed);
                stamina -= staminaReductionRate;
            }

        } else
        {
            //player is walking
            //- move them at walking speed
            //regen their stamina.
            charController.Move(moveVec * movementSpeed);
            if (stamina < 100) 
            {
                stamina += staminaRegenRate;
            }
            
        }
        staminaBar.setStamina(stamina);
        //print("Stamina: " + stamina);

        

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


        //Debug.Log("Interaction time");
        
        Collider[] col = Physics.OverlapSphere(this.transform.position, interactionRange);

        GameObject[] activeTooltips = GameObject.FindGameObjectsWithTag("Tooltip");
        foreach (GameObject activeTooltip in activeTooltips)
        {
            activeTooltip.SetActive(false);
            //Debug.Log("hereeeeee");
            //break;
        }
        foreach (Collider c in col)
        {
            if (c.TryGetComponent(out Tooltip tooltip)){

                if (tooltip.TooltipText.activeInHierarchy == true)
                {
                    tooltip.showToolTip();
                    return;
                }
                else {
                    tooltip.hideToolTip();
                    return;
                }
            }
        }

        //Debug.Log("HELOTHERE");

        Debug.Log("Object on head: " + objectOnHead);

        if (objectOnHead != null)
        {
            Debug.Log("Object on head is not null");
            if (objectOnHead.TryGetComponent(out IInteractable interactor))
            {
                Debug.Log("Object on head has an interactor");
                interactor.Interact();
                objectOnHead = null;
            }
        }
        else
        {

            for (int i = 0; i < col.Length; i++)
            {
                Debug.Log(col[i]);
                if (col[i].TryGetComponent(out IInteractable interactor))
                {
                    Debug.Log("Interactor responding to interaction");

                    if (col[i].isTrigger == true)
                    {
                        Debug.Log("Interactor is not a trigger");
                        continue;
                    }

                    interactor.Interact();
                    interactor = null;

                    //if the object can be put on the players head
                    if (col[i].TryGetComponent(out Interactable interactable))
                    {
                        objectOnHead = col[i].gameObject;
                    }
                    //return;
                }
                
            }
        }
        //Debug.Log("Done interacting");
    }

    private void OnSprintStartPerformed(InputAction.CallbackContext value)
    {
        //Debug.Log("Sprint time");
        isSprinting = true;
    }

    private void OnSprintEndPerformed(InputAction.CallbackContext value)
    {
        //Debug.Log("Sprint stop time");
        isSprinting = false;
    }

    void Interact()
    {
        //Debug.Log("Interacting");
    }

    private void OnExitPerformed(InputAction.CallbackContext value)
    {
        //Application.Quit();
        if (SceneManager.GetActiveScene().name != "Menu") {
            GameObject.Find("GameManager").GetComponent<GameManager>().LeaveLevel();
        }
    }
}
