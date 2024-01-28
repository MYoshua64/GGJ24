using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class QuitHandler : MonoBehaviour
{
    [SerializeField] Image statusImage;
    [SerializeField] float timeToReturn = 1f;

    CanvasGroup group;
    bool isShowing;

    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Cancel") && !isShowing)
        {
            isShowing = true;
            HoldForMenu();
        }
    }

    async Task HoldForMenu()
    {
        isShowing = true;
        group.DOFade(1, 0.35f);
        float timeCount = 0f;

        while (Input.GetButton("Cancel") && timeCount < timeToReturn)
        {
            timeCount += Time.deltaTime;
            statusImage.fillAmount = timeCount / timeToReturn;
            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }

        if (timeCount >= timeToReturn)
        {
            //Load into main menu
            ScreenFader.instance.FadeWithLevelLoad(0);
        }
        else
        {
            group.DOFade(0f, 0.35f).OnComplete(() =>
            {
                isShowing = false;
                statusImage.fillAmount = 0f;
            });
        }
    }
}
