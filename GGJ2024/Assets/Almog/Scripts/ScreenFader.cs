using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour
{
    [SerializeField] Material fadeMat;
    float fadeAmount = 1;

    // Start is called before the first frame update
    void Start()
    {
        Sequence fadeSequence = DOTween.Sequence();
        fadeSequence.Append(
        DOTween.To(() => fadeAmount, x => fadeAmount = x, 0, 0.4f)
            .OnUpdate(() =>
            {
                fadeMat.SetFloat("_FadeAmount", fadeAmount);
                Debug.Log(fadeAmount);
            }));
        fadeSequence.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
