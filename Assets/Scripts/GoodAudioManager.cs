using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoodAudioManager : MonoBehaviour
{
    public int audioSourceCount = 2;
    AudioSource[] audioSources;
    public int nextAudioSourceIndex = 0;

    void Awake()
    {
        audioSources = new AudioSource[audioSourceCount];
        for (int i = 0; i < audioSources.Length; i++)
        {
            audioSources[i] = gameObject.AddComponent<AudioSource>();
            audioSources[i].playOnAwake = false;
        }
    }

    public AudioSource PlayOneShot(AudioClip clip)
    {
        AudioSource aus = audioSources[nextAudioSourceIndex];
        aus.Stop();
        aus.pitch = 1;
        aus.PlayOneShot(clip);
        nextAudioSourceIndex = (nextAudioSourceIndex + 1) % audioSourceCount;
        return aus;
    }

    public AudioSource PlayOneShot(AudioClip clip, float pitch)
    {
        AudioSource aus = audioSources[nextAudioSourceIndex];
        aus.Stop();
        aus.pitch = pitch;
        aus.PlayOneShot(clip);
        nextAudioSourceIndex = (nextAudioSourceIndex + 1) % audioSourceCount;
        return aus;
    }
}
