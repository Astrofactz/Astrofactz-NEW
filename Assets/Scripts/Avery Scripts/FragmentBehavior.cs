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
    /// Called at start; initializes variables
    /// </summary>
    void Start()
    {
        InitializeVariables();
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
        if (currentSnapTarget)
        {
            // If correct target, combine pieces
            if (currentSnapTarget == correctSnapPoint)
                CombinePieces();

            // If incorrect target, return to startPos
            else if (currentSnapTarget != correctSnapPoint)
                transform.position = startPos;
                // return to idle movement
        }

        // If no target, stay at position, return to idle movement
        // return to idle movement, move backwards to avoid collision w artifact
    }
    #endregion


    #region Fragment movement and behavior
    /// <summary>
    /// Moves piece; piece follows mouse if not on snap point; if mouse is over
    /// snap point, piece snaps to snap point
    /// </summary>
    private void MovePiece()
    {
        Vector3 mousePos = FindMousePos();

        Ray snapRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        int layerMask = 1 << snapLayerMask;

        if (Physics.Raycast(snapRay, out RaycastHit hit, Mathf.Infinity, layerMask))
            currentSnapTarget = hit.transform.gameObject;
        else
            currentSnapTarget = null;

        // If mouse on snap point, snap piece to point
        if (currentSnapTarget)
        {
            Vector3 targetPos = currentSnapTarget.transform.position;
            transform.position = Vector3.MoveTowards(transform.position, targetPos, snapSpeed * Time.deltaTime);
        }

        // Else follow mouse
        else if (!currentSnapTarget)
        {
            transform.position = Vector3.MoveTowards(transform.position, mousePos, dragSpeed * Time.deltaTime);
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
    /// <param name="snapPointStatus">Tracks whether snap points are active</param>
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
