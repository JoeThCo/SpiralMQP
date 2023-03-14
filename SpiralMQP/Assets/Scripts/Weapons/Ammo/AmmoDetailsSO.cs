using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AmmoDetails", menuName = "Scriptable Objects/Weapons/Ammo Details")]
public class AmmoDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("BASIC AMMO DETAILS")]
    [Tooltip("Name for the ammo")]
    public string ammoName;
    public bool isPlayerAmmo;


    [Space(10)]
    [Header("AMMO SPRITE, PREFAB & MATERIALS")]
    [Tooltip("Sprite to be used for the ammo")]
    public Sprite ammoSprite;

    [Tooltip("Create with the prefab to be used for the ammo. If multiple prefabs are specified, then a random prefab from the array will be selected. The prefab can be an ammo pattern - as long as it conforms to the IFireable interface.")]
    public GameObject[] ammoPrefabArray;

    [Tooltip("The material to be used for the ammo")]
    public Material ammoMaterial;

    [Tooltip("(For Pattern)If the ammo should 'charge' briefly before moving then set the time in seconds that the ammo is held charging after firing before release")]
    public float ammoChargeTime = 0.1f;

    [Tooltip("(For Pattern)If the ammo has a charge time then specify what material should be used to render the ammo while charging")]
    public Material ammoChargeMaterial;


    [Space(10)]
    [Header("AMMO HIT EFFECT")]
    [Tooltip("The scriptable object that defines the parameters for the hit effect prefab.")]
    public AmmoHitEffectSO ammoHitEffect;


    [Space(10)]
    [Header("AMMO BASE PARAMETERS")]
    [Tooltip("The damage each ammo deals")]
    public int ammoDamage = 1;

    [Tooltip("The minimum speed of the ammo - the speed will be a random value between the min and the max")]
    public float ammoSpeedMin = 20f;
    [Tooltip("The maximum speed of the ammo - the speed will be a random value between the min and the max")]
    public float ammoSpeedMax = 20f;

    [Tooltip("The range of the ammo (or ammo pattern) in unity units")]
    public float ammoRange = 20f;

    [Tooltip("(For Pattern)The rotation speed in degrees per second of the ammo pattern")]
    public float ammoRotationSpeed = 1f;



    [Space(10)]
    [Header("AMMO SPREAD DETAILS")]
    [Tooltip("This is the minimum spread angle of the ammo - A higher spread means less accuracy. A random spread is calculated between the min and max value.")]
    public float ammoSpreadMin = 0f;
    [Tooltip("This is the maximum spread angle of the ammo - A higher spread means less accuracy. A random spread is calculated between the min and max value.")]
    public float ammoSpreadMax = 0f;


    [Space(10)]
    [Header("AMMO SPAWN DETAILS")]
    [Tooltip("This is the minimum number of ammo that are spawned per shot - A random number of ammo are spawned between the minimum and maximum values.")]
    public int ammoSpawnAmoutMin = 1;
    [Tooltip("This is the maximum number of ammo that are spawned per shot - A random number of ammo are spawned between the minimum and maximum values.")]
    public int ammoSpawnAmoutMax = 1;

    [Tooltip("Minimum spawn interval time - The time interval in seconds between each spawned ammo is a random value between minimum and maximum values specified.")]
    public float ammoSpawnIntervalMin = 0f;
    [Tooltip("Maximum spawn interval time - The time interval in seconds between each spawned ammo is a random value between minimum and maximum values specified.")]
    public float ammoSpawnIntervalMax = 0f;


    [Space(10)]
    [Header("AMMO TRAIL DETAILS")]
    [Tooltip("Selected if an ammo trail is required - If selected then the rest of the ammo trail values should be created")]
    public bool isAmmoTrail = false;

    [Tooltip("Color gradient for the ammo trail")]
    public Gradient colorGradient;

    [Tooltip("Ammo trail lifetime in seconds")]
    public float ammoTrailTime = 3f;

    [Tooltip("Ammo trail material")]
    public Material ammoTrailMaterial;

    [Tooltip("The starting width for the ammo trail")]
    [Range(0f,1f)] public float ammoTrailStartWidth; // Range will give us a slider to adjust the value in the inspector
    [Tooltip("The ending width for the ammo trail")]
    [Range(0f,1f)] public float ammoTrailEndWidth; // Range will give us a slider to adjust the value in the inspector


    #region Validation
#if UNITY_EDITOR
    private void OnValidate() 
    {
        if (ammoChargeTime > 0)
        {
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoChargeMaterial), ammoChargeMaterial);
        }

        if (isAmmoTrail)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailTime), ammoTrailTime, false);
            HelperUtilities.ValidateCheckNullValue(this, nameof(ammoTrailMaterial), ammoTrailMaterial);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailStartWidth), ammoTrailStartWidth, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(ammoTrailEndWidth), ammoTrailEndWidth, false);
        }
    }
#endif
    #endregion
}
