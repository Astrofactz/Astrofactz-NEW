using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("Options UI panel")]
    public GameObject optionsUI;

    /// <summary>
    /// 
    /// </summary>
    private bool optionsActive = false;

    [Tooltip("Buttons UI panel")]
    public GameObject buttonsUI;

    [Tooltip("Tutorial UI panel")]
    public GameObject tutorialUI;

    [Header("Game Win Variables")]

    [Tooltip("Game win UI panel")]
    public GameObject gameWinUI;

    public GameObject firework;

    public float timeBetweenSpawn = 1.0f;

    /// <summary>
    /// 
    /// </summary>
    public bool gameWon { get; set; } = false;

    /// <summary>
    /// 
    /// </summary>
    void Start()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            ToggleOptions();
    }

    /// <summary>
    /// Toggles options panel on/off
    /// </summary>
    public void ToggleOptions()
    {
        if (optionsActive)
            optionsUI.SetActive(false);

        else if (!optionsActive)
            optionsUI.SetActive(true);

        optionsActive = !optionsActive;
    }

    /// <summary>
    /// 
    /// </summary>
    private void CheckTutorial()
    {
        // if main menu scene
            // set player prefs to true


        // if gameplay scene
            // check player prefs

            // if false, do nothing
            // if true, display tutorial

            // set player prefs to false
    }

    /// <summary>
    /// When the artifact is completed, the winning UI will appear and fireworks
    /// will be invoked
    /// </summary>
    public void ArtifactComplete()                                              // COROUTINE
    {
        gameWon = true;

        // disable blueprint canvas
        // rotate pedestal to face front
        // spawn fireworks? here or later?

        // trigger complete artifact animation

        // trigger complete artifact curator pop-ups
        // click to continue, enable continue panel in UI

        buttonsUI.SetActive(false);
        gameWinUI.SetActive(true);

        Invoke("Firework", 0);
    }


    /// <summary>
    /// Starts firework particle effect
    /// </summary>
    public void Firework()
    {
        Instantiate(firework, new Vector3(0, 0, 10), Quaternion.identity);
    }
}
