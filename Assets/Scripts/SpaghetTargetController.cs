using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaghetTargetController : MonoBehaviour
{
    public GameScript gameScript;

    void OnTriggerEnter2D(Collider2D collider)
    {
        // Don't execute if already game over.
        if (gameScript.isGameOver) return;

        var hc = collider.GetComponent<HandController>();
        if (hc != null)
        {
            hc.MarkHandThatTouched(true);
            hc.CalculateGameOverAnimationComponents();
            gameScript.ExecuteGameOverSequence();
        }
    }
}
