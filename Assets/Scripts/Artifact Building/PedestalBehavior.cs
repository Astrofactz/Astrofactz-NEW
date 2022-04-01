using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PedestalBehavior : MonoBehaviour
{
    [Tooltip("Pedestal rotation speed")]
    public float rotateSpeed;

    [Tooltip("Distance mouse must be before pedestal can rotate")]
    public float rotationRadius;

    /// <summary>
    /// Direction pedestal should rotate
    /// </summary>
    private Vector3 rotationDir = Vector3.zero;

    /// <summary>
    /// Called every frame mouse is dragged; if holding right click, rotate
    /// pedestal
    /// </summary>
    private void OnMouseDrag()
    {
        if (Input.GetMouseButton(1))
            Rotate();
    }

    /// <summary>
    /// Rotates pedestal on y axis based on position of mouse
    /// </summary>
    private void Rotate()
    {
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