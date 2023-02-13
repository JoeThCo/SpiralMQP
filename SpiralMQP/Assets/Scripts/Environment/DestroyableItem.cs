using System.Collections;
using UnityEngine;

// Don't add require directives since we're destroying the components when the item is destroyed
[DisallowMultipleComponent]
public class DestroyableItem : MonoBehaviour
{
    [Space(10)]
    [Header("HEALTH")]
    [Tooltip("What the starting health for this destroyable item should be")]
    [SerializeField] private int startingHealthAmount = 1;
   

    [Space(10)]
    [Header("SOUND EFFECT")]
    [Tooltip("The sound effect when this item is destroyed")]
    [SerializeField] private SoundEffectSO destroySoundEffect;

    private Animator animator;
    private BoxCollider2D boxCollider2D; 
    private PolygonCollider2D polygonCollider2D;
    private HealthEvent healthEvent;
    private Health health;
    //private ReceiveContactDamage receiveContactDamage;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        boxCollider2D = GetComponent<BoxCollider2D>();
        polygonCollider2D = GetComponent<PolygonCollider2D>();
        healthEvent = GetComponent<HealthEvent>();
        health = GetComponent<Health>();
        health.SetStartingHealth(startingHealthAmount);
        //receiveContactDamage = GetComponent<ReceiveContactDamage>();
    }

    private void OnEnable()
    {
        healthEvent.OnHealthChanged += HealthEvent_OnHealthLost;
    }


    private void OnDisable()
    {
        healthEvent.OnHealthChanged -= HealthEvent_OnHealthLost;
    }

    private void HealthEvent_OnHealthLost(HealthEvent healthEvent, HealthEventArgs healthEventArgs)
    {
        if (healthEventArgs.healthAmount <= 0f)
        {
            StartCoroutine(PlayAnimation());
        }
    }

    private IEnumerator PlayAnimation()
    {
        // Destroy the trigger collider and collider
        Destroy(boxCollider2D);
        Destroy(polygonCollider2D);

        // Play sound effect
        if (destroySoundEffect != null)
        {
            SoundEffectManager.Instance.PlaySoundEffect(destroySoundEffect);
        }

        // Trigger the destroy animation
        animator.SetBool(Settings.destroy, true);

        // Let the animation play through
        while (!animator.GetCurrentAnimatorStateInfo(0).IsName(Settings.stateDestroyed))
        {
            yield return null;
        }

        // Then destroy all components other than the Sprite Renderer to just display the final sprite in the animation
        //Destroy(receiveContactDamage);
        Destroy(animator);
        Destroy(health);
        Destroy(healthEvent);
        Destroy(this);

    }
}