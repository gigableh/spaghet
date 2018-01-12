using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OscillateUpAndDown : MonoBehaviour
{
    public float frequency;
    public float amplitude;

    Vector3 initialPosition;

    void Awake()
    {
        initialPosition = transform.position;
    }

    void Update()
    {
        transform.position = initialPosition + Vector3.up * Mathf.Sin(Time.time * frequency) * amplitude;
    }
}
