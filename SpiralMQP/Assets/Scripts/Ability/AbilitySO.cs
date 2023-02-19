using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Ability", menuName = "Scriptable Objects/Ability")]
public class AbilitySO : ScriptableObject
{
    [Space(10)]
    [Header("Ability")]
    [Tooltip("Ability name")]
    public string AbilityName;

    [Tooltip("Ability sprite")]
    public Sprite AbilitySprite;

    [Tooltip("Ability type")]
    public AbilityType AbilityType;

    [Tooltip("Cool Down of this ability")]
    public float AbilityCoolDown;

    [Tooltip("Duration of the ability effect")]
    public float Duration;

    [Tooltip("Strength of a ability")]
    public float EffectLevel;


    #region Validation
#if UNITY_EDITOR
    private void OnValidate() 
    {
        HelperUtilities.ValidateCheckEmptyString(this, nameof(AbilityName), AbilityName);
        HelperUtilities.ValidateCheckNullValue(this, nameof(AbilitySprite), AbilitySprite);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(AbilityCoolDown), AbilityCoolDown, false);
        HelperUtilities.ValidateCheckPositiveValue(this, nameof(Duration), Duration, false);
    }
#endif
    #endregion
}
