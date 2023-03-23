using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public AudioSource impactSound;
    //public AudioSource impactSounds2;
    //AudioSource[] impactSounds;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.relativeVelocity.magnitude > 0.1)
        {
            //int num = UnityEngine.Random.Range(0, impactSounds.Length);
            //impactSounds[num].Play();
            impactSound.Play();
        }
    }
}
