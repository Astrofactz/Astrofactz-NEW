/*******************************************************************************
// File Name :      AudioSliderBehavior.cs
// Author :         Avery Macke
// Creation Date :  1 April 2022
// 
// Description :    Allows for volume to be adjusted with audio sliders.
*******************************************************************************/
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class AudioSlider : MonoBehaviour
{
    [Header("Audio Slider Variables")]

    [Tooltip("Name of corresponding AudioMixer/Player Pref Variable")]
    public string audioVariableName;

    [Tooltip("Corresponding Audio Slider")]
    public Slider audioSlider;

    [Tooltip("Main AudioMixer")]
    public AudioMixer audioMixer;

    /// <summary>
    /// Allows player to adjust volume with slider
    /// </summary>
    /// <param name="sliderValue">Float value from slider</param>
    public void SetVolume(float sliderValue)
    {
        // Converts slider to whole numbers to work with below conversion
        if (sliderValue == 0.0f)
            sliderValue = 0.0001f;
        else
            sliderValue /= 10.0f;

        // Converts linear slider value to exponential AudioGroup value
        float vol = Mathf.Log10(sliderValue) * 20.0f;

        audioMixer.SetFloat(audioVariableName, vol);

        // Saves value to PlayerPrefs
        PlayerPrefs.SetFloat(audioVariableName, audioSlider.value);
    }
}
