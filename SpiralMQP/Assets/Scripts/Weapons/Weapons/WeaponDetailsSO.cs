using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponDetails", menuName = "Scriptable Objects/Weapons/Weapon Details")]
public class WeaponDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("WEAPON BASE DETAILS")]
    [Tooltip("Weapon name")]
    public string weaponName;
    [Tooltip("The sprite for the weapon - the sprite should have the 'generated physics shape' option selected")]
    public Sprite weaponSprite;


    [Space(10)]
    [Header("WEAPON CONFIGURATION")]
    [Tooltip("Weapon Shoot Position - the offset position for the end of the weapon from the sprite pivot point")]
    public Vector3 weaponShootPosition;
    // [Tooltip("Weapon current ammo")]
    // public AmmoDetailsSO weaponCurrentAmmo;


    [Space(10)]
    [Header("WEAPON OPERATING VALUES")]
    [Tooltip("Select if the weapon has infinite ammo")]
    public bool hasInfiniteAmmo = false; // Here is how we can cheat the game as a game developer

    [Tooltip("Select if the weapon has infinite clip capacity")]
    public bool hasInfiniteClipCapacity = false; // This is meant to be used for the enemies

    [Tooltip("The weapon capacity - shots before a reload")]
    public int weaponClipAmmoCapacity = 6; // The ammo limit in the clip

    [Tooltip("Weapon ammo capacity - the maximum number of rounds at that can be held for this weapon")]
    public int weaponAmmoCapacity = 100; // The ammo limit you can collect

    [Tooltip("Weapon fire rate - 0.2 means 5 shots per second")]
    public float weaponFireRate = 0.2f; // How quickly the weapon can fire

    [Tooltip("Weapon precharge time - time in seconds to hold fire button down before firing")]
    public float weaponPrechargeTime = 0f; // The amount of time you have to wait before you can see the bullets come out - 武器前摇

    [Tooltip("This is the weapon reload time in seconds")]
    public float weaponReloadTime = 0f;




    #region Validation
#if UNITY_EDITOR
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(weaponName), weaponName);
        //HelperUtilities.ValidateCheckNullValue(this, nameof(weaponCurrentAmmo), weaponCurrentAmmo);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponFireRate), weaponFireRate, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponPrechargeTime), weaponPrechargeTime, true);

        if (!hasInfiniteAmmo)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponAmmoCapacity), weaponAmmoCapacity, false);
        }

        if (!hasInfiniteClipCapacity)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(weaponClipAmmoCapacity), weaponClipAmmoCapacity, false);
        }

    }
#endif
    #endregion
}
