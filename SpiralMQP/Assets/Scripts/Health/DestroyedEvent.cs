using System;
using UnityEngine;

[DisallowMultipleComponent]
public class DestroyedEvent : MonoBehaviour
{
    public event Action<DestroyedEvent, DestroyedEventArgs> OnDestroyed;

    public void CallDestroyedEvent(bool playerDied, int soulCount)
    {
        OnDestroyed?.Invoke(this, new DestroyedEventArgs() { playerDied = playerDied, soulCount = soulCount});
    }
}

public class DestroyedEventArgs : EventArgs
{
    public bool playerDied;
    public int soulCount;
}

