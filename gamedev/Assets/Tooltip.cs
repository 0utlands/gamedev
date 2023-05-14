using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tooltip : MonoBehaviour
{
    public Transform cam;
    private Vector3 startPosition;
    private float inc;
    private GameObject TooltipText;
    public GameObject TooltipTip;
    private bool closeButton;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        inc = 0.0f;
        TooltipText = GameObject.FindObjectOfType<GameManager>().TooltipText;
        TooltipTip.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        inc += 0.04f;
        transform.LookAt(transform.position + cam.forward);
        transform.position = new Vector3(transform.position.x, transform.position.y, startPosition.z + (float)System.Math.Sin(inc)/4);

    }

    public void showToolTip()
    {
        TooltipTip.SetActive(true);

    }
    public void hideToolTip()
    {
        TooltipTip.SetActive(false);
    }

    public void closed(bool close) {
        closeButton = close;
    }

    void OnTriggerEnter(Collider other)
    {
        closeButton = false;
        if (TooltipText.activeInHierarchy == false)
        {
            TooltipText.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (TooltipTip.activeInHierarchy == true) {
            TooltipText.SetActive(false);
        }
        if (closeButton == true) {
            TooltipText.SetActive(false);
        }
        /*if (TooltipText.activeInHierarchy == false && TooltipTip.activeInHierarchy == false)
        {
            TooltipText.SetActive(true);
        }*/
    }

    private void OnTriggerExit(Collider other)
    {
        if (TooltipText.activeInHierarchy == true)
        {
            TooltipText.SetActive(false);
            TooltipTip.SetActive(false);
        }
        
    }
}
