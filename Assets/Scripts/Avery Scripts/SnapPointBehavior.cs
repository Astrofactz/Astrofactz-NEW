using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPointBehavior : MonoBehaviour
{
    public GameObject correctSnapPoint;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DestroySnapPoints()
    {
        Destroy(correctSnapPoint);
        Destroy(gameObject);
    }
}
