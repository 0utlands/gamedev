using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public Transform player;
    public Vector3 positionRelativeToPlayer;
    

    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + positionRelativeToPlayer;
        transform.rotation = player.rotation;
    }
}
