using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class Child : ControlledEntity
{
    [Header("Child")]
    [SerializeField] float pushTime = 0.35f;
    [SerializeField] float pushSpeedMultiplier = 5f;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer unitedSpriteRenderer;
    [SerializeField] private SpriteRenderer alertIconRenderer;

    int scareCount;
    [SerializeField] private AudioClip gaspSFX, mgsClip, reuniteClip;
    public bool IsReunited { get; private set; }
    private Monster monster;
    private AudioSource footstepsAudio;

    int escapeAttempts;
    
    protected override void Start()
    {
        base.Start();
        IsReunited = false;
        isInLight = true;
        ShadowDetector.OnMonsterReunited += OnMonsterReunited;
        monster = FindObjectOfType<Monster>();
        footstepsAudio = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        ShadowDetector.OnMonsterReunited -= OnMonsterReunited;
    }

    private void OnMonsterReunited()
    {
        
        // TODO: Play sound
        GameObject rotationPoint = new GameObject("Rotation Point");
        Transform rotationPointTransform = rotationPoint.transform;
        Transform monsterTransform = monster.transform;
        rotationPointTransform.SetParent(transform);
        rotationPointTransform.localPosition = Vector3.zero;
        monster.enabled = false;
        inControl = false;

        Sequence sequence = DOTween.Sequence();
        sequence.Append(monsterTransform.DOMove(transform.position + Vector3.up * 2, 0.9f).SetEase(Ease.OutCirc).OnComplete(() =>
        {
            monsterTransform.SetParent(rotationPointTransform);
        }));
        sequence.Append(monsterTransform.DOMove(transform.position + Vector3.up * 2 + Vector3.forward, 0.5f));
        sequence.Join(rotationPointTransform.DORotate(new Vector3(0, 1080, 0), 4.5f, RotateMode.FastBeyond360));
        sequence.Append(monsterTransform.DOMove(transform.position, 0.5f).SetEase(Ease.InExpo).OnComplete(() =>
        {
            AudioSource.PlayClipAtPoint(reuniteClip, transform.position);
            monster.gameObject.SetActive(false);
            spriteRenderer.gameObject.SetActive(false);
            unitedSpriteRenderer.gameObject.SetActive(true);
            inControl = true;
            IsReunited = true;
        }));
        sequence.Play();
    }

    void Update()
    {
        if (!inControl) return;
        HandleMovement();
        if (walkDirection != Vector3.zero)
        {
            footstepsAudio.Play();
        }
        else footstepsAudio.Stop();
        if (!isInLight)
        {
            PlaySoundEffect();

            Push(-walkDirection);
        }
    }

    private void PlaySoundEffect()
    {
        int random = Random.Range(0, 7);
        Debug.Log(random);
        AudioClip selectedClip = random <= 5 ? gaspSFX : mgsClip;
        AudioSource.PlayClipAtPoint(selectedClip, transform.position);
        alertIconRenderer.transform.DOKill();
        Sequence popup = DOTween.Sequence();
        alertIconRenderer.gameObject.SetActive(true);
        alertIconRenderer.color = Color.white;
        popup.Append(alertIconRenderer.transform.DOShakeScale(0.5f,
                alertIconRenderer.transform.lossyScale * 1.01f))
            .Append((alertIconRenderer.DOFade(0, 0.3f)).SetDelay(0.3f));
        popup.Play();
    }

    public async Task Push(Vector3 direction, bool firstCall = true, float time = 0.35f)
    {
        inControl = false;
        float startTime = Time.time;
        while (Time.time < startTime + time)
        {
            characterController.Move(direction * Time.deltaTime * moveSpeedFactor * pushSpeedMultiplier);
            if (!firstCall && isInLight) break;
            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }
        inControl = true;
        if (!isInLight)
        {
            escapeAttempts++;
            if (escapeAttempts > 5)
            {
                transform.DOShakePosition(1.5f, Vector3.right * 0.2f)
                    .OnComplete(() =>
                    {
                        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
                        SceneManager.LoadScene(sceneIndex);
                    });
                return;
            }
            Vector3 directionFromLight = transform.position - ShadowDetector.instance.transform.position;
            direction.y = 0;
            Push(directionFromLight.normalized, false);
        }
    }
}
