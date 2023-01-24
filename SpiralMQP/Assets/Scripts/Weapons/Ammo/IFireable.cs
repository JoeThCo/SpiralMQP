using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFireable 
{
    // All fireable ammo that implements this interface should have the following functions
    // Initialize the ammo with specified input
    void InitializeAmmo(AmmoDetailsSO ammoDetails, float aimAngle, float weaponAimAngle, float ammoSpeed, Vector3 weaponAimDirectionVector, bool overrideAmmoMovement = false);

    // Return the ammo game object
    GameObject GetGameObject();
}
