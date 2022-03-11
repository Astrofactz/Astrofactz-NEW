using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "LevelData")]
public class LevelData : ScriptableObject
{
    [SerializeField]
    private string levelDifficulty;
    public string LevelDifficulty
    {
        get => levelDifficulty;
        set => levelDifficulty = value;
    }

    [SerializeField]
    private string puzzleSelection;

    public string PuzzleSelection
    {
        get => puzzleSelection;
        set => puzzleSelection = value;
    }
}
