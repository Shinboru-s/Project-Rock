using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

[System.Serializable]
public class Sound
{
    public string name;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;

    [Range(.1f, 3f)]
    public float pitch;

    public bool loop;

    [HideInInspector]
    public AudioSource source;
}

public class AudioManager : MonoBehaviour
{
    public Sound[] sounds;
    private AudioSource[] audios;
    private List<string> randomSFX = new List<string>();


    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);

            _instance = this;
        }

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;

        }


        try
        {
            Play("Theme");
        }
        catch
        {

            Debug.Log("Theme not found");
        }

        audios = gameObject.GetComponents<AudioSource>();
        foreach (var item in audios)
        {
            Debug.Log(item.clip.name);
        }

        FindRandomSFX();
        StartCoroutine(PlayRandomSFX());
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.Log("bu isimde ses dosyasi bulunamadi: " + name);
            return;

        }
        s.source.Play();
    }
    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        s.source.Stop();
    }


    public void VolumeUpdate(float volume, bool isTheme)
    {
        //Debug.Log("Theme: " + isTheme + " Volume: " + volume);

        

        for (int i = 0; i < audios.Length; i++)
        {
            if (isTheme == true)
            {
                if (audios[i].clip.name == "Theme")
                    audios[i].volume = volume;
            }
            else if (audios[i].clip.name != "Theme")
                audios[i].volume = volume;

        }

    }

    public float GetAudioVolume(bool isTheme)
    {
        return 0f;

        for (int i = 0; i < audios.Length; i++)
        {
            if (isTheme == true)
            {
                if (audios[i].clip.name == "Theme")
                    return audios[i].volume;
            }
            else if (audios[i].clip.name != "Theme")
                return audios[i].volume;
            

        }
        return 0f;
    }

    private void FindRandomSFX()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].clip.name.Contains("Random"))
            {
                randomSFX.Add(sounds[i].name);
                Debug.Log("Random SFX added: " + sounds[i].name);
            }
            else
            {
                Debug.Log(sounds[i].clip.name + " Not Selected");
            }
        }
    }

    private void ChangePitch(string soundName)
    {
        Sound s = Array.Find(sounds, sound => sound.name == soundName);
        float pitch = UnityEngine.Random.Range(0.5f, 1.5f);

        s.source.pitch = pitch;
    }

    IEnumerator PlayRandomSFX()
    {
        yield return new WaitForSeconds(5f);

        while (true)
        {
            float waitSeconds = UnityEngine.Random.Range(7f, 13f);

            yield return new WaitForSeconds(waitSeconds);

            int sfxIndex = UnityEngine.Random.Range(0, randomSFX.Count);
            ChangePitch(randomSFX[sfxIndex]);
            Play(randomSFX[sfxIndex]);
        }   
    }

}
