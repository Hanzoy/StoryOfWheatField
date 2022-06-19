using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class ItemFader : MonoBehaviour
{
    private SpriteRenderer _spriteRenderer;

    private void Awake()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    /// <summary>
    /// 逐渐恢复颜色
    /// </summary>
    public void FadeIn()
    {
        Color targetColor = new Color(1, 1, 1, 1);
        _spriteRenderer.DOColor(targetColor, Settings.ItemFadeDuration);
    }

    /// <summary>
    /// 逐渐颜色透明
    /// </summary>
    public void FadeOn()
    {
        Color targetColor = new Color(1, 1, 1, Settings.TargetAlpha);
        _spriteRenderer.DOColor(targetColor, Settings.ItemFadeDuration);
    }
}
