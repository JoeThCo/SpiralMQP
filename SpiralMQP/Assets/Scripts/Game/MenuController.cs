using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] private string startMenu;
    [SerializeField] private List<Menu> InSceneMenus;

    private void Start()
    {
        ShowMenu(startMenu);
    }

    public void ShowMenu(string s)
    {
        foreach (Menu m in InSceneMenus)
        {
            m.gameObject.SetActive(m.GetMenuName().Equals(s));
        }
    }
}
