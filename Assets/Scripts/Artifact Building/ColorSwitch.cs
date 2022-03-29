using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitch : MonoBehaviour
{
    private Material original;
    public Material selected;

    private void Start()
    {
        original = GetComponent<Renderer>().material;
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
        GetComponent<Renderer>().material = selected;
    }

    private void OnMouseUp()
    {
        GetComponent<Renderer>().material = original;
    }

}
