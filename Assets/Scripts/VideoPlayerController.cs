using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public GameObject background;
    public Animator animator;
    public string videoName;
    public GameObject videoPlayerPrefab;
    GameObject vpGob;
    VideoPlayer vp;

    void Awake()
    {
        SpawnVideoPlayer();
    }

    void RespawnVideoPlayer()
    {
        if (vpGob != null) Destroy(vpGob);
        SpawnVideoPlayer();
    }

    void SpawnVideoPlayer()
    {
        vpGob = Instantiate(videoPlayerPrefab, transform);
        vp = vpGob.GetComponent<VideoPlayer>();
        ConfigureVideoPlayer();
    }

    void ConfigureVideoPlayer()
    {
        vp.targetCamera = Camera.main;
        vp.url = Application.streamingAssetsPath + "/" + videoName + ".mp4";
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
        if (vp == null) return false;
        return vp.isPlaying;
    }

    public void Reset()
    {
        background.SetActive(true);
        animator.Rebind();
        RespawnVideoPlayer();
        //StartCoroutine(DelayedResetActions());
    }

    IEnumerator DelayedResetActions()
    {
        yield return null;
    }
}
