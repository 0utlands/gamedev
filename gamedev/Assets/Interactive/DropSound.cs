using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class DropSound : MonoBehaviour
{
    //script appplied to objects that make a sound upon dropping.
    public AudioSource impactSound;
    //public AudioSource impactSounds2;
    //AudioSource[] impactSounds;
    public float soundRange = 25.0f;
    public bool canMakeSound = true;
    void OnCollisionEnter(Collision collision)
    {
        //when the object collides with the ground, make a sound.
        if (canMakeSound)
        {
            //if the object is moving fast enough, make a sound.
            if (collision.relativeVelocity.magnitude > 0.5)
            {
                



                //CODE TO PLAY A SOUND AND MAKE PEOPLE REACT TO IT STARTS HERE ....
                //dont play the sound twice if its already playing.
                if (impactSound.isPlaying)
                {
                    return;
                }
                impactSound.Play();

                //make a new sound instance, with the position of the gameobject this applied to.
                var sound = new Sound(transform.position, soundRange);

                //use the Sounds class to make the sound. this function calls all soundHearer's respondToSound functions within the sounds radius. (the guard's respondToSound fgunction is within their guardStateManager script)
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
