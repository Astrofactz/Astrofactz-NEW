/*******************************************************************************
// File Name :      FragmentBehavior.cs
// Author :         Avery Macke
// Creation Date :  4 March 2022
// 
// Description :    Allows for movement, placement, and combination of artifact
                    pieces.
*******************************************************************************/
using UnityEngine;

public class FragmentBehavior : MonoBehaviour
{
    [Tooltip("Fragment ScriptableObject")]
    public Fragment fragment;

    [Tooltip("Correct Snap Point for fragment")]
    public GameObject correctSnapPoint;

    #region Active Movement
    /// <summary>
    /// Index of SnapPoint layer
    /// </summary>
    private int snapLayerMask;

    /// <summary>
    /// Speed fragment moves when dragged
    /// </summary>
    private float dragSpeed;

    /// <summary>
    /// Speed fragment moves when snapping into place
    /// </summary>
    private float snapSpeed;

    /// <summary>
    /// Speed fragment rotates
    /// </summary>
    private float rotateSpeed;
    #endregion

    #region Idle Movement
    /// <summary>
    /// Speed fragment moves when idle
    /// </summary>
    private float moveIdleSpeed;

    /// <summary>
    /// Speed fragment rotates when idle
    /// </summary>
    private float rotateIdleSpeed;
    #endregion

    /// <summary>
    /// Array of all artifact fragments in scene
    /// </summary>
    private FragmentBehavior[] fbArray;

    /// <summary>
    /// Current snap point target
    /// </summary>
    private GameObject currentSnapTarget = null;

    /// <summary>
    /// Tracks whether piece is placed in correct position
    /// </summary>
    private bool isPlaced = false;

    /// <summary>
    /// Tracks whether the piece is currently being dragged
    /// </summary>
    private bool beingDragged = false;

    /// <summary>
    /// Tracks previous mouse positoin
    /// </summary>
    private Vector3 prevFragmentPos;

    /// <summary>
    /// Tracks whether child snap points are active
    /// </summary>
    private bool snapPointsActive = true;

    /// <summary>
    /// Starting position of piece
    /// </summary>
    private Vector3 startPos;

    /// <summary>
    /// Reference to BoxCollider component
    /// </summary>
    private BoxCollider bc;

    /// <summary>
    /// Reference to Rigidbody componend
    /// </summary>
    private Rigidbody rb;

    /// <summary>
    /// Called at start; initializes variables
    /// </summary>
    void Start()
    {
        InitializeVariables();
        AddRandomForce();
    }

    /// <summary>
    /// Initializes variables
    /// </summary>
    private void InitializeVariables()
    {
        startPos = transform.position;

        snapLayerMask = fragment.snapLayerMask;

        dragSpeed = fragment.dragSpeed;
        snapSpeed = fragment.snapSpeed;
        rotateSpeed = fragment.rotateSpeed;

        moveIdleSpeed = fragment.moveIdleSpeed;
        rotateIdleSpeed = fragment.rotateIdleSpeed;

        bc = GetComponent<BoxCollider>();
        rb = GetComponent<Rigidbody>();

        fbArray = FindObjectsOfType<FragmentBehavior>();

        ToggleSnapPoints(snapPointsActive);
    }

    /// <summary>
    /// Returns whether fragment has been correctly placed
    /// </summary>
    /// <returns>Whether fragment is placed correctly</returns>
    public bool IsPlaced()
    {
        return isPlaced;
    }

    #region Fragment interaction
    /// <summary>
    /// Called when mouse drags object; if mouse collides with snap point, snaps
    /// piece to snap point, else piece follows mouse
    /// </summary>
    private void OnMouseDrag()
    {
        if (!isPlaced)
            MovePiece();
    }

    /// <summary>
    /// Called when mouse is released; determines fragment behavior when mouse
    /// is released
    /// </summary>
    private void OnMouseUp()
    {
        beingDragged = false;

        // If has snap target
        if (currentSnapTarget)
        {
            // If correct target, combine pieces
            if (currentSnapTarget == correctSnapPoint)
                CombinePieces();

            // If incorrect target                                              // figure out what to do when pieces placed incorrectly
            else if (currentSnapTarget != correctSnapPoint)                     // also zPos of pieces; bg when floating, foreground when moving, artifact in middle
            {                                                                   // if placed incorrectly anywhere in build zone, teleport it those outside bounds
                AddIdleForce();
            }
        }
        // If does not have snap target
        else
        {
            // Throw piece, if magnitude under threshold, add random force
            AddIdleForce();

            if (rb.velocity.magnitude < 0.01f)                                  // need stop movement check for non-drag pieces too
                Invoke("AddRandomForce", 2.0f);
        }
    }
    #endregion


    #region Fragment movement and behavior
    /// <summary>
    /// Moves piece; piece follows mouse if not on snap point; if mouse is over
    /// snap point, piece snaps to snap point
    /// </summary>
    private void MovePiece()
    {
        CancelInvoke("AddRandomForce");

        beingDragged = true;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        // Gets mouse position, raycasts to find snap points
        Vector3 mousePos = FindMousePos();

        Ray snapRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << snapLayerMask;

        if (Physics.Raycast(snapRay, out RaycastHit hit, Mathf.Infinity, layerMask))
            currentSnapTarget = hit.transform.gameObject;
        else
            currentSnapTarget = null;

        prevFragmentPos = transform.position;

        // If mouse on snap point, snap piece to point
        if (currentSnapTarget)
        {
            Vector3 targetPos = currentSnapTarget.transform.position;
            transform.position = Vector3.MoveTowards(transform.position,
                                 targetPos, snapSpeed * Time.deltaTime);
        }

        // Else follow mouse
        else if (!currentSnapTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                                  mousePos, dragSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Adds force to Rigidbody in a random direction; called when non-placed
    /// fragment stops moving
    /// </summary>
    private void AddRandomForce()
    {
        float xForce, yForce;

        xForce = Random.Range(-1.0f, 1.0f);
        yForce = Random.Range(-1.0f, 1.0f);

        Vector3 moveForce = new Vector3(xForce, yForce, 0.0f) * moveIdleSpeed
                                                              * Time.deltaTime;

        rb.AddForce(moveForce);
    }

    /// <summary>
    /// Adds force in direction fragment is thrown after dragging; called when
    /// mouse is released
    /// </summary>
    private void AddIdleForce()
    {
        Vector3 direction = transform.position - prevFragmentPos;
        direction.z = 0;

        float speed = direction.magnitude;

        Vector3 moveForce = speed * direction.normalized;

        rb.velocity = moveForce * moveIdleSpeed;
    }


    private void CheckBoundsDrag()
    {
        // prevent player from dragging piece out of bounds
    }

    private void CheckBoundsIdle()
    {
        // if not being dragged
        // if past x bound, teleport to other side
        // if past y bound, teleport to other side
    }

    /// <summary>
    /// Called on collision; handles knockback and collision of fragments
    /// </summary>
    /// <param name="collision">Other collider ivolved in collision</param>
    private void OnCollisionEnter(Collision collision)
    {
        if(!beingDragged && collision.gameObject.tag == "Fragment")
        {
            // Applies force in opposite direction of collision
            Vector3 force = transform.position - collision.transform.position;
            force.z = 0;

            force.Normalize();

            rb.AddForce(force * moveIdleSpeed * Time.deltaTime);
        }
    }

    /// <summary>
    /// Finds position of mouse in world space using raycast
    /// </summary>
    /// <returns>Position of mouse in world space</returns>
    private Vector3 FindMousePos()
    {
        Vector3 mousePos = new Vector3();

        Plane plane = new Plane(Vector3.forward, 0);

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(mouseRay, out float distance))
            mousePos = mouseRay.GetPoint(distance);

        return mousePos;
    }

    /// <summary>
    /// Combines fragment when targeting correct snap point; childs fragment
    /// to snap point parent, manages snap points, and checks for win
    /// </summary>
    private void CombinePieces()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        GameObject parentArtifact = correctSnapPoint.transform.parent.gameObject;
        transform.parent = parentArtifact.transform;

        bc.enabled = false;

        // Enable fragments snap points
        ToggleSnapPoints(snapPointsActive);

        // Disables snap point fragment connected to
        DisableSnapPoint(correctSnapPoint);

        isPlaced = true;

        CheckArtifactComplete();
    }
    #endregion

    /// <summary>
    /// Checks if artifact has been completed; called when pieces are assembled
    /// </summary>
    /// <returns>True if artifact is complete, false if incomplete</returns>
    private bool CheckArtifactComplete()
    {
        bool artifactComplete = true;

        // If any fragment is not placed, artifact is not complete
        foreach(FragmentBehavior fb in fbArray)
        {
            if (!fb.IsPlaced())
                artifactComplete = false;
        }

        if (artifactComplete)
            print("game over");
        else
            print("not game over");

        return artifactComplete;
    }

    /// <summary>
    /// Disables snap point after a piece has been placed at that point
    /// </summary>
    /// <param name="snapPoint">Snap point to disable</param>
    private void DisableSnapPoint(GameObject snapPoint)
    {
        snapPoint.SetActive(false);
    }

    /// <summary>
    /// Toggles childed snap points on and off
    /// </summary>
    /// <param name="snapPointStatus">Tracks if snap points are active</param>
    private void ToggleSnapPoints(bool snapPointStatus)
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject.tag == "SnapPoint")
                child.gameObject.SetActive(!snapPointStatus);
        }
        snapPointsActive = !snapPointsActive;
    }
}
