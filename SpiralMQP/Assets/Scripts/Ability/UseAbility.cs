using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(UseAbilityEvent))]

public class UseAbility : MonoBehaviour{
    private float abilityCoolDownTimer = 0f;
    private UseAbilityEvent useAbilityEvent;
    private ChangeAbilityEvent changeAbilityEvent;
    [SerializeField] private AbilitySO ability;
    private void Awake()
    {
        // Load components
        useAbilityEvent = GetComponent<UseAbilityEvent>();
        changeAbilityEvent = GetComponent<ChangeAbilityEvent>();

    }
    private void OnEnable()
    {   // Subscribe to event
        useAbilityEvent.OnUseAbility += UseAbilityEvent_OnUseAbility;
        changeAbilityEvent.OnChangeAbility += ChangeAbilityEvent_OnChangeAbility;
        
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

    private void ChangeAbilityEvent_OnChangeAbility(ChangeAbilityEvent changeAbilityEvent, ChangeAbilityEventArgs changeAbilityEventArgs)
    {
        ability = changeAbilityEventArgs.newAbility;
        ResetCoolDownTimer();
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
        abilityCoolDownTimer = ability.AbilityCoolDown;
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

    public AbilitySO GetCurrentAbility(){
        return ability;
    }
}