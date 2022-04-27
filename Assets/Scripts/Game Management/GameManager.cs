/*******************************************************************************
// File Name :      PedestalBehavior.cs
// Author :         Avery Macke
// Creation Date :  30 March 2022
// 
// Description :    Allows for general game functionality; tracks UI elements,
                    game win condition.
*******************************************************************************/
using System.Collections;
using UnityEngine;
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

    [Header("Pop-Up Variables")]
    [Tooltip("Count of all fragments in scene")]
    public int fragmentCount;

    [Tooltip("Counts to display popups on")]
    public int[] popupIndex;

    [Tooltip("Array of pop-up variants ")]
    public GameObject[] popups;

    public float popupDuration;

    [Header("Game Win Variables")]
    [Tooltip("Game win UI panel")]
    public GameObject gameWinUI;

    [Tooltip("Fireworks to spawn at game win")]
    public GameObject firework;

    [Tooltip("Pedestal object in scene")]
    public GameObject pedestal;

    [Tooltip("Artifact base")]
    public GameObject artifactBase;

    [Tooltip("Artifact animation object")]
    public GameObject artifactAnimation;

    public float timeBetweenSpawn = 1.0f;               // ???

    private SoundManager soundManager;

    private BGMusicBehavior bgMusic;

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

        if(SceneManager.GetActiveScene().name == "MainMenu")
        {
            PlayerPrefs.SetInt("ShowTutorial", 1);
        }

        if(tutorialUI != null)
        {
            if(PlayerPrefs.GetInt("ShowTutorial") == 1)
            {
                tutorialUI.SetActive(true);
                PlayerPrefs.SetInt("ShowTutorial", 0);
            }
        }
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
    /// When the artifact is completed, the winning UI will appear and fireworks
    /// will be invoked
    /// </summary>
    public void ArtifactComplete()
    {
        StartCoroutine(GameWin());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator GameWin()
    {
        foreach (GameObject popup in popups)
        {
            popup.SetActive(false);
        }

        gameWon = true;

        soundManager.Play("Win Sound");

        Destroy(bgMusic.gameObject);

        soundManager.Play("Artifact Fixed");

        buttonsUI.SetActive(false);

        gameWinUI.SetActive(true);          // update curator pop-ups, win UI

        InvokeRepeating("Firework", 0.0f, 1.5f);

        Quaternion targetRot = Quaternion.Euler(Vector3.zero);

        float rotDiff = (pedestal.transform.rotation.eulerAngles - targetRot.eulerAngles).magnitude;

        while ((rotDiff > 0.1f || rotDiff < -0.1f) && rotDiff != 360)
        {
            Quaternion target = Quaternion.RotateTowards(pedestal.transform.rotation, targetRot, 75.0f * Time.deltaTime);

            pedestal.transform.rotation = target;

            rotDiff = (pedestal.transform.rotation.eulerAngles - targetRot.eulerAngles).magnitude;

            yield return null;
        }

        pedestal.transform.rotation = targetRot;

        yield return new WaitForSeconds(0.5f);

        artifactBase.SetActive(false);

        artifactAnimation.SetActive(true);

        yield return new WaitForSeconds(5.0f);

        CancelInvoke("Firework");
    }

    /// <summary>
    /// Starts firework particle effect
    /// </summary>
    public void Firework()
    {
        Instantiate(firework, new Vector3(0, 0, 10), Quaternion.identity);
    }

    bool popupActive = false;

    public void CheckProgress()
    {
        fragmentCount--;

        foreach (int index in popupIndex)
        {
            if(index == fragmentCount && !popupActive)
            {
                int random = Random.Range(0, popups.Length);

                StartCoroutine(ProgressPopUp(popups[random]));
            }
        }
    }

    private IEnumerator ProgressPopUp(GameObject popup)
    {
        popupActive = true;

        popup.SetActive(true);

        yield return new WaitForSeconds(popupDuration);

        popup.SetActive(false);

        popupActive = false;
    }
}
