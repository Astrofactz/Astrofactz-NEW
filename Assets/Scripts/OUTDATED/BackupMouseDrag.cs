using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackupMouseDrag : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    private bool isBeingHeld;

    // Update is called once per frame
    void Update()
    {
        if (isBeingHeld)
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos.z = 16;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            this.gameObject.transform.localPosition = new Vector3((mousePos.x) - startPosX, (mousePos.y) - startPosY, -3);
            //Debug.Log(mousePos);
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
        //Debug.Log(isBeingHeld);
    }
}
