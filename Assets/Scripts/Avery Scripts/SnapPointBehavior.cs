/*******************************************************************************
// File Name :      SnapPointBehavior.cs
// Author :         Avery Macke
// Creation Date :  11 March 2022
// 
// Description :    Manages Snap Point functionality.
*******************************************************************************/
using UnityEngine;

public class SnapPointBehavior : MonoBehaviour
{
    [Tooltip("SnapPoint ScriptableObject")]
    public SnapPoint snapPoint;

    /// <summary>
    /// Radius in which mouse detects snap point
    /// </summary>
    private float snapPointRadius;

    /// <summary>
    /// Called at first frame; initializes variables
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
        snapPointRadius = snapPoint.snapPointRadius;

        GetComponent<SphereCollider>().radius = snapPointRadius;
    }
}
