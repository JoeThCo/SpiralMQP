using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class FireWeaponEvent : MonoBehaviour
{
    public event Action<FireWeaponEvent, FireWeaponEventArgs> OnFireWeapon;

    public void CallFireWeaponEvent(bool fire, bool firePreviousFrame, AimDirection aimDirection, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        OnFireWeapon?.Invoke(this, new FireWeaponEventArgs()
        {
            fire = fire,
            firePreviousFrame = firePreviousFrame,
            aimDirection = aimDirection,
            aimAngle = aimAngle,
            weaponAimAngle = weaponAimAngle,
            weaponAimDirectionVector = weaponAimDirectionVector
        });
    }
}

public class FireWeaponEventArgs : EventArgs
{
    public bool fire;
    public bool firePreviousFrame;
    public AimDirection aimDirection;
    public float aimAngle; // Player aim angle
    public float weaponAimAngle; // Weapon aim angle
    public Vector3 weaponAimDirectionVector;
}
