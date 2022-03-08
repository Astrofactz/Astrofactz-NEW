/*******************************************************************************
// File Name :      SnapBehavior.cs
// Author :         Avery Macke
// Creation Date :  4 March 2022
// 
// Description :    Allows for movement and placement of artifact pieces.
*******************************************************************************/
using UnityEngine;

public class SnapBehavior : MonoBehaviour
{
    [Tooltip("Correct snap point for piece")]
    public GameObject correctSnapTarget;

    [Tooltip("Index of SnapPoint layer mask")]
    public int snapLayerMask;

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
        if(currentSnapTarget == correctSnapTarget)
        {
            transform.parent = correctSnapTarget.transform;
            isPlaced = true;
        }
        else
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
            transform.position = currentSnapTarget.transform.position;

        // Else follow mouse
        else if (!currentSnapTarget)
            transform.position = mousePos;
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
