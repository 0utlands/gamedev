using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DoorController : MonoBehaviour, HasDefault
{
    public int id;
    [SerializeField] private Animator m_animatior;
    private bool m_isOpen = false;
    [SerializeField] private bool guardShouldMaintainState;
    [SerializeField] private bool openByDefault; //this is used when making the map to set the default state of the door, allowing guards to react to this state.
    [SerializeField] private List<GameObject> linkedButtons;
    // Start is called before the first frame update
    private void Start()
    {   //we are subscribing the doorControllers "onDoorwayOpen" method to the gameEvent's "onDoorwayTriggerEnter" event. When this event happens, OnDoorwayOpen is called.
        GameEvents.instance.onDoorwayTriggerEnter += OnDoorwayTriggerOpen;
        GameEvents.instance.onDoorwayTriggerExit += OnDoorwayClose;
        GameEvents.instance.onDoorwayToggle += OnDoorwayToggle;
        //m_doorOpenHash = Animator.StringToHash("OpenDoor");
        //m_doorCloseHash = Animator.StringToHash("CloseDoor");
    }
    
    public bool GetIfInDefaultState()
    {
        if (guardShouldMaintainState) 
        {
            //Debug.Log($"Is this object in its default state: {m_isOpen == openByDefault}");
            return m_isOpen == openByDefault;
        } else
        {
            return true;
        }
    }

    public List<GameObject> GetInteractors()
    {
        return linkedButtons;
    }

    //for opening and closing doorways using buttons.
    private void OnDoorwayToggle(int id)
    {
        
        if (id != this.id)
        {
            return;
        }
        if (!m_isOpen)
        {
            
            Open();
        } else
        {
            Close();
        }
        m_isOpen = !m_isOpen;
    }

    //For opening and closing doorways by walking into trigger areas
    private void OnDoorwayTriggerOpen(int id)
    {
        //print("close");
        if (id != this.id)
        {
            return;
        }
        if (!m_isOpen)
        {
            Open();
        }
        m_isOpen = !m_isOpen;
    }

    private void OnDoorwayClose(int id)
    {
        //print("dooropen");
        if (id != this.id)
        {
            return;
        }
        if (m_isOpen)
        {
            Close();
        }
        m_isOpen = !m_isOpen;
    }

    //to make a new door, with a new triggering mechanism - make sure the new doors' animator transition bool is named DoorIsOpen, and make sure anything triggering it acts in the same way as the doortrigger class.
    public void Open()
    {
        m_animatior.SetBool("DoorIsOpen", true);
    }

    public void Close()
    {
        m_animatior.SetBool("DoorIsOpen", false);
    }

    private void OnDestroy()
    {
        GameEvents.instance.onDoorwayTriggerEnter -= OnDoorwayTriggerOpen;
        GameEvents.instance.onDoorwayTriggerExit -= OnDoorwayClose;
    }
}
