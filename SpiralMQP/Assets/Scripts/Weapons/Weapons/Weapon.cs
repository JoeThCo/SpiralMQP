using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    public WeaponDetailsSO weaponDetails;
    public int weaponListPosition; // The position of the weapon in the weapon list
    public float weaponReloadTimer;
    public int weaponClipRemainingAmmo;
    public int weaponRemainingAmmo;
    public bool isWeaponReloading;
}
