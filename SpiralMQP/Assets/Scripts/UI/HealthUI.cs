using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]
public class HealthUI : MonoBehaviour
{
    private Animator HeartAnimator;
    private int playerHealthPercentage;
    [SerializeField] private Image fill;
    [SerializeField] private Slider slider;

    private void Awake() 
    {
        HeartAnimator = GetComponentInChildren<Animator>();
    }

    private void Start() 
    {
        playerHealthPercentage = 100;
        slider.value = 1;
    }

    private void OnEnable()
    {
        GameManager.Instance.GetPlayer().healthEvent.OnHealthChanged += HealthEvent_OnHealthChanged;
    }

    private void OnDisable()
    {
        GameManager.Instance.GetPlayer().healthEvent.OnHealthChanged -= HealthEvent_OnHealthChanged;
    }

    private void HealthEvent_OnHealthChanged(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        // Handle Heart
        CalculateHealthPercentUI(healthEventArgs);
        SetHitAnimation();
        Invoke(nameof(SetHeartAnimation), 0.2f);

        // Handle Health Bar
        SetHealthBar(healthEventArgs);
    }

    private void SetHealthBar(HealthEventArgs healthEventArgs)
    {
        slider.value = healthEventArgs.healthPercent;

        if (playerHealthPercentage >= 75)
        {
            fill.sprite = GameResources.Instance.healthBarGreen;
        }
        else if (playerHealthPercentage >= 25 && playerHealthPercentage < 75)
        {
            fill.sprite = GameResources.Instance.healthBarYellow;
        }
        else
        {
            fill.sprite = GameResources.Instance.healthBarRed;
        }
    }

    private void CalculateHealthPercentUI(HealthEventArgs healthEventArgs)
    {
        playerHealthPercentage = Mathf.CeilToInt(healthEventArgs.healthPercent * 100f);
    }

    private void SetHeartAnimation()
    {
        HeartAnimator.SetBool(Settings.isHit, false);
        HeartAnimator.SetInteger(Settings.healthPercentUI, playerHealthPercentage);
    }

    private void SetHitAnimation()
    {
        HeartAnimator.SetBool(Settings.isHit, true);
    }
}