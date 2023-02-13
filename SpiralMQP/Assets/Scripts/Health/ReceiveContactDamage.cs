using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Health))]
[DisallowMultipleComponent]
public class ReceiveContactDamage : MonoBehaviour
{
    [Header("The contact damage amount to receive")]
    [SerializeField] private int contactDamageAmount; // For damage overriding purpose - change the damage amount from the dealer
    private Health health;

    private void Awake()
    {
        // Load components
        health = GetComponent<Health>();
    }

    public void TakeContactDamage(int damageAmount = 0)
    {
        if (contactDamageAmount > 0)  damageAmount = contactDamageAmount;

        // For testing
        // Debug.Log("Receive Potential damage of: " + damageAmount);

        health.TakeDamage(damageAmount);
    }


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(contactDamageAmount), contactDamageAmount, true);
    }
#endif
    #endregion
}
