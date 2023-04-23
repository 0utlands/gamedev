using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    private PlayerInput controls;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        Debug.Log(animator);
    }

    // Update is called once per frame
    void Update()
    {
        //should change this to actually link to the input system in future versions
        bool moving = Input.GetKey("w") || Input.GetKey("a") || Input.GetKey("s") || Input.GetKey("d");
        bool isWalking = animator.GetBool("IsWalking");
        if (!isWalking && moving)
        {
            animator.SetBool("IsWalking", true);
            //Debug.Log("W pressed in animatorstatecontroller");
        }

        if (isWalking && !moving)
        {
            animator.SetBool("IsWalking", false);
            
        }
    }
}
