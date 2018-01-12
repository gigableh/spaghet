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

    public void PlayOneShot(AudioClip clip)
    {
        audioSources[nextAudioSourceIndex].Stop();
        audioSources[nextAudioSourceIndex].pitch = 1;
        audioSources[nextAudioSourceIndex].PlayOneShot(clip);
        nextAudioSourceIndex = (nextAudioSourceIndex + 1) % audioSourceCount;
    }

    public void PlayOneShot(AudioClip clip, float pitch)
    {
        audioSources[nextAudioSourceIndex].Stop();
        audioSources[nextAudioSourceIndex].pitch = pitch;
        audioSources[nextAudioSourceIndex].PlayOneShot(clip);
        nextAudioSourceIndex = (nextAudioSourceIndex + 1) % audioSourceCount;
    }
}
