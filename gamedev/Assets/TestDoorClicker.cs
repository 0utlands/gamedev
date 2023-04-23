using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class TestDoorClicker : MonoBehaviour
{
    [SerializeField] private Animator m_animatior;
    private bool m_isOpen = false;
    private int m_doorOpenHash;
    private int m_doorCloseHash;
    // Start is called before the first frame update
    void Start()
    {
        m_doorOpenHash = Animator.StringToHash("OpenDoor");
        m_doorCloseHash = Animator.StringToHash("CloseDoor");
    }

    private void OnMouseDown()
    {
        if (m_isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
        m_isOpen = !m_isOpen;
    }

    public void Open()
    {
        m_animatior.Play(m_doorOpenHash);
    }

    public void Close()
    {
        m_animatior.Play(m_doorCloseHash);
    }
}
