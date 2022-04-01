/*******************************************************************************
// File Name :      SceneTransitionManager.cs
// Author :         Avery Macke, 
// Creation Date :  25 March 2022
// 
// Description :    Manages transitions and loading/unloading between scenes.
*******************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    [Tooltip("List of objects to retain during async scene transition")]
    public GameObject[] retainObjs = new GameObject[1];

    enum Artifact { Rover, Rocket }

    private string artifact;

    private string sceneToLoad;

    private LevelSelectButtonsScript levelSelect;

    /// <summary>
    /// 
    /// </summary>
    public void Start()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="artifact"></param>
    public void SetArtifact(string artifact)
    {
        this.artifact = artifact;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="difficulty"></param>
    public void SetDifficulty(string difficulty)
    {
        sceneToLoad = artifact + difficulty;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneToLoad"></param>
    public void SetSceneToLoad(string difficulty)
    {
        if (difficulty == "easy")
        {
            if (levelSelect.Rover)
                sceneToLoad = "RoverEasy";
            else
                sceneToLoad = "RocketEasy";
        }
        else if (difficulty == "medium")
        {
            if (levelSelect.Rover)
                sceneToLoad = "RoverMedium";
            else
                sceneToLoad = "RocketMedium";
        }
        else if (difficulty == "hard")
        {
            if (levelSelect.Rover)
                sceneToLoad = "RoverHard";
            else
                sceneToLoad = "RocketHard";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void LoadSceneLevelSelect()
    {
        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// Loads new scene
    /// </summary>
    /// <param name="sceneName">String name of scene to load</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    /// <summary>
    /// Lads new scene asynchronously
    /// </summary>
    /// <param name="sceneName">String name of scene to load</param>
    public void LoadSceneAsync(string sceneName)
    {
        StartCoroutine("ChangeSceneAsync", sceneName);
    }

    /// <summary>
    /// Loads gameplay scene; can move objects from one scene to another
    /// </summary>
    /// <returns></returns>
    IEnumerator ChangeSceneAsync(string sceneName)
    {
        // Async loads gameplay scene
        Scene prevScene = (SceneManager.GetActiveScene());
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName,
                                   LoadSceneMode.Additive);

        // Waits until loading is finished
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // Transfers objects from current scene to new
        foreach (GameObject obj in retainObjs)
        {
            SceneManager.MoveGameObjectToScene(obj,
                         SceneManager.GetSceneByName(sceneName));
        }

        // Unloads customization scene
        SceneManager.UnloadSceneAsync(prevScene);
    }

    /// <summary>
    /// 
    /// </summary>
    public void QuitGame()
    {
        Application.Quit();
    }
}
