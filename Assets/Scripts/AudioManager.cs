using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioMixerGroup mixerGroup;
    public AudioClip sound;
    [Range(0, 1)]
    public float volume = 1f;
    [Range(0.1f, 3f)]
    public float pitch = 1f;
    public bool loop = false;
    [HideInInspector]
    public AudioSource source;

    internal void SetSource(AudioSource source)
    {
        this.source = source;
        source.volume = this.volume;
        source.clip = this.sound;
        source.pitch = this.pitch;
        source.loop = this.loop;
        source.playOnAwake = false;
        source.outputAudioMixerGroup = this.mixerGroup;
    }
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    Sound currentMusic;

    [SerializeField]
    public Sound[] sounds;


    void Awake()
    {

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        //DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds) {
            s.SetSource(gameObject.AddComponent<AudioSource>());
        }
    }


    public void Play(string name)
    {
        Sound s = System.Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
    }


    public void PlayMusic(string name, bool playImmediately = true)
    {
        if (currentMusic != null) {
            if (currentMusic.name == name) // don't change
            {
                if (!currentMusic.source.isPlaying)
                    currentMusic.source.Play();
                return;
            }
            currentMusic.source.Stop();
            currentMusic.source.time = 0;

        }
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s != null)
            s.source.Play();
        currentMusic = s;
    }

    public void MusicSettingToggled(bool status, string name)
    {
        if (status) {
            PlayMusic(name);
        }
        else {
            currentMusic.source.Pause();
        }
    }
}
