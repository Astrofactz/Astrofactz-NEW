using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Tooltip("Options UI panel")]
    public GameObject optionsUI;

    [Tooltip("Options close menu button")]
    public GameObject optionsCloseButton;

    [Tooltip("Blueprint UI panel")]
    public GameObject blueprintUI;

    [Tooltip("Game win UI panel")]
    public GameObject gameWinUI;

    public GameObject firework;

    public float timeBetweenSpawn = 1.0f;

    private bool blueprintActive, optionsActive = false;

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
    /// Toggles blueprint panel on/off
    /// </summary>
    public void ToggleBlueprint()
    {
        if (blueprintActive)
            blueprintUI.SetActive(false);

        else if (!blueprintActive)
            blueprintUI.SetActive(true);

        blueprintActive = !blueprintActive;
    }

    public void InvisibleToggle()
    {
        if(blueprintActive)
        {
            blueprintUI.SetActive(false);
        }

        blueprintActive = !blueprintActive;
    }

    /// <summary>
    /// Toggles options panel on/off
    /// </summary>
    public void ToggleOptions()
    {
        if (optionsActive)
        {
            optionsUI.SetActive(false);
            optionsCloseButton.SetActive(false);
        }

        else if (!optionsActive)
        {
            optionsUI.SetActive(true);
            optionsCloseButton.SetActive(true);
        }

        optionsActive = !optionsActive;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ArtifactComplete()
    {
        gameWinUI.SetActive(true);
        Invoke("Firework", 0);
    }


    /// <summary>
    /// 
    /// </summary>
    public void Firework()
    {
        Instantiate(firework, new Vector3(0, 0, 10), Quaternion.identity);
    }

}
