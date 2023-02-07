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
}
