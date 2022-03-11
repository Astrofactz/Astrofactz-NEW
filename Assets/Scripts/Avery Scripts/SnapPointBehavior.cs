using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPointBehavior : MonoBehaviour
{
    private GameObject[] snapPointArray;

    [HideInInspector]
    public List<GameObject> snapPointList = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        snapPointArray = GameObject.FindGameObjectsWithTag("SnapPoint");

        snapPointList.AddRange(snapPointArray);

        DisableSnapPoints();
    }

    /// <summary>
    /// 
    /// </summary>
    public void EnableSnapPoints()
    {
        foreach(GameObject snapPoint in snapPointList)
        {
            snapPoint.SetActive(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void DisableSnapPoints()
    {
        foreach (GameObject snapPoint in snapPointList)
        {
            snapPoint.SetActive(false);
        }
    }
}
