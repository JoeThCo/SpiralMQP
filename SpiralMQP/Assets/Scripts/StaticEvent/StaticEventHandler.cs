using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public static class StaticEventHandler
{
    // Room change event
    public static event Action<RoomChangedEventArgs> OnRoomChanged;

    public static void CallRoomChangedEvent(Room room)
    {
        OnRoomChanged?.Invoke(new RoomChangedEventArgs() { room = room });
    }


    // Room enemies defeated event
    public static event Action<RoomEnemiesDefeatedArgs> OnRoomEnemiesDefeated;

    public static void CallRoomEnemiesDefeatedEvent(Room room)
    {
        OnRoomEnemiesDefeated?.Invoke(new RoomEnemiesDefeatedArgs() { room = room });
    }


    // Soul Collected event - when a enemy is defeated dreamy will collect souls from them based on enemy's health
    public static event Action<SoulsCollectedArgs> OnSoulsCollected;

    public static void CallSoulsCollectedEvent(int soulCount)
    {
        OnSoulsCollected?.Invoke(new SoulsCollectedArgs() { soulCount = soulCount });
    }


    // Score changed event
    public static event Action<SoulAmountChangedArgs> OnSoulAmountChanged;

    public static void CallSoulAmountChangedEvent(long soulAmount, int multiplier)
    {
        OnSoulAmountChanged?.Invoke(new SoulAmountChangedArgs() { soulAmount = soulAmount, multiplier = multiplier });
    }


    // Multiplier event
    public static event Action<MultiplierArgs> OnMultiplier;

    public static void CallMultiplierEvent(bool multiplier)
    {
        OnMultiplier?.Invoke(new MultiplierArgs() { multiplier = multiplier });
    }
}

public class RoomChangedEventArgs : EventArgs
{
    public Room room;
}

public class RoomEnemiesDefeatedArgs : EventArgs
{
    public Room room;
}

public class SoulsCollectedArgs : EventArgs
{
    public int soulCount;
}

public class SoulAmountChangedArgs : EventArgs
{
    public long soulAmount; // Total soul amount
    public int multiplier;
}

public class MultiplierArgs : EventArgs
{
    // When true - increase the multiplier
    // When false - decrease the multiplier
    public bool multiplier;
}
