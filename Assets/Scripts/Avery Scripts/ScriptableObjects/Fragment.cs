using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Fragment", menuName = "Fragment Assets/Fragment")]
public class Fragment : ScriptableObject
{
    [Header("Fragment Variables")]

    [Tooltip("Index of SnapPoint layer")]
    public int snapLayerMask;

    [Header("Active Movement")]

    [Tooltip("Speed fragment moves when dragged")]
    public float dragSpeed;

    [Tooltip("Speed fragment moves when snapping into place")]
    public float snapSpeed;

    [Tooltip("Speed fragment rotates")]
    public float rotateSpeed;

    [Header("Idle Movement")]

    [Tooltip("Speed object moves when idle")]
    public float moveIdleSpeed;

    [Tooltip("Speed object rotates when idle")]
    public float rotateIdleSpeed;


    // might need rotation offsets/threshold or other weird shit
}
