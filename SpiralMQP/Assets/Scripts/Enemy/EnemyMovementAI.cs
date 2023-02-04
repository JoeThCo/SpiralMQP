using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
[DisallowMultipleComponent]
public class EnemyMovementAI : MonoBehaviour
{
    // MovementDetailsSO contains movement details such as speed
    [SerializeField] private MovementDetailsSO movementDetails;
}
