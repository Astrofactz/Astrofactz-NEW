/*******************************************************************************
// File Name :      PedestalBehavior.cs
// Author :         Avery Macke
// Creation Date :  30 March 2022
// 
// Description :    Allows for general game functionality; tracks UI elements,
                    game win condition.
*******************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("Options UI panel")]
    public GameObject optionsUI;

    /// <summary>
    /// Tracks whether options panel is active
    /// </summary>
    private bool optionsActive = false;

    [Tooltip("Buttons UI panel")]
    public GameObject buttonsUI;

    [Tooltip("Tutorial UI panel")]
    public GameObject tutorialUI;

    [Header("Game Win Variables")]

    [Tooltip("Game win UI panel")]
    public GameObject gameWinUI;

    [Tooltip("Fireworks to spawn at game win")]
    public GameObject firework;

    public float timeBetweenSpawn = 1.0f;               // ???

    private SoundManager soundManager;

    public BGMusicBehavior bgMusic;

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    /// <summary>
    /// Tracks whether game has been won
    /// </summary>
    public bool gameWon { get; set; } = false;

    /// <summary>
    /// Tracks whether player is currenly interacting with fragment/pedestal
    /// </summary>
    public bool isDraggingPiece { get; set; } = false;


    private void Start()
    {
        Invoke("FindBGM", 1.0f);
    }

    private void FindBGM()
    {
        bgMusic = FindObjectOfType<BGMusicBehavior>();
    }

    /// <summary>
    /// Called every frame; enables options menu when escape is pressed
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
    /// Checks whether tutorial should be displayed when playing level
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

        soundManager.Play("Win Sound");
        //soundManager.Stop("Stellar Station");
        Destroy(bgMusic.gameObject);
        soundManager.Play("Artifact Fixed");
    }

    /// <summary>
    /// Starts firework particle effect
    /// </summary>
    public void Firework()
    {
        Instantiate(firework, new Vector3(0, 0, 10), Quaternion.identity);
    }

    public void ProgressPopUp()
    {
        // move pop-ups here????
    }
}
