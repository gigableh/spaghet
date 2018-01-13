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

    public TMP_Text gameOverText;
    public TMP_Text timesSpaghetText;
    public TMP_Text slapCountText;
    TMP_Text buttonText;
    Image panelImage;

    [Header("General Settings")]
    public float fadeDurationSec = 0.8f;

    // Moving color values.
    Color gameOverTextColor;
    Color timesSpaghetTextColor;
    Color slapCountTextColor;
    Color buttonTextColor;
    Color panelImageColor;

    // Initial alpha values.
    float iaGameOverText;
    float iaTimesSpaghetText;
    float iaSlapCountText;
    float iaButtonText;
    float iaPanelImage;

    void Awake()
    {
        panel.SetActive(false);
        buttonText = playAgainButton.GetText();
        panelImage = panel.GetComponent<Image>();
        InitGlobalColorVariables();
    }

    void InitGlobalColorVariables()
    {
        gameOverTextColor = gameOverText.color;
        timesSpaghetTextColor = timesSpaghetText.color;
        slapCountTextColor = slapCountText.color;
        buttonTextColor = buttonText.color;
        panelImageColor = panelImage.color;

        iaGameOverText = gameOverTextColor.a;
        iaTimesSpaghetText = timesSpaghetTextColor.a;
        iaSlapCountText = slapCountTextColor.a;
        iaButtonText = buttonTextColor.a;
        iaPanelImage = panelImageColor.a;
    }

    void SetAlphaValueStrengths(float ratio)
    {
        gameOverTextColor.a = ratio * iaGameOverText;
        timesSpaghetTextColor.a = ratio * iaTimesSpaghetText;
        slapCountTextColor.a = ratio * iaSlapCountText;
        buttonTextColor.a = ratio * iaButtonText;
        panelImageColor.a = ratio * iaPanelImage;

        gameOverText.color = gameOverTextColor;
        timesSpaghetText.color = timesSpaghetTextColor;
        slapCountText.color = slapCountTextColor;
        buttonText.color = buttonTextColor;
        panelImage.color = panelImageColor;
    }

    [ContextMenu("Fade In")]
    public void ExecuteFadeIn(int slapCount)
    {
        StartCoroutine(FadeIn(slapCount));
    }

    public IEnumerator FadeIn(int slapCount)
    {
        playAgainButton.SetInteractive(false);
        slapCountText.text = slapCount.ToString();

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
