using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    [SerializeField] string startMenu;
    [SerializeField] List<Menu> InSceneMenus;

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
