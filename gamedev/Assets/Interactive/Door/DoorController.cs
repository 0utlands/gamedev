using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public int id;
    [SerializeField] private Animator m_animatior;
    private bool m_isOpen = false;
    private int m_doorOpenHash;
    private int m_doorCloseHash;
    // Start is called before the first frame update
    private void Start()
    {   //we are subscribing the doorControllers "onDoorwayOpen" method to the gameEvent's "onDoorwayTriggerEnter" event. When this event happens, OnDoorwayOpen is called.
        GameEvents.instance.onDoorwayTriggerEnter += OnDoorwayTriggerOpen;
        GameEvents.instance.onDoorwayTriggerExit += OnDoorwayClose;
        GameEvents.instance.onDoorwayToggle += OnDoorwayToggle;
        //m_doorOpenHash = Animator.StringToHash("OpenDoor");
        //m_doorCloseHash = Animator.StringToHash("CloseDoor");
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
