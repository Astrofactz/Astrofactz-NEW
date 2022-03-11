using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPointTest : MonoBehaviour
{
    public GameObject correctSnap;
    public GameObject parentObject;
    public float snapRadius;
    public LayerMask snapLayer;

    [HideInInspector]
    public Vector3 snapOffset;
    [HideInInspector]
    public GameObject targetSnap;

    [HideInInspector]
    public bool shouldSnap;
    [HideInInspector]
    public bool hasTarget;

    public LineRenderer line;

    private DragMoveScript dm;

    private void Awake()
    {
        dm = gameObject.GetComponentInParent<DragMoveScript>();
    }

    private void FixedUpdate()
    {
        if (dm.selectedObject != null)
            MovePiece();
    }

    private void MovePiece()
    {
        Ray snapRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit snapHit;

        if(!hasTarget)
        {
            if (Physics.SphereCast(SetCastPoint(gameObject), snapRadius, Vector3.back, out snapHit, snapRadius, snapLayer) && !gameObject.CompareTag(snapHit.collider.tag))
            {
                targetSnap = snapHit.collider.gameObject;
                hasTarget = true;
            }
        }

        if (Physics.CheckSphere(transform.position, snapRadius, snapLayer) && hasTarget)
        {

            snapOffset = gameObject.transform.position - parentObject.transform.position;

            if (snapRadius >= Vector3.Distance(transform.position, targetSnap.transform.position))
            {
                if (targetSnap == correctSnap)
                {
                    line.startColor = Color.green;
                    line.endColor = Color.green;
                    shouldSnap = true;
                }
                else
                {
                    line.startColor = Color.red;
                    line.endColor = Color.red;
                    shouldSnap = false;
                }

                line.SetPosition(0, gameObject.transform.position);
                line.SetPosition(1, targetSnap.transform.position);
                line.enabled = true;
            }
        }
        else if(!hasTarget || snapRadius < Vector3.Distance(transform.position, targetSnap.transform.position))
        {
            line.enabled = false;
            shouldSnap = false;
            hasTarget = false;
            targetSnap = null;
        }
    }

    public Vector3 SetCastPoint(GameObject snapPoint)
    {
        Vector3 pos = new Vector3(
            snapPoint.transform.position.x,
            snapPoint.transform.position.y,
            snapPoint.transform.position.z + snapRadius);
        return pos;
    }

    void OnDrawGizmosSelected()
    {
        // Display the radius of the snap
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(gameObject.transform.position, snapRadius);
    }

}
