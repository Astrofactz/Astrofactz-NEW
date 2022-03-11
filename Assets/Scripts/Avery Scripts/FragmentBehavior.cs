/*******************************************************************************
// File Name :      FragmentBehavior.cs
// Author :         Avery Macke
// Creation Date :  4 March 2022
// 
// Description :    Allows for movement and placement of artifact pieces.
*******************************************************************************/
using UnityEngine;
using System.Collections.Generic;

public class FragmentBehavior : MonoBehaviour
{
    [Tooltip("List of correct snap targets for piece")]
    public List<GameObject> correctSnapTargets;

    [Tooltip("Index of SnapPoint layer mask")]
    public int snapLayerMask;

    [Tooltip("Movement speed while dragging pieces")]
    public float dragSpeed;

    [Tooltip("Movement speed while piece snaps into place")]
    public float snapSpeed;

    /// <summary>
    /// Current snap point target
    /// </summary>
    private GameObject currentSnapTarget = null;

    /// <summary>
    /// Tracks whether piece is placed in correct position
    /// </summary>
    private bool isPlaced = false;

    /// <summary>
    /// Starting position of piece
    /// </summary>
    private Vector3 startPos;

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
    }

    /// <summary>
    /// Called when mouse drags object; if mouse collides with snap point, snaps
    /// piece to snap point, else piece follows mouse
    /// </summary>
    private void OnMouseDrag()
    {
        if(!isPlaced)
            MovePiece();
    }

    /// <summary>
    /// Called when mouse is released; if piece is in correct position, keeps
    /// piece in position, else sends piece back to starting position
    /// </summary>
    private void OnMouseUp()
    {
        if (currentSnapTarget)
        {
            // Checks through list of correct snap targets

            foreach (GameObject correctSnapTarget in correctSnapTargets)
            {
                // If current target is correct, snap in place
                if (currentSnapTarget == correctSnapTarget && !isPlaced)
                {
                    transform.parent = currentSnapTarget.transform;
                    isPlaced = true;
                }
            }

            // Disables correct snap points after piece is placed
            if (isPlaced)
                DisableSnapPoints(correctSnapTargets);

            else if (!isPlaced)
                transform.position = startPos;
        }

        else if (!currentSnapTarget)
            transform.position = startPos;
    }

    /// <summary>
    /// Moves piece; piece follows mouse if not on snap point; if mouse is over
    /// snap point, piece snaps to snap point
    /// </summary>
    private void MovePiece()
    {
        Vector3 mousePos = FindMousePos();

        Ray snapRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        int layerMask = 1 << snapLayerMask;

        if (Physics.Raycast(snapRay, out hit, Mathf.Infinity, layerMask))
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
    /// 
    /// </summary>
    private void MoveAssembled()
    {
        // if parent, not really an issue, just move pieces
        // if child, will need to get parent's transform
    }

    /// <summary>
    /// Disables all correct snap targets after a piece has been placed
    /// </summary>
    /// <param name="correctSnapTargets">List of piece's correct snap targets</param>
    private void DisableSnapPoints(List<GameObject> correctSnapTargets)
    {
        foreach(GameObject snapTarget in correctSnapTargets)
        {
            snapTarget.GetComponent<SphereCollider>().enabled = false;
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

        float distance;

        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (plane.Raycast(mouseRay, out distance))
            mousePos = mouseRay.GetPoint(distance);

        return mousePos;
    }
}
