using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyDetails", menuName = "Scriptable Objects/Enemy/EnemyDetails")]
public class EnemyDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("BASE ENEMY DETAILS")]
    [Tooltip("The name of the enemy")]
    public string enemyName;

    [Tooltip("The prefab for the enemy")]
    public GameObject enemyPrefab;

    [Tooltip("Distance to the player before enemy starts chasing")]
    public float chaseDistance = 50f;



    [Space(10)]
    [Header("ENEMY MATERIAL")]
    [Tooltip("This is the standard lit shader material for the enemy (used after the enemy materializeation)")]
    public Material enemyStandardMaterial;



    [Space(10)]
    [Header("ENEMY MATERIALIZE SETTINGS")]
    [Tooltip("The time in seconds that it takes the enemy to materialize")]
    public float enemyMaterializeTime;

    [Tooltip("The shader to be used when the enemy materializes")]
    public Shader enemyMaterializeShader;

    [ColorUsage(true, true)] // This attribute basically allow unity to display a HDR picker in the inspector
    [Tooltip("The color to use when the enemy materializes. This is an HDR color so intensity can be set to cause glowing / bloom")]
    public Color enemyMaterializeColor;



    [Space(10)]
    [Header("ENEMY WEAPON SETTINGS")]
    [Tooltip("The weapon for the enemy - none if the enemy doesn't have a weapon")]
    public WeaponDetailsSO enemyWeapon;

    [Tooltip("The minimum time delay interval in seconds between bursts of enemy shooting. This value should be greater than 0. A random value will be selected between the minimum value and the maximum value")]
    public float firingIntervalMin = 0.1f;

    [Tooltip("The maximum time delay interval in seconds between bursts of enemy shooting. A random value will be selected between the minimum value and the maximum value")]
    public float firingIntervalMax = 1f;

    [Tooltip("The minimum firing duration that the enemy shoots for during a firing burst. This value should be greater than zero. A random value will be selected between the minimum value and the maximum value.")]
    public float firingDurationMin = 1f;

    [Tooltip("The maximum firing duration that the enemy shoots for during a firing burst. A random value will be selected between the minimum value and the maximum value.")]
    public float firingDurationMax = 2f;

    [Tooltip("Select this if line of sight is required of the player before the enemy fires. If line of sight isn't selected the enemy will fire regardless of obstacles whenever the player is 'in range'")]
    public bool firingLineOfSightRequired;
}
