using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{
    public Transform spaghetTarget;
    public AnimationCurve curve;
    public float duration = 10f;
    public GameScript gameScript;
    public Transform bowlTracking;

    bool startTimeSet = false;
    float startTime;

    Vector3 startPosition;

    bool isHandThatTouched = false;
    public Vector3 deltaFromBowlTrackingOnGameOver;
    Vector3 initialScale;

    void Awake()
    {
        initialScale = transform.localScale;
    }

    void Update()
    {
        if (gameScript.isGameOver)
        {
            if (!isHandThatTouched)
            {
                // If it's not the hand that touched the bowl, remove it.
                Destroy(gameObject);
                return;
            }
            else
            {
                // Update tracking as animation kicks in.
                transform.position = bowlTracking.position + deltaFromBowlTrackingOnGameOver * bowlTracking.localScale.x;
                transform.localScale = initialScale * bowlTracking.localScale.x;
            }
        }
        else if (spaghetTarget != null)
        {
            // Will start tracking time and position when the spaghet target is set.
            if (!startTimeSet)
            {
                startTime = Time.time;
                startPosition = transform.position;
                startTimeSet = true;
            }

            float elapsedTime = Time.time - startTime;
            float normElapsedTime = elapsedTime / duration;
            float curveVal = curve.Evaluate(normElapsedTime);

            Vector3 delta = spaghetTarget.position - transform.position;
            Vector3 dir = delta.normalized;
            transform.rotation = transform.rotation * Quaternion.FromToRotation(transform.right, dir);
            //transform.position += dir * (speed * Time.deltaTime);
            transform.position = Vector3.Lerp(startPosition, spaghetTarget.position, curveVal);
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

    public void SelfDestruct()
    {
        Destroy(gameObject);
    }
}
