using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitch : MonoBehaviour
{
    //private Material original;
    //public Material selected;

    public GameObject highlight;
    public GameObject outline;

    private void Start()
    {
        //original = GetComponent<Renderer>().material;
        highlight.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //if((GetComponent<MouseDrag>().isBeingHeld == true))
        //{
        //    GetComponent<Renderer>().material = selected;
        //}
        //else
        //{
        //    GetComponent<Renderer>().material = original;
        //}
    }

    private void OnMouseDrag()
    {
        //GetComponent<Renderer>().material = selected;
        highlight.SetActive(true);
        outline.SetActive(false);
    }

    private void OnMouseOver()
    {
        highlight.SetActive(true);
        outline.SetActive(false);
    }

    private void OnMouseExit()
    {
        highlight.SetActive(false);
        outline.SetActive(true);
    }

    private void OnMouseUp()
    {
        //GetComponent<Renderer>().material = original;
        highlight.SetActive(false);
        outline.SetActive(true);
    }

}
