using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEditor.ShaderGraph;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Child : ControlledEntity
{
    [SerializeField] float pushTime = 0.35f;
    [SerializeField] float pushSpeedMultiplier = 5f;
    [SerializeField] private Monster monster;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private SpriteRenderer unitedSpriteRenderer;
    [SerializeField] private SpriteRenderer alertIconRenderer;
    
    int scareCount;
    public bool IsReunited { get; private set; }
    
    protected override void Start()
    {
        base.Start();
        IsReunited = false;
        ShadowDetector.OnMonsterReunited += OnMonsterReunited;
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
        HandleMovement();
        if (!isInLight)
        {
            scareCount++;
            if (scareCount >= 3)
            {
                Debug.Log("Game Over");
                Scene scene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(scene.name);
            }
            else
            {
                Sequence popup = DOTween.Sequence();
                alertIconRenderer.gameObject.SetActive(true);
                alertIconRenderer.color = Color.white;
                popup.Append(alertIconRenderer.transform.DOShakeScale(0.5f, alertIconRenderer.transform.lossyScale * 1.01f))
                    .Append((alertIconRenderer.DOFade(0, 0.3f)).SetDelay(0.3f));
                popup.Play();
                Push(-walkDirection);
            }
        }
    }

    public async Task Push(Vector3 direction)
    {
        inControl = false;
        float startTime = Time.time;
        while (Time.time < startTime + pushTime)
        {
            characterController.Move(direction * Time.deltaTime * moveSpeedFactor * pushSpeedMultiplier);
            await Task.Delay(Mathf.RoundToInt(Time.deltaTime * 1000));
        }
        inControl = true;
        if (isInLight) scareCount = 0;
    }
}
