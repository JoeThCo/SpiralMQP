using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class AimWeaponEvent : MonoBehaviour
{
    // Create action delegate variables
    public event Action<AimWeaponEvent, AimWeaponEventArgs> OnWeaponAim;

    // A publisher will call this method to notify all its subscribers
    public void CallAimWeaponEvent(AimDirection aimDirection, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        // Use null-conditional operator to safely access Invoke() fucntion on an object that may be null
        // If onWeaponAim is null, no error will be thrown, instead return null
        OnWeaponAim?.Invoke(this, new AimWeaponEventArgs() {aimDirection = aimDirection, aimAngle = aimAngle, weaponAimAngle = weaponAimAngle, weaponAimDirectionVector = weaponAimDirectionVector});
    }
}

// The EventArgs class is a base class for classes that contain event data.
// It is used as a base class for custom event argument classes, which can hold additional information about an event. 
public class AimWeaponEventArgs : EventArgs
{
    public AimDirection aimDirection;
    public float aimAngle; // The angle between the mouse cursor and the pivot point of the player
    public float weaponAimAngle; // The angle between the mouse cursor and the pivot point of the weapon
    public Vector3 weaponAimDirectionVector; // The direction vector used to calculate the "weaponAimAngle"
}
