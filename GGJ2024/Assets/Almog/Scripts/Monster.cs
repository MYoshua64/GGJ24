using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Monster : ControlledEntity
{
    [SerializeField] private GameObject deathParticles;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private AudioClip deathClip;

    // Update is called once per frame
    void Update()
    {
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
