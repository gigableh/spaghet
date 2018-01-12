using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Transform spaghetTarget;
    public GameScript gameScript;
    public Transform bowlTracking;

    public float enterSpeed = 0;
    public float leaveSpeed = 0;

    public GoodAudioManager audioManager;
    public AudioClip slapSound;
    public float handSlapSoundPitchMaxChange;
    public float randomSlapPitchBias;

    bool isHandThatTouched = false;
    public Vector3 deltaFromBowlTrackingOnGameOver;
    Vector3 initialScale;

    bool isHandLeaving = false;

    public HandSpawner handSpawner;
    public bool isHandActive = false;

    void Awake()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (!isHandActive) return;

        if (gameScript.isGameOver)
        {
            if (!isHandThatTouched)
            {
                if (!isHandLeaving)
                {
                    StartCoroutine(HandLeaveSequence());
                }
                return;
            }
            else
            {
                // Update tracking as animation kicks in.
                transform.position = bowlTracking.position + deltaFromBowlTrackingOnGameOver * bowlTracking.localScale.x;
                transform.localScale = initialScale * bowlTracking.localScale.x;
            }
        }
        else if (spaghetTarget != null && !isHandLeaving)
        {
            // Update position
            // Note: rotation should already be set by GameScript so the +X axis should point
            // toward the spaghet.
            transform.position = transform.position + transform.right * (enterSpeed * Time.deltaTime);
        }
    }

    public void UpdateRotation()
    {
        Vector3 delta = spaghetTarget.position - transform.position;
        Vector3 dir = delta.normalized;
        transform.rotation = transform.rotation * Quaternion.FromToRotation(transform.right, dir);
    }

    public void MarkHandThatTouched(bool b)
    {
        isHandThatTouched = b;
    }

    public void CalculateGameOverAnimationComponents()
    {
        deltaFromBowlTrackingOnGameOver = transform.position - bowlTracking.position;
    }

    public void SlapHand()
    {
        if (!isHandLeaving)
        {
            ++gameScript.handsSlapped;
            float randomPitch = 1f + (Random.value - (1f - randomSlapPitchBias)) * handSlapSoundPitchMaxChange;
            audioManager.PlayOneShot(slapSound, randomPitch);
            StartCoroutine(HandLeaveSequence());
        }
    }

    IEnumerator HandLeaveSequence()
    {
        isHandLeaving = true;
        while (!IsHandOutsideOfCameraView())
        {
            transform.position = transform.position - transform.right * (leaveSpeed * Time.deltaTime);
            yield return null;
        }
        ReturnToPool();
    }

    bool IsHandOutsideOfCameraView()
    {
        // Don't count as outside right on the edge.
        // the hand pivot is not at the tip of the hand.
        float bufferUnits = 1f;

        // Get necessary camera info.
        Camera mainCam = Camera.main;
        float camHalfSizeY = mainCam.orthographicSize;
        float camHalfSizeX = camHalfSizeY * mainCam.aspect;
        Vector3 camPos = mainCam.transform.position;

        // Calculate the displacement from the camera to the hand.
        Vector3 camToHandDisp = transform.position - camPos;

        // Use the info to determine if hand is outside the camera's view.
        return ( Mathf.Abs(camToHandDisp.y) > camHalfSizeY + bufferUnits ) ||
            ( Mathf.Abs(camToHandDisp.x) > camHalfSizeX + bufferUnits );
    }

    public void Activate()
    {
        isHandActive = true;
        gameObject.SetActive(true);
    }

    public void ReturnToPool()
    {
        gameObject.SetActive(false);
        isHandActive = false;
        ResetState();
    }

    void ResetState()
    {
        isHandThatTouched = false;
        isHandLeaving = false;
    }
}
