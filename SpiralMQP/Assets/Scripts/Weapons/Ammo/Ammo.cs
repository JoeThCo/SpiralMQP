using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Ammo : MonoBehaviour, IFireable
{
    [Tooltip("Create with child TrailRenderer component")]
    [SerializeField] private TrailRenderer trailRenderer;

    private float ammoRange = 0f; // The range of each ammo
    private float ammoSpeed;
    private Vector3 fireDirectionVector; // Can be calculated
    private float fireDirectionAngle;
    private SpriteRenderer spriteRenderer;
    private AmmoDetailsSO ammoDetails;
    private float ammoChargeTimer; // This is used if we want to use an ammo pattern instead of a single bullet
    private bool isAmmoMaterialSet = false;
    private bool overrideAmmoMovement;
    private bool isColliding = false; // This boolean value is used to prevent a single bullet triggering multiple collisions and causing player or enemy dealt more damage from the ammo

    private void Awake()
    {
        // Cache sprite renderer
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    private void Update()
    {
        // Ammo charge effect
        if (ammoChargeTimer > 0f)
        {
            ammoChargeTimer -= Time.deltaTime;
            return;
        }
        else if (!isAmmoMaterialSet)
        {
            SetAmmoMaterial(ammoDetails.ammoMaterial);
            isAmmoMaterialSet = true;
        }

        // Don't move ammo if movement has been overriden - e.g. this ammo is part of an ammo pattern
        if (!overrideAmmoMovement)
        {
            // Calculate distance vector to move ammo for each frame or each update
            Vector3 distanceVector = fireDirectionVector * ammoSpeed * Time.deltaTime;
            transform.position += distanceVector;

            // Distance after max range reached
            ammoRange -= distanceVector.magnitude;

            if (ammoRange < 0f)
            {
                // Only interested in player ammo
                if (ammoDetails.isPlayerAmmo)
                {
                    // No multiplier
                    StaticEventHandler.CallMultiplierEvent(false);
                }

                // Return ammo back to the pool
                DisableAmmo();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // If already colliding with something, return
        if (isColliding) return;

        // Deal damage to collision object
        DealDamage(other);

        // Show ammo hit effect
        AmmoHitEffect();

        // Return ammo back to the pool
        DisableAmmo();
    }

    private void DealDamage(Collider2D other)
    {
        Health health = other.GetComponent<Health>();

        bool enemyHit = false; // A boolean value to check if this the damage is given to the enemy

        if (health != null)
        {
            // Set isColliding to prevent ammo dealing damage multiple times
            isColliding = true;

            health.TakeDamage(ammoDetails.ammoDamage);

            // Enemy hit
            if (health.enemy != null)
            {
                enemyHit = true;

                if (ammoDetails.ammoPrefabArray[0].CompareTag("shotgunAmmo"))
                {
                    other.gameObject.GetComponent<EnemyMovementAI>().isShotgunHit = true;
                }
            }
        }

        // If player ammo then update multiplier
        if (ammoDetails.isPlayerAmmo)
        {
            if (enemyHit)
            {
                // Multiplier 
                StaticEventHandler.CallMultiplierEvent(true);
            }
            else
            {
                // No multiplier
                StaticEventHandler.CallMultiplierEvent(false);
            }
        }
    }

    /// <summary>
    /// Display the ammo hit effect
    /// </summary>
    private void AmmoHitEffect()
    {
        // Process if a hit effect has been specified
        if (ammoDetails.ammoHitEffect != null && ammoDetails.ammoHitEffect.ammoHitEffectPrefab != null)
        {
            // Get ammo hit effect gameobject from the pool (with particle system component)
            AmmoHitEffect ammoHitEffect = (AmmoHitEffect)PoolManager.Instance.ReuseComponent(ammoDetails.ammoHitEffect.ammoHitEffectPrefab, transform.position, Quaternion.identity);

            // Set Hit Effect
            ammoHitEffect.SetHitEffect(ammoDetails.ammoHitEffect);

            // Set gameobject active (the particle system is set to automatically disable the gameobject once finished)
            ammoHitEffect.gameObject.SetActive(true);
        }
    }


    /// <summary>
    /// Disable the ammo - thus returning it to the object pool
    /// </summary>
    private void DisableAmmo()
    {
        gameObject.SetActive(false);
    }


    /// <summary>
    /// Set the sprite renderer material with the input material
    /// </summary>
    public void SetAmmoMaterial(Material material)
    {
        spriteRenderer.material = material;
    }

    /// <summary>
    /// Initilize the ammo being fired 
    /// </summary>
    /// <param name="overrideAmmoMovement">If the ammo is part of a pattern, the ammo movement can be overriden by setting this to true </param>
    public void InitializeAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false)
    {
        #region Ammo 

        this.ammoDetails = ammoDetails;

        // Initialize isColliding to false
        isColliding = false;

        // Set fire direction
        SetFireDirection(ammoDetails, aimAngle, weaponAimAngle, weaponAimDirectionVector);

        // Set ammo sprite
        spriteRenderer.sprite = ammoDetails.ammoSprite;

        // Set initial ammo material depending on whether there is an ammo charge period
        if (ammoDetails.ammoChargeTime > 0f)
        {
            // Set ammo charge timer
            ammoChargeTimer = ammoDetails.ammoChargeTime;
            SetAmmoMaterial(ammoDetails.ammoChargeMaterial); // Set to charge material
            isAmmoMaterialSet = false;
        }
        else
        {
            ammoChargeTimer = 0f;
            SetAmmoMaterial(ammoDetails.ammoMaterial); // Set to normal material 
            isAmmoMaterialSet = true;
        }

        // Set ammo range
        ammoRange = ammoDetails.ammoRange;

        // Set ammo speed
        this.ammoSpeed = ammoSpeed;

        // Override ammo movement
        this.overrideAmmoMovement = overrideAmmoMovement; // For ammo pattern

        // Activate ammo gameobject
        gameObject.SetActive(true);

        #endregion

        // ------------------------------------------------------------------------------------------------------------

        #region Trail

        if (ammoDetails.isAmmoTrail)
        {
            trailRenderer.gameObject.SetActive(true);
            trailRenderer.emitting = true;
            trailRenderer.material = ammoDetails.ammoTrailMaterial;
            trailRenderer.startWidth = ammoDetails.ammoTrailStartWidth;
            trailRenderer.endWidth = ammoDetails.ammoTrailEndWidth;
            trailRenderer.colorGradient = ammoDetails.colorGradient;
            trailRenderer.time = ammoDetails.ammoTrailTime;
        }
        else
        {
            trailRenderer.emitting = false;
            trailRenderer.gameObject.SetActive(false);
        }

        #endregion
    }


    /// <summary>
    /// Set ammo fire direction and angle based on the input angle and direction adjusted by the random spread
    /// </summary>
    private void SetFireDirection(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        // Calculate random spread angle between min and max
        float randomSpread = UnityEngine.Random.Range(ammoDetails.ammoSpreadMin, ammoDetails.ammoSpreadMax);

        // Get a random spread toggle of 1 or -1
        int spreadToggle = Random.Range(0, 2) * 2 - 1;

        // Check to see which angle we are using
        if (weaponAimDirectionVector.magnitude < Settings.useAimAngleDistance)
        {
            fireDirectionAngle = aimAngle; // Using aim angle from the player
        }
        else
        {
            fireDirectionAngle = weaponAimAngle; // Using aim angle from the weapon
        }

        // Adjust ammo fire angle by random spread
        fireDirectionAngle += spreadToggle * randomSpread;

        // Set ammo rotaion on Z-axis
        transform.eulerAngles = new Vector3(0f, 0f, fireDirectionAngle);

        // Set ammo fire direction
        fireDirectionVector = HelperUtilities.GetDirectionVectorFromAngle(fireDirectionAngle);
    }

    public GameObject GetGameObject()
    {
        return gameObject;
    }


    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(trailRenderer), trailRenderer);
    }
#endif
    #endregion
}
