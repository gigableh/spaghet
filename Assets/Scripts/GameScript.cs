using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject handPrefab;
    public VideoPlayerController videoPlayerController;
    public Transform spaghetTarget;
    public Transform bowlTracking;

    [Header("Game Config / Info")]
    public bool isGameOver = false;
    public int handsSlapped = 0;

    [Header("Hand Config")]
    public float handEnterSpeed = 15f;
    public float handLeaveSpeed = 5f;
    public float handSpawnDistance = 9f; // Calculate this from camera size.

    void Start()
    {
        StartCoroutine(SpawnRandomHandLoop());
    }

    void SpawnRandomHand()
    {
        var handGob = Instantiate(handPrefab, transform);
        handGob.transform.position = (Quaternion.AngleAxis(Random.value * 359.9f, Vector3.forward) * Vector3.right) * handSpawnDistance;

        var hc = handGob.GetComponent<HandController>();

        // Pass a few things to each hand controller.
        hc.spaghetTarget = spaghetTarget;
        hc.gameScript = this;
        hc.bowlTracking = bowlTracking;
        hc.enterSpeed = handEnterSpeed;
        hc.leaveSpeed = handLeaveSpeed;

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
