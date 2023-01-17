using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CurrentPlayer", menuName = "Scriptable Objects/Player/Current Player")]
public class CurrentPlayerSO : ScriptableObject
{
   public PlayerDetailsSO playerDetails; // Holds the selected player detail info 
   public string playerName; // This is actual player's choice of name
}
