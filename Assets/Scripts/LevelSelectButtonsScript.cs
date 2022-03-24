using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectButtonsScript : MonoBehaviour
{
    private bool easy;
    private bool medium;
    private bool hard;
    private bool rover;
    private bool blender;

    public bool Easy { get => easy; set => easy = value; }
    public bool Medium { get => medium; set => medium = value; }
    public bool Hard { get => hard; set => hard = value; }
    public bool Rover { get => rover; set => rover = value; }
    public bool Blender { get => blender; set => blender = value; }

    public void BoolCheck(string category)
    {
        if(category == "Easy")
        {
            easy = true;
            medium = false;
            hard = false;
            print("easy = true");
        }
        if(category == "Medium")
        {
            medium = true;
            easy = false;
            hard = false;
            print("medium = true");
        }
        if(category == "Hard")
        {
            hard = true;
            easy = false;
            medium = false;
            print("hard = true");
        }
        if(category == "Rover")
        {
            rover = true;
            blender = false;
            print("rover = true");
        }
        if(category == "Blender")
        {
            blender = true;
            rover = false;
            print("blender = true");
        }
    }

    public void ConfirmLevelLoader()
    {
        if (blender == true && easy == true)
        {
            SceneManager.LoadScene("BlenderEasy");
        }
        if(blender == true && medium == true)
        {
            SceneManager.LoadScene("BlenderMedium");
        }
        if(blender == true && hard == true)
        {
            SceneManager.LoadScene("BlenderHard");
        }
        if (rover == true && easy == true)
        {
            SceneManager.LoadScene("RoverEasy");
        }
        if (rover == true && medium == true)
        {
            SceneManager.LoadScene("RoverMedium");
        }
        if (rover == true && hard == true)
        {
            SceneManager.LoadScene("RoverHard");
        }
    }
}
