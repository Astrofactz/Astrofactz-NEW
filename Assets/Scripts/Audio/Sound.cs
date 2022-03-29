using UnityEngine.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "Sound", menuName = "ScriptableObjects/Sound")]
public class Sound : ScriptableObject
{
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(0.5f, 2f)]
    public float pitch;

    public bool doesLoop;

    public AudioMixerGroup outputChannel;

    [HideInInspector]
    public AudioSource srce;
}
