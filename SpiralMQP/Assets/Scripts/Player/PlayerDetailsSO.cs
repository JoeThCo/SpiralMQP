using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerDetails", menuName = "Scriptable Objects/Player/Player Details")]
public class PlayerDetailsSO : ScriptableObject
{
    [Space(10)]
    [Header("PLAYER BASE DETAILS")]
    [Tooltip("Player character name")]
    public string playerCharacterName;

    [Tooltip("Prefab gameobject for the player")]
    public GameObject playerPrefab;

    [Tooltip("Player runtime animator controller")]
    public RuntimeAnimatorController runtimeAnimatorController; // RuntimeAnimatorController is an asset that allows you to control the animation of a game object at runtime.


    [Space(10)]
    [Header("HEALTH")]
    [Tooltip("Player starting health")]
    public int playerHealthAmount;


    [Space(10)]
    [Header("Other")]
    [Tooltip("Player icon sprite for minimap")]
    public Sprite playerMinimapIcon;

    [Tooltip("Player hand sprite")]
    public Sprite playerHandSprite;

// This validation is definitely not neccesary, but I think this is a good practice to have for any SO related class or reference
    #region Validation
#if UNITY_EDITOR
    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(playerCharacterName), playerCharacterName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerPrefab), playerPrefab);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(playerHealthAmount), playerHealthAmount, false);
        HelperUtilities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerMinimapIcon), playerMinimapIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);
    }
#endif
    #endregion
}
