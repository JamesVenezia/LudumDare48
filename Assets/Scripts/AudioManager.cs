using UnityEngine.Audio;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{

    [System.Serializable]
    public class Sound
    {
        public string clipName;

        public AudioClip clip;

        [Range(0f, 1f)]
        public float volume = 1f;

        [Range(0f, 3f)]
        public float pitch = 1f;

        [HideInInspector]
        public AudioSource source;

        public bool loop = false;


    }


    public Sound[] sounds;

    public static AudioManager instance;


    

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        

        foreach( Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }
    }

    public void Play(string name)
    {

        Sound s = Array.Find(sounds, sound => sound.clipName.ToLower() == name.ToLower());

        if (s == null)
        {
            Debug.Log("Audio clip: " + name + " not found.");
            return;
        }
        
        s.source.Play();

    }

}
