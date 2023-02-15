using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class UseAbilityEvent : MonoBehaviour
{
    public event Action<UseAbilityEvent, UseAbilityEventArgs> OnUseAbility;

    public void CallOnUseAbilityEvent(bool use, AimDirection aimDirection, float aimAngle, float weaponAimAngle, Vector3 weaponAimDirectionVector)
    {
        OnUseAbility?.Invoke(this, new UseAbilityEventArgs()
        {
            use = use,
            aimDirection = aimDirection,
            aimAngle = aimAngle,
            weaponAimAngle = weaponAimAngle,
            weaponAimDirectionVector = weaponAimDirectionVector
        });
    }
}

public class UseAbilityEventArgs : EventArgs
{
    public bool use;
    public AimDirection aimDirection;
    public float aimAngle; // Player aim angle
    public float weaponAimAngle; // Weapon aim angle
    public Vector3 weaponAimDirectionVector;
}
