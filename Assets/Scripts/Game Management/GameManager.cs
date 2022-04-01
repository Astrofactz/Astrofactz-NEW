using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject blueprintUI;

    public GameObject GameWinUI;
    public GameObject firework;

    public float timeBetweenSpawn = 1.0f;

    private bool blueprintActive = false;

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
        if(Input.GetKey(KeyCode.Escape))
        {
            string currentScene = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(currentScene);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ToggleBlueprint()
    {
        if (blueprintActive)
            blueprintUI.SetActive(false);

        else if (!blueprintActive)
            blueprintUI.SetActive(true);

        blueprintActive = !blueprintActive;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ArtifactComplete()
    {
        GameWinUI.SetActive(true);
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
