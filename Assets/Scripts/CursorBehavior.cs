/*******************************************************************************
// File Name :      CursorBehavior.cs
// Author :         
// Creation Date :  2o April 2022
// 
// Description :    Handles custom cursor behavior; swaps between sprites when
                    mouse is clicked.
*******************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CursorBehavior : MonoBehaviour
{
    // <summary>
    /// The cursor used when in game
    /// </summary>
    public Texture2D inGameCursor;

    /// <summary>
    /// The cursor used when holding down left click
    /// </summary>
    public Texture2D inGameCursorPressed;

    /// <summary>
    /// The cursor used when holding down left click in the menus
    /// </summary>
    public Texture2D inGameCursorTap;

    /// <summary>
    /// Forces cursor to render as the selected sprite
    /// </summary>
    public CursorMode cursorMode = CursorMode.Auto;

    /// <summary>
    /// The offset from the top left of the texture to use as the target point
    /// </summary>
    public Vector2 hotspot = Vector2.zero;

    public bool isMainMenuScenes;

    /// <summary>
    /// Called at start; sets mouse cursor sprite
    /// </summary>
    public void Start()
    {
        Cursor.SetCursor(inGameCursor, hotspot, cursorMode);


    }

    /// <summary>
    /// Called every frame; detects mouse button inputs
    /// </summary>
    public void Update()
    {

        if (Input.GetMouseButtonDown(0) && isMainMenuScenes == false)
            Cursor.SetCursor(inGameCursorPressed, hotspot, cursorMode);

        else if (Input.GetMouseButtonDown(0) && isMainMenuScenes == true)
            Cursor.SetCursor(inGameCursorTap, hotspot, cursorMode);

        else if (Input.GetMouseButtonUp(0))
            Cursor.SetCursor(inGameCursor, hotspot, cursorMode);
    }

    /* Moved these to update; OnMouse is only called when clicking an object's
     * collider, putting it in update will let us detect mouse clicks whether
     * you're clicking an object or not. (But! Same code still works great!) */

    //private void OnMouseUp()
    //{
    //    print("cursor");
    //    Cursor.SetCursor(ingameCursor, hotspot, cursorMode);
    //}

    //private void OnMouseDown()
    //{
    //    print("cursor down");
    //    Cursor.SetCursor(ingameCursorPressed, hotspot, cursorMode);
    //}
}
