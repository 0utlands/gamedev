using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Transform cam;

    private void Awake()
    {
        cam = GameObject.FindObjectOfType<Camera>().transform;
    }

    //always called after the regular update
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
