using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class menuOption : MonoBehaviour, ISelectHandler, IPointerEnterHandler
{
    private SaveLoadSystem saveLoad;
    private LevelLoader loader;

    [SerializeField] private GameObject submenu; //to open when clicked
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
        loader.LoadLevel(meta.Variables["currentScene"]);
    }
    public void ContinueClicked()
    {
        loader.LoadLevel(meta.Variables["currentScene"]);
    }
    
    public void OnPointerEnter(PointerEventData pointerData)
    {
        AkSoundEngine.PostEvent("Play_UI_select_02", gameObject);
    }

    public void OnSelect(BaseEventData eventData)
    {
        AkSoundEngine.PostEvent("Play_UI_select_05", gameObject);
    }

}
