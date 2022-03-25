/*******************************************************************************
// File Name :      SceneTimer.cs
// Author :         Madison Gorman, Avery Macke
// Creation Date :  9 March 2022
// 
// Description :    Allows for scenes to last only specified duration.
*******************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTimer : MonoBehaviour
{
    [Tooltip("Total amount of time scene should last")]
    public float sceneTime;

    [Tooltip("Target scene to load")]
    public string sceneToLoad;

    /// <summary>
    /// String name of current invoked function
    /// </summary>
    private string currentInvoke;

    /// <summary>
    /// Reference to SceneTransitionManager script
    /// </summary>
    private SceneTransitionManager stm;

    /// <summary>
    /// Called at start; assigns variables, invokes correct timer function
    /// </summary>
    void Start()
    {
        stm = FindObjectOfType<SceneTransitionManager>();

        Invoke("TransitionScene", sceneTime);
        currentInvoke = "TransitionScene";
    }

    /// <summary>
    /// Called after timer ends, transitions to new scene
    /// </summary>
    private void TransitionScene()
    {
        stm.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// Resets timer
    /// </summary>
    public void ResetTimer()
    {
        CancelInvoke(currentInvoke);
        Invoke(currentInvoke, sceneTime);
    }
}
