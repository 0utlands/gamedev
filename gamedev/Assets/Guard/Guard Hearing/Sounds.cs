using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class Sounds
{
    //used for maing a sound at a position. this is called when a box is dropped on the floor.
    public static void MakeSound(Sound sound)
    {
        //any colliders within the overlapsphere which implement soundHearer will have their respondToSound fucntion called.
        Collider[] col = Physics.OverlapSphere(sound.pos, sound.range);

        for (int i = 0; i < col.Length; i++)
        {
            //Debug.Log(col[i]);
            if (col[i].TryGetComponent(out SoundHearer hearer))
            {
                Debug.Log("Hearer responding to sound...");
                hearer.RespondToSound(sound);
            }
        }

    }
}
