using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public static class Sounds
{
    public static void MakeSound(Sound sound)
    {
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
