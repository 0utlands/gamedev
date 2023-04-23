using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{

    public Transform cam;
    
    //always called after the regular update
    void LateUpdate()
    {
        transform.LookAt(transform.position + cam.forward);
    }
}
