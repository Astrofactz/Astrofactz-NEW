using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DifficultySelection : MonoBehaviour
{
    [SerializeField]
    private LevelData levelData;

    public void SetDifficulty(string diff)
    {
        levelData.LevelDifficulty = diff;
        SceneManager.LoadScene("PuzzleSelection");
    }
}
