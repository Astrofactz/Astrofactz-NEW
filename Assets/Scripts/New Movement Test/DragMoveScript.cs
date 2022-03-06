using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMoveScript : MonoBehaviour
{
    private GameObject selectedObject;
    private GameObject targetSnap;
    public float objectZ;
    public float snapRadius;

    public LayerMask snapLayer;

    private bool isBeingHeld;

    public GameObject[] snapPoints;

    // Update is called once per frame
    void Update()
    {
        if (selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 
                Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            foreach(GameObject obj in snapPoints)
            {
                RaycastHit snapHit;
                if(Physics.SphereCast(SetCastPoint(obj), snapRadius, Vector3.down, out snapHit, snapRadius, snapLayer))
                {
                    targetSnap = snapHit.collider.gameObject;

                    if(selectedObject.CompareTag(targetSnap.tag))
                    {
                        return;
                    }

                    selectedObject.transform.position = new Vector3(
                        targetSnap.transform.position.x - Mathf.Round(obj.transform.position.x - worldPosition.x),
                        targetSnap.transform.position.y - Mathf.Round(obj.transform.position.y - worldPosition.y),
                        objectZ);
                }
                else
                    selectedObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, objectZ);
            }
        }
    }

    private RaycastHit CastRay()
    {
        Vector3 screenMousePosFar = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.farClipPlane);
        Vector3 screenMousePosNear = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            Camera.main.nearClipPlane);
        Vector3 worldMousePosFar = Camera.main.ScreenToWorldPoint(screenMousePosFar);
        Vector3 worldMousePosNear = Camera.main.ScreenToWorldPoint(screenMousePosNear);
        RaycastHit hit;
        Physics.Raycast(worldMousePosNear, worldMousePosFar - worldMousePosNear, out hit);

        return hit;
    }

    /// <summary>
    /// Called when the mouse button is pressed.
    /// </summary>
    private void OnMouseDown()
    {
        if (Input.GetMouseButton(0))
        {
            if (selectedObject == null)
            {
                RaycastHit hit = CastRay();

                if (!hit.collider.tag.StartsWith("Drag"))
                {
                    return;
                }

                selectedObject = hit.collider.gameObject;

            }
            /*else
            {
                Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
                Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
                selectedObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, objectZ);

                selectedObject = null;
            }*/
        }
    }

    /// <summary>
    /// Called when the mouse button is released.
    /// </summary>
    private void OnMouseUp()
    {
        /*Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);
        selectedObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, objectZ);*/

        selectedObject = null;
        targetSnap = null;

        isBeingHeld = false;
    }

    public Vector3 SetCastPoint(GameObject snapPoint)
    {
        Vector3 pos = new Vector3(
            snapPoint.transform.position.x,
            snapPoint.transform.position.y + snapRadius,
            snapPoint.transform.position.z);
        return pos;
    }
}
