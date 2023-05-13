using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class DropSound : MonoBehaviour
{
    public AudioSource impactSound;
    //public AudioSource impactSounds2;
    //AudioSource[] impactSounds;
    public float soundRange = 25.0f;
    public bool canMakeSound = true;
    void OnCollisionEnter(Collision collision)
    {
        if (canMakeSound)
        {
            if (collision.relativeVelocity.magnitude > 0.5)
            {
                //int num = UnityEngine.Random.Range(0, impactSounds.Length);
                //impactSounds[num].Play();



                //CODE TO PLAY A SOUND AND MAKE PEOPLE REACT TO IT STARTS HERE ....
                if (impactSound.isPlaying)
                {
                    return;
                }
                impactSound.Play();
                var sound = new Sound(transform.position, soundRange);
                Sounds.MakeSound(sound);
                print($"Sound: with sound pos {sound.pos} and range {sound.range}");
                //and ends here...


            }

            else
            {
                //print("No sound :(");
            }
        }
    }
}
