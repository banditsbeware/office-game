using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuOption : MonoBehaviour
{
    [SerializeField] private GameObject submenu;
    private static GameObject[] submenus = new GameObject[0];
    
    void Start()
    {
        if (submenus.Length == 0)
        {
            submenus = GameObject.FindGameObjectsWithTag("submenu");
            UIManager.hide(submenus);
            Debug.Log("nice!");
        }
    }

    public void enableSubmenu()
    {
        UIManager.hide(submenus);
        submenu.SetActive(true);
    }

}
