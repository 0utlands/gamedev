using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonSelecter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        EventSystem eventSystem = GameObject.Find("EventSystem").GetComponent<EventSystem>();
        Button[] buttons = GameObject.FindObjectsOfType<Button>();
        foreach(Button b in buttons)
        {
            eventSystem.SetSelectedGameObject(b.GetComponent<Button>().gameObject);
            return;

        }
    }


}
