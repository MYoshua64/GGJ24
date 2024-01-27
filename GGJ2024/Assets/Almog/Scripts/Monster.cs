using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster : ControlledEntity
{
    [Header("Monster")]
    [SerializeField] private GameObject deathParticles;
    [SerializeField] public SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip deathClip;
    [SerializeField] private Sprite eyes1, eyes2;
    [SerializeField] float blinkInterval = 5;
    public Sequence sequence;
    protected override void Start()
    {
        base.Start();
        sequence = DOTween.Sequence()
            .AppendCallback(() => { spriteRenderer.sprite = eyes2;}).AppendInterval(0.5f)
            .Append(spriteRenderer.transform.DOScale(spriteRenderer.transform.lossyScale * 1.15f, 0.35f)
            .SetEase(Ease.InExpo).OnComplete(() => { spriteRenderer.sprite = eyes1;})
        ).AppendInterval(blinkInterval).SetLoops(-1, LoopType.Yoyo);

    }

    // Update is called once per frame
    void Update()
    {
        if (!inControl) return;
        HandleMovement();
        if (isInLight && inControl)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        inControl = false;
        Debug.Log("Game Over");
        
        // TODO: add sound
        AudioSource.PlayClipAtPoint(deathClip, transform.position);

        deathParticles.SetActive(true);
        DOTween.Sequence()
            .Append(spriteRenderer.DOColor(Color.black, 1).SetEase(Ease.InCubic))
            .Join(transform.DOShakeScale(0.5f, transform.lossyScale * 1.01f))
            .Join(transform.DOShakePosition(1f, 0.15f))
            .Join(transform.DOShakeRotation(1f, 0.5f)).AppendInterval(1).OnComplete(() =>
            {
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            });
    }
}
