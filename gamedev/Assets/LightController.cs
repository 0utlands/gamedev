using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightController : MonoBehaviour
{
    public int id;
    private bool m_isOpen = false;
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
            TurnOn();
        }
        else
        {
            TurnOff();
        }
        m_isOpen = !m_isOpen;
    }

    //For opening and closing doorways by walking into trigger areas
    private void OnDoorwayTriggerOpen(int id)
    {
        print("close");
        if (id != this.id)
        {
            return;
        }
        if (!m_isOpen)
        {
            TurnOn();
        }
        m_isOpen = !m_isOpen;
    }

    private void OnDoorwayClose(int id)
    {
        print("dooropen");
        if (id != this.id)
        {
            return;
        }
        if (m_isOpen)
        {
            TurnOff();
        }
        m_isOpen = !m_isOpen;
    }

    //to make a new door, with a new triggering mechanism - make sure the new doors' animator transition bool is named DoorIsOpen, and make sure anything triggering it acts in the same way as the doortrigger class.
    public void TurnOn()
    {
        GetComponent<Light>().intensity = 1;
    }

    public void TurnOff()
    {
        GetComponent<Light>().intensity = 0;
    }

    private void OnDestroy()
    {
        GameEvents.instance.onDoorwayTriggerEnter -= OnDoorwayTriggerOpen;
        GameEvents.instance.onDoorwayTriggerExit -= OnDoorwayClose;
    }
}
