using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UseAbilityEvent))]

public class UseAbility : MonoBehaviour{
    private float abilityCoolDownTimer = 0f;
    private UseAbilityEvent useAbilityEvent;
    private void Awake()
    {
        // Load components
        useAbilityEvent = GetComponent<UseAbilityEvent>();
    }
    private void OnEnable()
    {   // Subscribe to event
        useAbilityEvent.OnUseAbility += UseAbilityEvent_OnUseAbility;
    }

    private void OnDisable()
    {
        // Unsubscribe from event
        useAbilityEvent.OnUseAbility -= UseAbilityEvent_OnUseAbility;
    }

    private void Update()
    {
        // Decrease cooldown timer
        abilityCoolDownTimer -= Time.deltaTime;
    }
    private void UseAbilityEvent_OnUseAbility(UseAbilityEvent useAbilityEvent, UseAbilityEventArgs useAbilityEventArgs)
    {
        ExecuteAbility(useAbilityEventArgs);
    }

    private void ExecuteAbility(UseAbilityEventArgs useAbilityEventArgs)
    {
        // Use active ability
        if (useAbilityEventArgs.use)
        {
            // Test if ability is ready to use
            if (IsReadyToUse())
            {
                ResetCoolDownTimer();
            }
        }
    }

    private void ResetCoolDownTimer()
    {
        // Reset cooldown timer
        abilityCoolDownTimer = 10f;
    }


    private bool IsReadyToUse()
    {
        if(abilityCoolDownTimer <= 0f)
            return true;
        return false;
    }

    public float GetAbilityCD(){
        return abilityCoolDownTimer;
    }
}