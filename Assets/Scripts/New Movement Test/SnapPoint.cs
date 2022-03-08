using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnapPoint : MonoBehaviour
{
    public GameObject correctSnap;
    public GameObject parentObject;
    public float snapRadius;
    public LayerMask snapLayer;

    public Vector3 snapOffset;
    public GameObject targetSnap;
    public bool shouldSnap;

    public LineRenderer line;

    private void Update()
    {
        RaycastHit snapHit;
        if(Physics.SphereCast(SetCastPoint(gameObject), snapRadius, Vector3.back, out snapHit, snapRadius, snapLayer))
        {
            snapOffset = gameObject.transform.position - parentObject.transform.position;

            if(!gameObject.CompareTag(snapHit.collider.tag))
            {
                targetSnap = snapHit.collider.gameObject;

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
        else
        {
            line.enabled = false;
            shouldSnap = false;
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
