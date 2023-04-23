using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TestClicker : MonoBehaviour
{
    [SerializeField] public DoorButton m_interactable;
    
    private void OnMouseDown()
    {
        m_interactable.Interact();
    }

   
}
