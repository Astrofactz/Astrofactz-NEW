using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public class SnapPoint
{
    /* 0 = hilt
     * 1 = guard
     * 2 = blade 1
     * 3 = blade 2
     */
    public int objNum;

    //how many objects can snap to a single point.
    public int numSnapPoints;

    public GameObject snap;

    //how many objects are currently snapped to a point.
    int currentSnaps;

}
