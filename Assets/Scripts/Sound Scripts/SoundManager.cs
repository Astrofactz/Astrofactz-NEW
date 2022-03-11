using UnityEngine.Audio;
using UnityEngine;
using System;

public class SoundManager : MonoBehaviour
{
    public Sound[] sounds;

    public static SoundManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.srce = gameObject.AddComponent<AudioSource>();
            s.srce.clip = s.clip;

            s.srce.volume = s.volume;
            s.srce.pitch = s.pitch;

            s.srce.loop = s.doesLoop;

            s.srce.outputAudioMixerGroup = s.outputChannel;
        }
    }

    //Plays the selected sound
    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found. You complete buffoon.");
            return;
        }
        s.srce.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " not found. You stinkyhead.");
            return;
        }
        s.srce.Stop();
    }
}