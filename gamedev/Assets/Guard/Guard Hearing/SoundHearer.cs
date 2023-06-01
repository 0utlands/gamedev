using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface SoundHearer
{
    //interface for things which can respond to sound. it is implemented for guards in thei guardStateManager. this function is called if Sounds.MakeSound finds anything implenmenting this within the sounds range. Sounds.MakeSound is called within dropSound (in assets/interactive) when a box drops on the floor.
    void RespondToSound(Sound sound)
    {

    }
}
