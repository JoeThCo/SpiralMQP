using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthEvent))]
[DisallowMultipleComponent] // Good practice
public class Health : MonoBehaviour  // This class is used for anything that needs health
{
    private int startingHealth;
    private int currentHealth;
    private HealthEvent healthEvent;
    private Player player;
    private Coroutine immunityCoroutine;
    private bool isImmuneAfterHit = false;
    private float immunityTime = 0f;
    private SpriteRenderer spriteRenderer = null;
    private const float spriteFlashInterval = 0.2f;
    private WaitForSeconds WaitForSecondsSpriteFlashInterval = new WaitForSeconds(spriteFlashInterval);

    [HideInInspector] public bool isDamageable = true;
    [HideInInspector] public Enemy enemy;

    private void Awake()
    {
        // Load components
        healthEvent = GetComponent<HealthEvent>();
    }

    private void Start()
    {
        // Trigger a health event for UI update
        CallHealthEvent(0);

        // Attempt to load enemy / player components
        player = GetComponent<Player>();
        enemy = GetComponent<Enemy>();

        // Get player / enemy hit immunity details
        if (player != null)
        {
            if (player.playerDetails.isImmuneAfterHit)
            {
                isImmuneAfterHit = true;
                immunityTime = player.playerDetails.hitImmunityTime;
                spriteRenderer = player.spriteRenderer;
            }
        }
        else if (enemy != null)
        {
            if (enemy.enemyDetails.isImmuneAfterHit)
            {
                isImmuneAfterHit = true;
                immunityTime = enemy.enemyDetails.hitImmunityTime;
                spriteRenderer = enemy.spriteRendererArray[0];
            }
        }
    }

    private void CallHealthEvent(int damageAmount)
    {
        // Trigger health event
        healthEvent.CallHealthChangedEvent(((float)currentHealth / (float)startingHealth), currentHealth, damageAmount);
    }

    /// <summary>
    /// Public method called when damage is taken
    /// </summary>
    public void TakeDamage(int damageAmount)
    {
        // Value holder for the player isPlayerRolling boolean
        bool isRolling = false;

        if (player != null) isRolling = player.playerControl.isPlayerRolling;

        if (isDamageable && !isRolling)
        {
            currentHealth -= damageAmount;
            CallHealthEvent(damageAmount);

            // Do the flash and be immune for some time
            PostHitImmunity();
        }

        // Testing
        if (isDamageable && isRolling)
        {
            Debug.Log("Bullet Dodged by Dashing/Rolling");
        }
        if (!isDamageable && !isRolling)
        {
            Debug.Log("Avoided Damage");
        }
    }

    /// <summary>
    /// Indicate a hit and give some post hit immunity
    /// </summary>
    private void PostHitImmunity()
    {
        // Check if gameobject is active - if not return
        if (gameObject.activeSelf == false) return;

        // If there is post hit immunity then
        if (isImmuneAfterHit)
        {
            if (immunityCoroutine != null) StopCoroutine(immunityCoroutine);

            // Flash red and give period of immunity
            immunityCoroutine = StartCoroutine(PostHitImmunityRoutine(immunityTime, spriteRenderer));
        }
    }

    /// <summary>
    /// Coroutine to indicate a hit and give some post hit immunity
    /// </summary>
    private IEnumerator PostHitImmunityRoutine(float immunityTime, SpriteRenderer spriteRenderer)
    {
        int iterations = Mathf.RoundToInt(immunityTime / spriteFlashInterval / 2f);

        isDamageable = false;

        while (iterations > 0)
        {
            spriteRenderer.color = Color.red;

            yield return WaitForSecondsSpriteFlashInterval;

            spriteRenderer.color = Color.white;

            yield return WaitForSecondsSpriteFlashInterval;

            iterations--;

            yield return null;

        }

        isDamageable = true;
    }

    /// <summary>
    /// Set starting health
    /// </summary>
    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }


    /// <summary>
    /// Get the starting health
    /// </summary>
    public int GetStartingHealth()
    {
        return startingHealth;
    }
}
