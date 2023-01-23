using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is a subscriber of the AimWeaponEvent
[RequireComponent(typeof(AimWeaponEvent))]
[DisallowMultipleComponent]
public class AimWeapon : MonoBehaviour
{
    [Tooltip("Create with the transform from the child WeaponRotationPoint gameobject")]
    [SerializeField] private Transform weaponRotationPointTransform;

    // A cashed memeber variable
    private AimWeaponEvent aimWeaponEvent;

    private void Awake() 
    {
        // Load components
        aimWeaponEvent = GetComponent<AimWeaponEvent>();
    }


    // Here is the best place to subscribe an event
    private void OnEnable() 
    {
        // Subscribe to aim weapon event
        aimWeaponEvent.OnWeaponAim += AimWeaponEvent_OnWeaponAim; // You can also use "AddEventListener(), same thing"
    }

    // Here is the best place to unsubscribe a event (Always remember to unsubscribe an event when it's disabled)
    private void OnDisable() 
    {
        // Unsubscribe from aim weapon event
        aimWeaponEvent.OnWeaponAim -= AimWeaponEvent_OnWeaponAim;
    }

    /// <summary>
    /// Aim weapon event handler
    /// </summary>
    private void AimWeaponEvent_OnWeaponAim(AimWeaponEvent aimWeaponEvent, AimWeaponEventArgs aimWeaponEventArgs)
    {
        Aim(aimWeaponEventArgs.aimDirection, aimWeaponEventArgs.aimAngle);
    }

    /// <summary>
    /// Aim the weapon
    /// </summary>
    private void Aim(AimDirection aimDirection, float aimAngle)
    {
        // Set angle of the weapon transform on Z-axis
        weaponRotationPointTransform.eulerAngles = new Vector3(0f, 0f, aimAngle);

        // Flip weapon transform based on player direction
        switch (aimDirection)
        {
            case AimDirection.Left:
            case AimDirection.UpLeft:
                weaponRotationPointTransform.localScale = new Vector3(1f, -1f, 0f); // Flip on Y-axis
                break;
            
            case AimDirection.Up:
            case AimDirection.UpRight:
            case AimDirection.Right:
            case AimDirection.Down:
                weaponRotationPointTransform.localScale = new Vector3(1f, 1f, 0f); // Set it back to normal or stay normal
                break;
        }
    }


    #region Validation
    // Compiler directive: only runs in the unity editor
#if UNITY_EDITOR
    /// <summary>
    /// Editor-only function that Unity calls when the script is loaded or a value changes in the inspector
    /// </summary>
    private void OnValidate()
    {
        HelperUtilities.ValidateCheckNullValue(this, nameof(weaponRotationPointTransform), weaponRotationPointTransform);
    }
#endif
    #endregion
}
