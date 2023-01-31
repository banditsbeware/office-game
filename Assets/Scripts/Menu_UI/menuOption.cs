using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menuOption : MonoBehaviour
{
    [SerializeField] private GameObject submenu;
    private SaveLoadSystem saveLoad;
    private LevelLoader loader;
    private static GameObject[] submenus = new GameObject[0];
    
    void Start()
    {
        if (submenus.Length == 0)
        {
            submenus = GameObject.FindGameObjectsWithTag("submenu");
            UIManager.hide(submenus);
        }

        saveLoad = GameObject.Find("LevelLoader").GetComponent<SaveLoadSystem>();
        loader = GameObject.Find("LevelLoader").GetComponent<LevelLoader>();
    }

    public void enableSubmenu()
    {
        UIManager.hide(submenus);
        submenu.SetActive(true);
    }

    public void NewGameClicked()
    {
        meta.ResetData();
        loader.LoadLevel(meta.currentScene);
    }
    public void ContinueClicked()
    {
        loader.LoadLevel(meta.currentScene);
    }
}
