﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject handPrefab;
    public VideoPlayerController videoPlayerController;
    public Transform spaghetTarget;
    public Transform bowlTracking;
    public GoodAudioManager audioManager;

    [Header("Game Config / Info")]
    public bool isGameOver = false;
    public int handsSlapped = 0;
    [Range(0f, 170f)]
    public float angleSpawnBuffer = 30f;
    public float handSpawnsPerSec = 1f;
    public float handSpawnsIncreasePerSec = 1f;
    public float handSpawnsPerSecLimit = 5f;
    float handSpawnsPerSecWithIncrease;

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

    void Start()
    {
        handEnterSpeedWithIncrease = handEnterSpeed;
        handSpawnsPerSecWithIncrease = handSpawnsPerSec;
        CalculateHandSpawnDistance();
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
        var handGob = Instantiate(handPrefab, transform);
        float halfCompressedAngleRange = 180f - angleSpawnBuffer;
        float fullCompressedAngleRange = halfCompressedAngleRange * 2f;
        float angle = Random.value * fullCompressedAngleRange - halfCompressedAngleRange;
        angle += Mathf.Sign(angle) * angleSpawnBuffer;
        handGob.transform.position = ( lastSpawnDirection = Quaternion.AngleAxis(angle, Vector3.forward) * lastSpawnDirection ) * handSpawnHalfDistance;

        var hc = handGob.GetComponent<HandController>();

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
    }
   
    public void ExecuteGameOverSequence()
    {
        Debug.Log("Game is over!");
        isGameOver = true;
        videoPlayerController.Play();
    }
}
