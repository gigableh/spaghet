﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    [Header("Dependencies")]
    public VideoPlayerController videoPlayerController;
    public Transform spaghetTarget;
    public Transform bowlTracking;
    public GoodAudioManager audioManager;
    public HandSpawner handSpawner;
    public MainMenuController mainMenu;
    public GameOverMenuController gameOverMenu;

    [Header("Game Config / Info")]
    [HideInInspector]
    public bool isGameOver = false;
    [HideInInspector]
    public int handsSlapped = 0;
    [Range(0f, 170f)]
    public float angleSpawnBuffer = 30f;
    public float handSpawnsPerSec = 1f;
    public float handSpawnsIncreasePerSec = 1f;
    public float handSpawnsPerSecLimit = 5f;
    float handSpawnsPerSecWithIncrease;
    public float mainMenuFadeOutSec = 1f;
    float roundTime; // First set with start time, then on game over has round duration.

    [Header("General Audio")]
    public AudioClip spaghetSquishSound;

    [Header("Hand Config")]
    public float handEnterSpeed = 15f;
    public float handLeaveSpeed = 5f;
    public float handSpeedIncreasePerSec = 1f;
    public float handSpeedLimit = 25f;
    float handEnterSpeedWithIncrease;

    public float handSpawnDistancePad = 1f;
    float handSpawnHalfDistance = 0f; // Calculate from camera size.
    Vector3 lastSpawnDirection = Vector3.right;

    [Header("Hand Config: Audio")]
    public AudioClip[] handSlapSounds;
    public int currentHandSlapSound = 0;
    public float likelyhoodOfSwitchingSlapSound = 0.8f;
    public float handSlapSoundPitchMaxChange = 1f;
    [Range(0f, 1f), Tooltip("0 is all lower pitches, 1 is all higher pitches")]
    public float randomSlapPitchBias = 0.5f;

    void Awake()
    {
        handEnterSpeedWithIncrease = handEnterSpeed;
        handSpawnsPerSecWithIncrease = handSpawnsPerSec;
        CalculateHandSpawnDistance();
    }

    public void StartGame()
    {
        StartCoroutine(mainMenu.FadeOutSequence(mainMenuFadeOutSec));
        StartCoroutine(HandSpawnerLoop());
    }

    void CalculateHandSpawnDistance()
    {
        Camera mainCam = Camera.main;
        float yHalfSize = mainCam.orthographicSize;
        float xHalfSize = yHalfSize * mainCam.aspect;
        float diagonalHalfSize = Mathf.Sqrt(yHalfSize*yHalfSize + xHalfSize*xHalfSize);
        handSpawnHalfDistance = diagonalHalfSize + handSpawnDistancePad;
    }

    void Update()
    {
        handEnterSpeedWithIncrease = Mathf.Clamp(handEnterSpeedWithIncrease + Time.deltaTime * handSpeedIncreasePerSec, handEnterSpeed, handSpeedLimit);
        handSpawnsPerSecWithIncrease = Mathf.Clamp(handSpawnsPerSecWithIncrease + Time.deltaTime * handSpawnsIncreasePerSec, handSpawnsPerSec, handSpawnsPerSecLimit);
    }

    IEnumerator HandSpawnerLoop()
    {
        roundTime = Time.time;
        while (!isGameOver)
        {
            float startTime = Time.time;

            // Wait for time to spawn new hand.
            while (Time.time - startTime < 1f / handSpawnsPerSecWithIncrease)
            {
                if (isGameOver) yield break; // Break out early if game over while waiting.
                yield return null; // Otherwise keep waiting.
            }

            if (isGameOver) yield break; // Don't spawn if just happened to become game over.
            SpawnRandomHand(); // If we got to here spawn the hand.
        }
    }

    void SpawnRandomHand()
    {
        var hc = handSpawner.GetHand();
        if (hc == null)
        {
            Debug.LogWarning("GameScript:: Could not spawn random hand, pool returned null.");
            return;
        }

        float halfCompressedAngleRange = 180f - angleSpawnBuffer;
        float fullCompressedAngleRange = halfCompressedAngleRange * 2f;
        float angle = Random.value * fullCompressedAngleRange - halfCompressedAngleRange;
        angle += Mathf.Sign(angle) * angleSpawnBuffer;
        hc.transform.position = ( lastSpawnDirection = Quaternion.AngleAxis(angle, Vector3.forward) * lastSpawnDirection ) * handSpawnHalfDistance;

        // Pass a few things to each hand controller.
        hc.spaghetTarget = spaghetTarget;
        hc.gameScript = this;
        hc.bowlTracking = bowlTracking;
        hc.enterSpeed = handEnterSpeedWithIncrease;
        hc.leaveSpeed = handLeaveSpeed;

        // Pass audio things to hand controller.
        hc.audioManager = audioManager;
        hc.slapSound = handSlapSounds[currentHandSlapSound];
        hc.handSlapSoundPitchMaxChange = handSlapSoundPitchMaxChange;
        hc.randomSlapPitchBias = randomSlapPitchBias;
        // Change to next slap sound on chance.
        if (Random.value < likelyhoodOfSwitchingSlapSound)
            currentHandSlapSound = (++currentHandSlapSound) % handSlapSounds.Length;

        // Also update rotation before the next frame.
        hc.UpdateRotation();
        hc.Activate();
    }
   
    public void ExecuteGameOverSequence()
    {
        isGameOver = true;
        roundTime = Time.time - roundTime;
        StartCoroutine(GameOverSequence());
    }

    IEnumerator GameOverSequence()
    {
        AudioSource aus = audioManager.PlayOneShot(spaghetSquishSound);
        while (aus.isPlaying) yield return null;
        videoPlayerController.Play();
        while (videoPlayerController.IsPlaying()) yield return null;
        gameOverMenu.ExecuteFadeIn(handsSlapped, roundTime);
    }

    public void RestartGame()
    {
        gameOverMenu.ExecuteFadeOut();
        videoPlayerController.Reset();
        handSpawner.ResetAll();
        handsSlapped = 0;
        handEnterSpeedWithIncrease = handEnterSpeed;
        handSpawnsPerSecWithIncrease = handSpawnsPerSec;
        isGameOver = false;
        StartCoroutine(HandSpawnerLoop());
    }
}
