using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent] // Good practice
public class Health : MonoBehaviour  // This class is used for anything that needs health
{
    private int startingHealth;
    private int currentHealth;

    /// <summary>
    /// Set starting health
    /// </summary>
    public void SetStartingHealth(int startingHealth)
    {
        this.startingHealth = startingHealth;
        currentHealth = startingHealth;
    }


    /// <summary>
    /// Get the starting health
    /// </summary>
    public int GetStartingHealth()
    {
        return startingHealth;
    }
}
