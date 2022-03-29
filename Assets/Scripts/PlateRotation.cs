using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlateRotation : MonoBehaviour
{
    private float startPosX;
    private bool isBeingHeld;

    public float rotMultiplierX;
    public float rotMultiplierY;

    public List<GameObject> piecesInMenu;
    public List<GameObject> piecesInSpace;

    private Quaternion currentRotation;

    

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingHeld)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos.z = 16;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            currentRotation.eulerAngles = new Vector3(0, -rotMultiplierX * (mousePos.x - startPosX), 0);

            foreach (GameObject obj in piecesInSpace)
            {
                obj.transform.parent = gameObject.transform;
            }

            transform.localRotation = currentRotation;

            foreach(GameObject obj in piecesInMenu)
            {
                obj.transform.localRotation = currentRotation;
            }
            
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos.z = 16;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            startPosX = mousePos.x - transform.localRotation.y;
            Debug.Log(startPosX);

            isBeingHeld = true;
        }
    }

    private void OnMouseUp()
    {
        isBeingHeld = false;

        currentRotation = transform.rotation;
    }
}
