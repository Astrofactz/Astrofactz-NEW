using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrag : MonoBehaviour
{
    private float startPosX;
    private float startPosY;
    public bool isBeingHeld;

    private bool isInMenu;

    public float snapRadius;
    public float objectZ;

    public LayerMask snapLayer;
    public LayerMask menuLayer;

    public GameObject[] snapPoints;
    //public GameObject ObjMenu;
    //public Transform snapTarget;

    private RaycastHit hit;
    private GameObject selectedSnapPoint;

    private ArtifactPieceManager apm;

    private void Awake()
    {
        isInMenu = true;

        //finds the artifact manager.
        apm = FindObjectOfType<ArtifactPieceManager>();

    }

    // Update is called once per frame
    void Update()
    {
        if (isBeingHeld)
        {
            //Sets a vector3 to the mouse position.
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos.z = 16;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //checks if every snap point is in range of another.
            foreach(GameObject obj in snapPoints)
            {
                if (Physics.SphereCast(SetCastPoint(obj), snapRadius, Vector3.down, out hit, snapRadius, snapLayer))
                {
                    //selectedSnapPoint = hit.collider.gameObject;

                    //Snaps the selected object to the snap point.
                    gameObject.transform.position = new Vector3(
                        Mathf.Round((mousePos.x - startPosX) + (hit.transform.position.x - Mathf.Round(hit.transform.position.x))),
                        Mathf.Round((mousePos.y - startPosY) + (hit.transform.position.y - Mathf.Round(hit.transform.position.y))),
                        objectZ
                        );

                }
                else
                    gameObject.transform.position = new Vector3((mousePos.x - startPosX), (mousePos.y - startPosY), objectZ);
            }

            //Debug.Log(Physics.OverlapSphere(snapPoint.transform.position, 0.5f, snapLayer));
            
        }
    }

    public Vector3 SetCastPoint(GameObject snapPoint)
    {
        Vector3 pos = new Vector3(
            snapPoint.transform.position.x,
            snapPoint.transform.position.y + snapRadius,
            snapPoint.transform.position.z);
        return pos;
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos;
            mousePos = Input.mousePosition;
            mousePos.z = 16;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            //allows the player to grab any part of the object.
            startPosX = mousePos.x - transform.localPosition.x;
            startPosY = mousePos.y - transform.localPosition.y;           

            isBeingHeld = true;
        }
    }

    /// <summary>
    /// sets the object to not being held, and changes what menu its in, if applicable.
    /// </summary>
    private void OnMouseUp()
    {
        isBeingHeld = false;

        Vector3 mousePos;
        mousePos = Input.mousePosition;
        mousePos.z = 16;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        //checks if the piece needs to be removed from or added to the menu.
        if (Physics.Raycast(mousePos, Vector3.forward, menuLayer) && apm.piecesInSpace.Contains(gameObject))
        {
            apm.BackInMenu(gameObject);
            isInMenu = true;
        }
        else if (apm.piecesInMenu.Contains(gameObject))
        {
            apm.TakeFromMenu(gameObject);
        }

    }

    void OnDrawGizmosSelected()
    {
        // Display the radius of the snap
        Gizmos.color = Color.white;
        //Gizmos.DrawWireSphere(snapPoint.transform.position, 0.5f);
    }
}
