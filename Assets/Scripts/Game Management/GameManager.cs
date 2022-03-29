using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public GameObject GameWinUI;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.Escape))
        {
            string currentScene = SceneManager.GetActiveScene().name;

            SceneManager.LoadScene(currentScene);
        }
    }


    public void ArtifactComplete()
    {
        GameWinUI.SetActive(true);
    }
}