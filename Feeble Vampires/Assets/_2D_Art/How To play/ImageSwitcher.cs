using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwitcher : MonoBehaviour
{
    public Image[] images;
    public Button leftButton;
    public Button rightButton;
    public float fadeDuration = 0.5f;
    public Button returnButton;
    private int currentIndex = 0;

    void Start()
    {
        UpdateUI(instant: true);

        leftButton.onClick.AddListener(() => { AnimateButton(leftButton); GoLeft(); });
        rightButton.onClick.AddListener(() => { AnimateButton(rightButton); GoRight(); });

        returnButton.gameObject.SetActive(false);
        returnButton.onClick.AddListener(ReturnToMainScene);
    }

    void GoLeft()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdateUI();
        }
    }

    void GoRight()
    {
        if (currentIndex < images.Length - 1)
        {
            currentIndex++;
            UpdateUI();
        }
    }

    void UpdateUI(bool instant = false)
    {
        StopAllCoroutines();

        for (int i = 0; i < images.Length; i++)
        {
            if (i == currentIndex)
            {
                if (instant)
                {
                    SetAlpha(images[i], 1f);
                    images[i].gameObject.SetActive(true);
                }
                else
                {
                    images[i].gameObject.SetActive(true);
                    StartCoroutine(FadeImage(images[i], 1f));
                }
            }
            else
            {
                if (instant)
                {
                    SetAlpha(images[i], 0f);
                    images[i].gameObject.SetActive(false);
                }
                else
                {
                    StartCoroutine(FadeOutAndDisable(images[i]));
                }
            }
        }

        leftButton.gameObject.SetActive(currentIndex != 0);
        rightButton.gameObject.SetActive(currentIndex != images.Length - 1);

        returnButton.gameObject.SetActive(currentIndex == images.Length - 1);
    }

    IEnumerator FadeImage(Image img, float targetAlpha)
    {
        float startAlpha = img.color.a;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            float alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            SetAlpha(img, alpha);
            yield return null;
        }
        SetAlpha(img, targetAlpha);
    }

    IEnumerator FadeOutAndDisable(Image img)
    {
        yield return FadeImage(img, 0f);
        img.gameObject.SetActive(false);
    }

    void SetAlpha(Image img, float alpha)
    {
        Color c = img.color;
        c.a = alpha;
        img.color = c;
    }

    void AnimateButton(Button btn)
    {
        StartCoroutine(ButtonBounce(btn.transform));
    }

    IEnumerator ButtonBounce(Transform btnTransform)
    {
        Vector3 originalScale = btnTransform.localScale;
        Vector3 smallerScale = originalScale * 0.9f;

        float shrinkTime = 0.1f;
        float time = 0f;
        while (time < shrinkTime)
        {
            time += Time.deltaTime;
            btnTransform.localScale = Vector3.Lerp(originalScale, smallerScale, time / shrinkTime);
            yield return null;
        }
        btnTransform.localScale = smallerScale;

        float growTime = 0.1f;
        time = 0f;
        while (time < growTime)
        {
            time += Time.deltaTime;
            btnTransform.localScale = Vector3.Lerp(smallerScale, originalScale, time / growTime);
            yield return null;
        }
        btnTransform.localScale = originalScale;
    }

    void ReturnToMainScene()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Title Screen");
    }
}


