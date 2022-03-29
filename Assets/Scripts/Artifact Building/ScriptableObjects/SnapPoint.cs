/*******************************************************************************
// File Name :      SnapPoint.cs
// Author :         Avery Macke
// Creation Date :  11 March 2022
// 
// Description :    Contains variables for snap point objects.
*******************************************************************************/
using UnityEngine;

[CreateAssetMenu(fileName = "New Fragment", menuName = "Fragment Assets/Snap Point")]
public class SnapPoint : ScriptableObject
{
    [Header("Snap Point Variables")]

    [Tooltip("Radius in which mouse detects snap point")]
    public float snapPointRadius;
}
