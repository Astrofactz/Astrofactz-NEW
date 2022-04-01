/*******************************************************************************
// File Name :      AudioPrefsManager.cs
// Author :         Avery Macke
// Creation Date :  1 April 2022
// 
// Description :    Manages player audio preferences.
*******************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioPrefsManager : MonoBehaviour
{
    [Tooltip("List of Audio sliders")]
    public List<Slider> audioSliders;

    [Tooltip("List of AudioMixer/player pref Variable names; MUST BE IN SAME" +
             "ORDER AS AUDIO SLIDER LIST")]
    public List<string> audioVariableName;

    /// <summary>
    /// Called at first frame; retrieves player audio preferences or sets to
    /// default value
    /// </summary>
    void Start()
    {
        for (int i = 0; i < audioSliders.Count; i++)
        {
            audioSliders[i].value = PlayerPrefs.GetFloat(audioVariableName[i],
                                                         10.0f);
        }
    }
}
