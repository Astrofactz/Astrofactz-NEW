using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalBehavior : MonoBehaviour
{
    [Tooltip("Pedestal rotation speed")]
    public float rotateSpeed;

    [Tooltip("Distance mouse must be before pedestal can rotate")]
    public float rotationRadius;

    private bool isRotating = false;

    /// <summary>
    /// Direction pedestal should rotate
    /// </summary>
    private Vector3 rotationDir = Vector3.zero;

    /// <summary>
    /// Default and selected outline shaders
    /// </summary>
    private GameObject outlineIdle, outlineSelected;

    private GameManager gm;

    private void Start()
    {
        gm = FindObjectOfType<GameManager>();

        outlineIdle = transform.Find("outlineIdle").gameObject;

        outlineSelected = transform.Find("outlineSelected").gameObject;
    }

    /// <summary>
    /// Called every frame mouse is dragged; if holding right click, rotate
    /// pedestal
    /// </summary>
    private void OnMouseDrag()
    {
        if(!gm.gameWon)
            Rotate();
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnMouseUp()
    {
        gm.isDraggingPiece = false;
        isRotating = false;
        outlineIdle.SetActive(true);
        outlineSelected.SetActive(false);
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnMouseEnter()
    {
        if (!gm.isDraggingPiece)
        {
            outlineIdle.SetActive(false);
            outlineSelected.SetActive(true);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    private void OnMouseExit()
    {
        if (!isRotating)
        {
            outlineIdle.SetActive(true);
            outlineSelected.SetActive(false);
        }
    }

    /// <summary>
    /// Rotates pedestal on y axis based on position of mouse
    /// </summary>
    private void Rotate()
    {
        gm.isDraggingPiece = true;
        isRotating = true;

        Vector3 mousePos = FindMousePos();
        Ray snapRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Direction and distance of mouse
        float mouseDir = transform.position.x - mousePos.x;
        float mouseDis = Vector3.Distance(transform.position, mousePos);

        // Rotate right
        if (mouseDir > 0f && mouseDis > rotationRadius)
            rotationDir = Vector3.up;
        // Rotate left
        else if (mouseDir < 0f && mouseDis > rotationRadius)
            rotationDir = Vector3.down;
        // Don't rotate
        else
            rotationDir = Vector3.zero;

        // Rotates towards target direction
        Vector3 targetRot = transform.eulerAngles + rotationDir;
        Quaternion targetRotation = Quaternion.Euler(targetRot);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation,
                             (rotateSpeed * mouseDis * Time.deltaTime));
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

}