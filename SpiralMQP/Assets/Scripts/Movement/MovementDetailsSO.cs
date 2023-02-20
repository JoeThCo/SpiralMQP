using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MovementDetails", menuName = "Scriptable Objects/Movement/MovementDetails")]
public class MovementDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("MOVEMENT DETAILS")]
    [Tooltip("The minimum move speed.")] // The GetMoveSpeed method calculates a random value between the max and min speed
    public float minMoveSpeed = 8f;

    [Tooltip("The maximum move speed.")]
    public float maxMoveSpeed = 8f;


    // Rolling is only for the player (The enemies don't need to roll, try to convince me otherwise)
    [Tooltip("This is the roll speed")]
    public float rollSpeed; 

    [Tooltip("This is the roll distance")]
    public float rollDistance; 

    [Tooltip("This is the cooldown time in seconds between roll actions")]
    public float rollCooldownTime; 

    /// <summary>
    /// Get a random movement speed between the minimum and maximum values
    /// </summary>
    /// <returns></returns>
    public float GetMoveSpeed()
    {
        if (minMoveSpeed == maxMoveSpeed)
        {
            return minMoveSpeed;
        }
        else
        {
            return Random.Range(minMoveSpeed, maxMoveSpeed);
        }
    }


    #region Validation
#if UNITY_EDITOR
    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckPositiveRange(this, nameof(minMoveSpeed), minMoveSpeed, nameof(maxMoveSpeed), maxMoveSpeed, false);

        if (rollDistance != 0f || rollSpeed != 0f || rollCooldownTime != 0f)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(rollDistance), rollDistance, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(rollSpeed), rollSpeed, false);
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(rollCooldownTime), rollCooldownTime, false);
        }
    }
#endif
    #endregion
}
