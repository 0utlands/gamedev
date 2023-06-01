using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound
{
    //sound class. used for storing the position and range of a sound. a new sound instance is made when a box drops on the floor.
    public Sound(Vector3 pos, float range)
    {
        this.pos = pos;
        this.range = range;
    }

    public Vector3 pos;
    public float range;
}
