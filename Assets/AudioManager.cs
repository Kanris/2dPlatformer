using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume = 0.5f;
    [Range(0.5f, 1.5f)]
    public float pitch = 1f;

    [Range(0f, 0.5f)]
    public float volumeOffset = 0.1f;
    [Range(0f, 0.5f)]
    public float pitchOffset = 0.1f;

    public bool loop = false;

    private AudioSource source;

    public void SetSource(AudioSource source)
    {
        this.source = source;
        this.source.clip = clip;
    }

    public void Play()
    {
        this.source.volume = volume * (1 + Random.Range(-volumeOffset / 2f, volumeOffset / 2f));
        this.source.pitch = pitch * (1 + Random.Range(-pitchOffset / 2f, pitchOffset / 2f));
        this.source.loop = loop;
        this.source.Play();
    }

    public void Stop()
    {
        this.source.Stop();
    }
}

public class AudioManager : MonoBehaviour {

    [SerializeField]
    Sound[] sounds;

    public static AudioManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
    }

    private void Start()
    {
        for (int index = 0; index < sounds.Length; index++)
        {
            GameObject go = new GameObject("Sound_" + index + "_" + sounds[index].name);
            go.transform.SetParent(this.transform);
            sounds[index].SetSource(go.AddComponent<AudioSource>());
        }

        PlaySound("Music");
    }

    public void PlaySound(string name)
    {
        var neededSound = sounds.First(x => x.name == name);

        if (neededSound != null)
        {
            neededSound.Play();
            return;
        }

        Debug.LogError("Audiomanager: can't found sound with name - " + name);
    }

    public void StopSound(string name)
    {
        var neededSound = sounds.First(x => x.name == name);

        if (neededSound != null)
        {
            neededSound.Stop();
            return;
        }

        Debug.LogError("Audiomanager: can't found sound with name - " + name);
    }
}
