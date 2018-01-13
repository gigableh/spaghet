using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextButton : MonoBehaviour
{
    [Header("Dependencies")]
    public GoodAudioManager audioManager;
    public AudioClip slapSound;
    public TMP_Text buttonText;

    [Header("General Settings")]
    public float lerpChangePerSec = 0.1f;
    float currentLerpValue = 0f;

    [Header("Normal Settings")]
    public Vector3 normalScale = Vector3.one;
    public Color normalColor = Color.grey;

    [Header("Hover Settings")]
    public Vector3 hoverScale = Vector3.one * 2f;
    public Color hoverColor = Color.white;

    bool isInteractive = true;

    void Update()
    {
        if (isInteractive)
        {
            Vector2 mousePos = Input.mousePosition;
            if (RectTransformUtility.RectangleContainsScreenPoint(buttonText.rectTransform, mousePos, Camera.main))
            {
                currentLerpValue = Mathf.Clamp01(currentLerpValue + lerpChangePerSec * Time.deltaTime);
                if (Input.GetMouseButtonDown(0))
                {
                    audioManager.PlayOneShot(slapSound);
                }
            }
            else
            {
                currentLerpValue = Mathf.Clamp01(currentLerpValue - lerpChangePerSec * Time.deltaTime);
            }

            buttonText.color = Color.Lerp(normalColor, hoverColor, currentLerpValue);
            buttonText.rectTransform.localScale =  Vector3.Lerp(normalScale, hoverScale, currentLerpValue);
        }
    }

    public void SetInteractive(bool b)
    {
        isInteractive = b;
    }

    public TMP_Text GetText()
    {
        return buttonText;
    }
}
