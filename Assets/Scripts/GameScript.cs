using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    public GameObject handPrefab;
    public VideoPlayerController videoPlayerController;
    public AnimationCurve[] curves;
    public Transform spaghetTarget;
    public float handHomingBaseDuration = 2f;
    public float spawnDistance = 9f;
    public Transform bowlTracking;

    public bool isGameOver = false;

    void Start()
    {
        StartCoroutine(SpawnRandomHandLoop());
    }

    void SpawnRandomHand()
    {
        var handGob = Instantiate(handPrefab, transform);
        handGob.transform.position = (Quaternion.AngleAxis(Random.value * 359.9f, Vector3.forward) * Vector3.right) * spawnDistance;
        AnimationCurve randomCurve = curves[Random.Range(0, curves.Length)];

        var hc = handGob.GetComponent<HandController>();

        // Pass a few things to each hand controller.
        hc.curve = randomCurve;
        hc.spaghetTarget = spaghetTarget;
        hc.gameScript = this;
        hc.bowlTracking = bowlTracking;

        // Also update rotation before the next frame.
        hc.UpdateRotation();
    }

    IEnumerator SpawnRandomHandLoop()
    {
        if (isGameOver) yield break;
        SpawnRandomHand();
        yield return new WaitForSeconds(Random.value * 3.5f + 0.5f);
        StartCoroutine(SpawnRandomHandLoop());
    }

    public void ExecuteGameOverSequence()
    {
        Debug.Log("Game is over!");
        isGameOver = true;
        videoPlayerController.Play();
    }
}
