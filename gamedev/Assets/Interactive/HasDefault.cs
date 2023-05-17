using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface HasDefault
{

    
    //get if this map part is in its default state. if false, the guard who sees it should return it to its default state.
    public bool GetIfInDefaultState()
    {
        return true;
    }

    //should be overriden in doors / things with hasdefault, to return all the gameobjects which can be interacted with to change the state of the thing.
    public List<GameObject> GetInteractors()
    {
        return new List<GameObject>();
    }
}
