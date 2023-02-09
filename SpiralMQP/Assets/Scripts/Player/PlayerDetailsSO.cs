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

    [Tooltip("Select if has immunity period immediately after being hit. If so specify the immunity time in seconds in the other field")]
    public bool isImmuneAfterHit = false;

    [Tooltip("Immunity time in seconds after being hit")]
    public float hitImmunityTime;


    [Space(10)]
    [Header("WEAPON")]
    [Tooltip("Player initial starting weapon")]
    public WeaponDetailsSO startingWeapon; // This is the starting weapon for the player
    [Tooltip("Create with the list of starting weapons")]
    public List<WeaponDetailsSO> startingWeaponList; // We could also have a list of starting weapon and the player can choose one from it


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
        HelperUtilities.ValidateCheckNullValue(this, nameof(startingWeapon), startingWeapon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(runtimeAnimatorController), runtimeAnimatorController);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerMinimapIcon), playerMinimapIcon);
        HelperUtilities.ValidateCheckNullValue(this, nameof(playerHandSprite), playerHandSprite);
        HelperUtilities.ValidateCheckEnumerableValues(this, nameof(startingWeaponList), startingWeaponList);

        if (isImmuneAfterHit)
        {
            HelperUtilities.ValidateCheckPositiveValue(this, nameof(hitImmunityTime), hitImmunityTime, false);
        }
    }
#endif
    #endregion
}
