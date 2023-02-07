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
   
}
