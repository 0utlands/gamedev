using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GuardBaseState
{
    public abstract void enterState(GuardStateManager guard);

    public abstract void updateState(GuardStateManager guard);

    
}
