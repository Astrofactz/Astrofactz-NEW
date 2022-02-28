using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDragBasePiece : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    private bool isBeingHeld;

    public float snapRadius;
    public float rotMultiplierX;
    public float rotMultiplierY;

    public GameObject basePoint;
    public GameObject baseSnapPoint;

    public LayerMask basePointLayer;

    private bool isBaseSet;
    private bool isSnapping;

    private RaycastHit hit;

    private Quaternion currentRotation;

    // Update is called once per frame
    void Update()
    {
        /*if (!isBaseSet)
            ObjectTransform();
        else
            ObjectRotate();*/

        ObjectRotate();
    }

    public void ObjectTransform()
    {
        if (isBeingHeld)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos.z = 16;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            if (Physics.SphereCast(SetCastPoint(), snapRadius, Vector3.down, out hit, snapRadius, basePointLayer))
            {
                gameObject.transform.localPosition = new Vector3(
                    Mathf.Round((mousePos.x - startPosX) + (hit.transform.position.x - Mathf.Round(hit.transform.position.x))),
                    Mathf.Round((mousePos.y - startPosY) + (hit.transform.position.y - Mathf.Round(hit.transform.position.y))),
                    0
                    );
                isSnapping = true;
            }
            else
                gameObject.transform.localPosition = new Vector3((mousePos.x) - startPosX, (mousePos.y) - startPosY, 0);
            isSnapping = false;
        }
        //basePoint.SetActive(isBeingHeld);
    }

    public Vector3 SetCastPoint()
    {
        Vector3 pos = new Vector3(
            baseSnapPoint.transform.position.x,
            baseSnapPoint.transform.position.y + snapRadius,
            baseSnapPoint.transform.position.z);
        return pos;
    }

    public void ObjectRotate()
    {
        if(isBeingHeld)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos.z = 16;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            currentRotation.eulerAngles = new Vector3(rotMultiplierY * (mousePos.y - startPosY) -90, rotMultiplierX * (mousePos.x - startPosX), 0);

            basePoint.transform.rotation = currentRotation;
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

            startPosX = mousePos.x - transform.localPosition.x;
            startPosY = mousePos.y - transform.localPosition.y;

            isBeingHeld = true;
            //Debug.Log(isBeingHeld);
        }
    }

    private void OnMouseUp()
    {
        isBeingHeld = false;

        /*if (isSnapping)
        {
            isBaseSet = true;
        }*/

    }

    public void SnapToMe(GameObject obj)
    {
        obj.transform.SetParent(gameObject.transform);
    }
}
