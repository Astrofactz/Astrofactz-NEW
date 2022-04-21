using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorBehavior : MonoBehaviour
{
    // <summary>
    /// The cursor used when in game
    /// </summary>
    public Texture2D ingameCursor;

    /// <summary>
    /// The cursor used when holding down left click
    /// </summary>
    public Texture2D ingameCursorPressed;

    /// <summary>
    /// Forces cursor to render as the selected sprite
    /// </summary>
    public CursorMode cursorMode = CursorMode.Auto;

    /// <summary>
    /// The offset from the top left of the texture to use as the target point
    /// </summary>
    public Vector2 hotspot = Vector2.zero;


    private void Start()
    {
        // Gets mouse position, raycasts to find snap points
        Vector3 mousePos = FindMousePos();
    }
    private void OnMouseUp()
    {
        Cursor.SetCursor(ingameCursor, hotspot, cursorMode);
        print("cursor");
    }

    private void OnMouseDown()
    {
        Cursor.SetCursor(ingameCursorPressed, hotspot, cursorMode);
        print("cursordown");
    }
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
