using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

[DisallowMultipleComponent]
public class ChangeAbilityEvent : MonoBehaviour
{
    public event Action<ChangeAbilityEvent, ChangeAbilityEventArgs> OnChangeAbility;

    public void CallOnChangeAbilityEvent(AbilitySO previousAbility, AbilitySO newAbility)
    {
        OnChangeAbility?.Invoke(this, new ChangeAbilityEventArgs()
        {
            previousAbility = previousAbility,
            newAbility = newAbility
        });
    }
}

public class ChangeAbilityEventArgs : EventArgs
{
    public AbilitySO previousAbility;
    public AbilitySO newAbility;
}
