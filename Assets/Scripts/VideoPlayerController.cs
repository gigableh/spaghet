using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public GameObject background;
    public Animator animator;
    VideoPlayer vp;

    void Awake()
    {
        vp = GetComponent<VideoPlayer>();
    }

    void OnEnable()
    {
        vp.Prepare();
    }

    [ContextMenu("Play")]
    public void Play()
    {
        vp.Play();
        StartCoroutine(DisableBackgroundDelayed());
    }

    IEnumerator DisableBackgroundDelayed()
    {
        while (vp.frame < 3) yield return null;
        animator.Play("Bowl Tracking");
        background.SetActive(false);
    }

    public bool IsPlaying()
    {
        return vp.isPlaying;
    }

    public void Reset()
    {
        background.SetActive(true);
        animator.Rebind();
        vp.Stop();
        StartCoroutine(DelayedResetActions());
    }

    IEnumerator DelayedResetActions()
    {
        yield return null;
        vp.Prepare();
    }
}
