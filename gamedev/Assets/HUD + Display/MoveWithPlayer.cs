using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveWithPlayer : MonoBehaviour
{
    public Transform player;
    public Vector3 positionRelativeToPlayer;


    // Update is called once per frame
    void Update()
    {
        transform.position = player.position + positionRelativeToPlayer;
    }
}
