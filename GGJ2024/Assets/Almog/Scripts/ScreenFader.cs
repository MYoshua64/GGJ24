using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    public static ScreenFader instance;

    private void Awake()
    {
        instance = this; 
    }

    Image panel;
    float alpha = 1;

    // Start is called before the first frame update
    void Start()
    {
        panel = GetComponent<Image>();
        Color panelColor = panel.color;
        panelColor.a = 1;
        panel.color = panelColor;
        Fade(0);
    }

    public void Fade(float targetAlpha)
    {
        Sequence fadeSequence = DOTween.Sequence();
        if (targetAlpha == 1)
        {
            panel.raycastTarget = true;
        }
        fadeSequence.Append(
        DOTween.To(() => alpha, x => alpha = x, targetAlpha, 1f)
            .OnUpdate(() =>
            {
                Color panelColor = panel.color;
                panelColor.a = alpha;
                panel.color = panelColor;
            }));
        if (targetAlpha == 0) 
        {
            fadeSequence.OnComplete(() =>
            {
                panel.raycastTarget = false;
            });
        }
        fadeSequence.Play();
    }

    public void FadeWithLevelLoad(int sceneIndex = 1)
    {
        panel.raycastTarget = true;
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Append(
        DOTween.To(() => alpha, x => alpha = x, 1, 1f)
            .OnUpdate(() =>
            {
                Color panelColor = panel.color;
                panelColor.a = alpha;
                panel.color = panelColor;
            })
            .OnComplete(() =>
            {
                SceneManager.LoadScene(sceneIndex);
            }));
    }
}
