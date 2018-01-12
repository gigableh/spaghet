using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public TMP_Text titleText;
    public GameObject playButton;
    public Image menuBackgroundImage;

    public IEnumerator FadeOutSequence(float fadeOutDuration)
    {
        titleText.enabled = false;
        playButton.SetActive(false);

        float startTime = Time.time;
        float elapsedTime = 0;

        Color c = menuBackgroundImage.color;
        while ((elapsedTime = Time.time - startTime) < fadeOutDuration)
        {
            c.a = 1f - elapsedTime / fadeOutDuration;
            menuBackgroundImage.color = c;
            yield return null;
        }

        gameObject.SetActive(false);
    }
}
