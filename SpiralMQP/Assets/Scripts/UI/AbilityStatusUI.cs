using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityStatusUI : MonoBehaviour
{
    [Space(10)]
    [Header("OBJECT REFERENCES")]

    [Tooltip("Populate with the RectTransform of the child gameobject CoolDownBar")]
    [SerializeField] private Transform cooldownBar;

    [Tooltip("Populate with the Image component of the child gameobject BarImage")]
    [SerializeField] private Image barImage;

    private Player player;
    private UseAbility useAbility;

    private void Awake()
    {
        // Get player
        player = GameManager.Instance.GetPlayer();
        useAbility = player.useAbility;
    }

    private void OnEnable()
    {
        // Subscribe to ability use event
        player.useAbilityEvent.OnUseAbility += UseAbilityEvent_OnUseAbility;
    }

    private void OnDisable()
    {
        // Unsubscribe from ability use event
        player.useAbilityEvent.OnUseAbility -= UseAbilityEvent_OnUseAbility;
    }

    private void Start()
    {
        // Update ability status on the UI
        // SetActiveWeapon(player.activeWeapon.GetCurrentWeapon());
    }

    private void Update()
    {
        // Update ability status on the UI
        UpdateAbilityCoolDownBar();
    }

    
    private void UseAbilityEvent_OnUseAbility(UseAbilityEvent useAbilityEvent, UseAbilityEventArgs useAbilityEventArgs)
    {

    }

    /// <summary>
    /// Update ability CD bar
    /// </summary>
    private void UpdateAbilityCoolDownBar()
    {
        // Update CDbar
        float barFill =  1-useAbility.GetAbilityCD()/ 10.0f; // Calculate the percentage progress of the cd bar
        if(barFill >= 1f) barFill = 1f;
        // Update bar fill
        cooldownBar.transform.localScale = new Vector3(barFill, 1f, 1f);

        // Set bar color as green
        if(barFill>=1f){
            barImage.color = Color.green;
        } else{
            barImage.color = Color.red;
        }
    }
}
