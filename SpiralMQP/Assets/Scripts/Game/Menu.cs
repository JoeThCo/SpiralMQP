using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] string MenuName;

    public string GetMenuName() { return MenuName; }
}
