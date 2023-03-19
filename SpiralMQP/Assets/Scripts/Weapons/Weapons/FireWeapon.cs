using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ActiveWeapon))]
[RequireComponent(typeof(FireWeaponEvent))]
[RequireComponent(typeof(ReloadWeaponEvent))]
[RequireComponent(typeof(WeaponFiredEvent))]
[DisallowMultipleComponent]
public class FireWeapon : MonoBehaviour
{
    private float fireRateCoolDownTimer = 0f;
    private float firePreChargeTimer = 0f;
    private ActiveWeapon activeWeapon;
    private FireWeaponEvent fireWeaponEvent;
    private ReloadWeaponEvent reloadWeaponEvent;
    private WeaponFiredEvent weaponFiredEvent;
    private GameObject prechargeEffect;
    private Enemy enemy;

    private void Awake()
    {
        // Load components
        activeWeapon = GetComponent<ActiveWeapon>();
        fireWeaponEvent = GetComponent<FireWeaponEvent>();
        reloadWeaponEvent = GetComponent<ReloadWeaponEvent>();
        weaponFiredEvent = GetComponent<WeaponFiredEvent>();

        // Try load enemy
        enemy = GetComponent<Enemy>();
    }

    private void OnEnable()
    {   // Subscribe to fire weapon event
        fireWeaponEvent.OnFireWeapon += FireWeaponEvent_OnFireWeapon;
    }

    private void OnDisable()
    {
        // Unsubscribe from fire weapon event
        fireWeaponEvent.OnFireWeapon -= FireWeaponEvent_OnFireWeapon;

        if (prechargeEffect != null)
        {
            Destroy(prechargeEffect);
        }
    }

    private void Update()
    {
        // Decrease cooldown timer
        fireRateCoolDownTimer -= Time.deltaTime;

        // Set effect position
        if (prechargeEffect != null)
        {
            prechargeEffect.transform.position = transform.position;
        }
    }

    /// <summary>
    /// Handle fire weapon event
    /// </summary>
    private void FireWeaponEvent_OnFireWeapon(FireWeaponEvent fireWeaponEvent, FireWeaponEventArgs fireWeaponEventArgs)
    {
        WeaponFire(fireWeaponEventArgs);
    }


    /// <summary>
    /// Fire weapon
    /// </summary>
    private void WeaponFire(FireWeaponEventArgs fireWeaponEventArgs)
    {
        // Handle weapon precharge timer
        WeaponPreCharge(fireWeaponEventArgs);

        // Weapon fire
        if (fireWeaponEventArgs.fire)
        {
            // Test if weapon is ready to fire
            if (IsWeaponReadyToFire())
            {
                if (enemy != null && enemy.enemyDetails.enemyWeapon.weaponName == "Plasma Blaster")
                {
                    StartCoroutine(PrechargeFireIndication());
                }
                else if (enemy != null && fireWeaponEventArgs.isBossFiring)
                {
                    if (activeWeapon.GetCurrentWeapon().weaponDetails.weaponName == "BossAmmo1")
                    {
                        FireBossAmmoPattern();
                    }
                }
                else
                {
                    FireAmmo(fireWeaponEventArgs.aimAngle, fireWeaponEventArgs.weaponAimAngle, fireWeaponEventArgs.weaponAimDirectionVector);
                }

                ResetCoolDownTimer();

                ResetPreChargeTimer();
            }
        }
    }

    private IEnumerator PrechargeFireIndication()
    {
        // Instantiate the prechage effect on enemy
        prechargeEffect = Instantiate(GameResources.Instance.prechargeEffect, transform.position, Quaternion.identity);

        yield return new WaitForSeconds(0.4f);

        Destroy(prechargeEffect);

        // Player distance
        Vector3 playerDirectionVector = GameManager.Instance.GetPlayer().GetPlayerPosition() - transform.position;

        // Calculate direction vector of player from weapon shoot position
        Vector3 weaponDirection = (GameManager.Instance.GetPlayer().GetPlayerPosition() - enemy.enemyWeaponAI.weaponShootPosition.position);

        // Get weapon to player angle
        float weaponAngleDegrees = HelperUtilities.GetAngleFromVector(weaponDirection);

        // Get enemy to player angle
        float enemyAngleDegrees = HelperUtilities.GetAngleFromVector(playerDirectionVector);

        FireAmmo(enemyAngleDegrees, weaponAngleDegrees, weaponDirection);
    }

    /// <summary>
    /// Handle weapon precharge
    /// </summary>
    private void WeaponPreCharge(FireWeaponEventArgs fireWeaponEventArgs)
    {
        // Weapon precharge
        if (fireWeaponEventArgs.firePreviousFrame)
        {
            // Decrease precharge timer if fire button held previous frame
            firePreChargeTimer -= Time.deltaTime;
        }
        else
        {
            // Reset the precharge timer
            ResetPreChargeTimer();
        }
    }


    /// <summary>
    /// Reset cooldown timer
    /// </summary>
    private void ResetCoolDownTimer()
    {
        // Reset cooldown timer
        fireRateCoolDownTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponFireRate;
    }

    /// <summary>
    /// Reset precharge timer
    /// </summary>
    private void ResetPreChargeTimer()
    {
        firePreChargeTimer = activeWeapon.GetCurrentWeapon().weaponDetails.weaponPrechargeTime;
    }

    /// <summary>
    /// Set up ammo using an ammo gameobject and component from the object pool
    /// </summary>
    private void FireAmmo(float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector, bool isBossRectFire = false, float rectMultiplier = 1f)
    {
        AmmoDetailsSO currentAmmo = activeWeapon.GetCurrentAmmo();

        if (currentAmmo != null)
        {
            // Fire ammo routine
            StartCoroutine(FireAmmoRoutine(currentAmmo, aimAngle, weaponAimAngle, weaponAimDirectionVector, isBossRectFire, rectMultiplier));
        }
    }

    private void FireBossAmmoPattern()
    {
        int randomPicker = Random.Range(1, 101);

        if (randomPicker % 2 == 0)
        {
            for (int i = 0; i < 360; i = i + 9)
            {
                FireAmmo(i, i, Vector3.one);
            }
        }
        else
        {
            for (int i = 0; i < 360; i = i + 5)
            {
                if (i % 90 == 0)
                {
                    FireAmmo(i, i, Vector3.one);
                }
                else if (i == 45 || i == 135 || i == 225 || i == 315)
                {
                    continue;
                }
                else if ((i > 0 && i < 45) || (i > 315 && i < 360) || (i > 135 && i < 180) || (i > 180 && i < 225))
                {
                    FireAmmo(i, i, Vector3.one, true, Mathf.Abs(1 / Mathf.Cos(Mathf.Deg2Rad * i)));
                } 
                else
                {
                    FireAmmo(i, i, Vector3.one, true, Mathf.Abs(1 / Mathf.Sin(Mathf.Deg2Rad * i)));
                }  
            }
        }
    }

    /// <summary>
    /// Coroutine to spawn multiple ammo per shot if specified in the ammo details
    /// </summary>
    private IEnumerator FireAmmoRoutine(AmmoDetailsSO currentAmmo, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector, bool isBossRectFire, float rectMultiplier)
    {
        int ammoCounter = 0;

        // Get random ammo per shot
        int ammoPerShot = Random.Range(currentAmmo.ammoSpawnAmoutMin, currentAmmo.ammoSpawnAmoutMax + 1);

        // Get random interval between ammo
        float ammoSpawnInterval;

        if (ammoPerShot > 1)
        {
            ammoSpawnInterval = Random.Range(currentAmmo.ammoSpawnIntervalMin, currentAmmo.ammoSpawnIntervalMax);
        }
        else
        {
            ammoSpawnInterval = 0f;
        }

        // Loop for number of ammo per shot
        while (ammoCounter < ammoPerShot)
        {
            ammoCounter++;

            // Get random ammo prefab from array 
            GameObject ammoPrefab = currentAmmo.ammoPrefabArray[Random.Range(0, currentAmmo.ammoPrefabArray.Length)];

            // Get random speed value
            float ammoSpeed = Random.Range(currentAmmo.ammoSpeedMin, currentAmmo.ammoSpeedMax);

            if (isBossRectFire)
            {
                ammoSpeed *= rectMultiplier;
            }

            // Get Gameobject with IFireable component
            IFireable ammo = (IFireable)PoolManager.Instance.ReuseComponent(ammoPrefab, activeWeapon.GetShootPosition(), Quaternion.identity);

            // Initialize Ammo
            ammo.InitializeAmmo(currentAmmo, aimAngle, weaponAimAngle, ammoSpeed, weaponAimDirectionVector);

            // Wait for ammo per shot time gap
            yield return new WaitForSeconds(ammoSpawnInterval);
        }

        // Reduce ammo clip count if not infinite clip capacity
        if (!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity)
        {
            activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo--;
            activeWeapon.GetCurrentWeapon().weaponRemainingAmmo--;
        }

        // Call weapon fired event
        weaponFiredEvent.CallWeaponFiredEvent(activeWeapon.GetCurrentWeapon());

        // Display weapon shoot effect
        WeaponShootEffect(aimAngle);

        // Weapon fired sound effect
        WeaponSoundEffect();
    }

    /// <summary>
    /// Display the weapon shoot effect
    /// </summary>
    private void WeaponShootEffect(float aimAngle)
    {
        // Process if there is a shoot effect & prefab
        if (activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect != null && activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect.weaponShootEffectPrefab != null)
        {
            // Get weapon shoot effect gameobject from the pool with particle system component
            WeaponShootEffect weaponShootEffect = (WeaponShootEffect)PoolManager.Instance.ReuseComponent(activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect.weaponShootEffectPrefab, activeWeapon.GetShootEffectPosition(), Quaternion.identity);

            // Set shoot effect
            weaponShootEffect.SetShootEffect(activeWeapon.GetCurrentWeapon().weaponDetails.weaponShootEffect, aimAngle);

            // Set gameobject active (the particle system is set to automatically disable the gameobject once finished)
            weaponShootEffect.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Play weapon shooting sound effect
    /// </summary>
    private void WeaponSoundEffect()
    {
        if (activeWeapon.GetCurrentWeapon().weaponDetails.weaponFiringSoundEffect != null)
        {
            SoundEffectManager.Instance.PlaySoundEffect(activeWeapon.GetCurrentWeapon().weaponDetails.weaponFiringSoundEffect);
        }
    }

    /// <summary>
    /// Return true if the weapon is ready to fire, return false otherwise
    /// </summary>
    /// <returns></returns>
    private bool IsWeaponReadyToFire()
    {
        // If there is no ammo and weapon doesn't have infinite ammo then return false
        if (activeWeapon.GetCurrentWeapon().weaponRemainingAmmo <= 0 && !activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteAmmo) return false;

        // If the weapon is reloading then return false
        if (activeWeapon.GetCurrentWeapon().isWeaponReloading) return false;

        // If the weapon is cooling down or isn't precharged then return false
        if (fireRateCoolDownTimer > 0f || firePreChargeTimer > 0f) return false;

        // If no ammo in the clip and the weapon doesn't have infinite clip capacity then reload and return false
        if (!activeWeapon.GetCurrentWeapon().weaponDetails.hasInfiniteClipCapacity && activeWeapon.GetCurrentWeapon().weaponClipRemainingAmmo <= 0)
        {
            // Trigger a reload weapon event
            reloadWeaponEvent.CallReloadWeaponEvent(activeWeapon.GetCurrentWeapon(), 0);

            return false;
        }

        // Weapon is ready to fire - return true
        return true;
    }
}
