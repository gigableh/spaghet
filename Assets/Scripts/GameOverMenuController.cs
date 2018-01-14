using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOverMenuController : MonoBehaviour
{
    [Header("Dependencies")]
    public GameObject panel;
    public TextButton playAgainButton;
    public TMP_Text slapCountText;
    public TMP_Text survivalTimeText;
    public TMP_Text[] textObjects;

    TMP_Text buttonText;
    Image panelImage;

    [Header("General Settings")]
    public float fadeDurationSec = 0.8f;

    // Moving color values.
    Color[] textObjectColors;
    Color buttonTextColor;
    Color panelImageColor;

    // Initial alpha values.
    float[] textObjectIAs;
    float buttonTextIA;
    float panelImageIA;

    void Awake()
    {
        panel.SetActive(false);
        buttonText = playAgainButton.GetText();
        panelImage = panel.GetComponent<Image>();
        InitGlobalColorVariables();
    }

    void InitGlobalColorVariables()
    {
        // Initialize color vars and IAs for text objects.
        textObjectColors = new Color[textObjects.Length];
        textObjectIAs = new float[textObjects.Length];
        for (int i = 0; i < textObjects.Length; ++i)
        {
            textObjectColors[i] = textObjects[i].color;
            textObjectIAs[i] = textObjectColors[i].a;
        }

        // Initialize color vars and IAs for other unique objects.
        buttonTextColor = buttonText.color;
        panelImageColor = panelImage.color;
        buttonTextIA = buttonTextColor.a;
        panelImageIA = panelImageColor.a;
    }

    void SetAlphaValueStrengths(float ratio)
    {
        // Calculate and set colors for text objects.
        for (int i = 0; i < textObjects.Length; ++i)
        {
            textObjectColors[i].a = textObjectIAs[i] * ratio;
            textObjects[i].color = textObjectColors[i];
        }

        // Calculate and set colors for other unique objects.
        buttonTextColor.a = buttonTextIA * ratio;
        panelImageColor.a = panelImageIA * ratio;
        buttonText.color = buttonTextColor;
        panelImage.color = panelImageColor;
    }

    [ContextMenu("Fade In")]
    public void ExecuteFadeIn(int slapCount, float timeSurvived)
    {
        StartCoroutine(FadeIn(slapCount, timeSurvived));
    }

    public IEnumerator FadeIn(int slapCount, float timeSurvived)
    {
        playAgainButton.SetInteractive(false);
        slapCountText.text = slapCount.ToString();
        survivalTimeText.text = ((int)timeSurvived).ToString();

        float startTime = Time.time;
        float elapsedTime = 0f;

        SetAlphaValueStrengths(0f);

        panel.SetActive(true);

        while ((elapsedTime = Time.time - startTime) < fadeDurationSec)
        {
            SetAlphaValueStrengths(elapsedTime / fadeDurationSec);
            yield return null;
        }

        SetAlphaValueStrengths(1f);

        playAgainButton.SetInteractive(true);
    }

    [ContextMenu("Fade Out")]
    public void ExecuteFadeOut()
    {
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeOut()
    {
        playAgainButton.SetInteractive(false);

        float startTime = Time.time;
        float elapsedTime = 0f;

        SetAlphaValueStrengths(1f);

        while ((elapsedTime = Time.time - startTime) < fadeDurationSec)
        {
            SetAlphaValueStrengths(1f - elapsedTime / fadeDurationSec);
            yield return null;
        }

        SetAlphaValueStrengths(0f);

        panel.SetActive(false);
    }
}
