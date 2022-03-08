using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorSwitch : MonoBehaviour
{
    public Material original;
    public Material selected;
    
    // Update is called once per frame
    void Update()
    {
        if((GetComponent<MouseDrag>().isBeingHeld == true))
        {
            GetComponent<Renderer>().material = selected;
        }
        else
        {
            GetComponent<Renderer>().material = original;
        }
    }
}
