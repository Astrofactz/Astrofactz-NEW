using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ArtifactSelection : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;

    public void SetArtifact(string artifact)
    {
        levelData.LevelDifficulty = artifact;
        Loadlevel();
    }

    private void Loadlevel()
    {
        if (levelData.LevelDifficulty == "Easy" && levelData.PuzzleSelection == "Rover")
        {
            SceneManager.LoadScene("RoverSceneEasy");
        }

        if (levelData.LevelDifficulty == "Easy" && levelData.PuzzleSelection == "Blender")
        {
            SceneManager.LoadScene("BlenderEasy");
        }

        if (levelData.LevelDifficulty == "Medium" && levelData.PuzzleSelection == "Rover")
        {
            SceneManager.LoadScene("RoverSceneMedium");
        }

        if (levelData.LevelDifficulty == "Medium" && levelData.PuzzleSelection == "Blender")
        {
            SceneManager.LoadScene("BlenderMedium");
        }

        if (levelData.LevelDifficulty == "Hard" && levelData.PuzzleSelection == "Rover")
        {
            SceneManager.LoadScene("RoverSceneHard");
        }

        if (levelData.LevelDifficulty == "Hard" && levelData.PuzzleSelection == "Blender")
        {
            SceneManager.LoadScene("BlenderSceneHard");
        }
    }
}
