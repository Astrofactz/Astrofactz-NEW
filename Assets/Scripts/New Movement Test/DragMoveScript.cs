using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragMoveScript : MonoBehaviour
{
    [HideInInspector]
    public GameObject selectedObject;

    private GameObject targetSnap;
    public float objectZ;

    //public LineRenderer line;

    public LayerMask snapLayer;

    private bool isBeingHeld;
    private bool shouldSnap;

    public SnapPointTest[] snapPoints;

    [HideInInspector]
    public Vector3 snapOffset;

    // Update is called once per frame
    void Update()
    {
        if (selectedObject != null)
        {
            Vector3 position = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 
                Camera.main.WorldToScreenPoint(selectedObject.transform.position).z);
            Vector3 worldPosition = Camera.main.ScreenToWorldPoint(position);

            selectedObject.transform.position = new Vector3(worldPosition.x, worldPosition.y, objectZ);
        }
        //line.enabled = shouldSnap;
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
        }
    }

    /// <summary>
    /// Called when the mouse button is released.
    /// </summary>
    private void OnMouseUp()
    {

        if (shouldSnap)
        {
            selectedObject.transform.position = targetSnap.transform.position - snapOffset;
        }

        foreach (SnapPointTest sp in snapPoints)
        {
            sp.line.enabled = false;

            if (!sp.shouldSnap)
            {
                continue;
            }
            selectedObject.transform.position = sp.targetSnap.transform.position - sp.snapOffset;
            sp.shouldSnap = false;
        }

        selectedObject = null;
        targetSnap = null;

        isBeingHeld = false;
        shouldSnap = false;
    }
}
