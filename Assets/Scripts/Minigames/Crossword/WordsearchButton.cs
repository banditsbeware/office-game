using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SpeakEasy;

public class WordsearchButton : MonoBehaviour, IPointerEnterHandler
{
    public bool usedByOtherWord = false;
    private bool selected = false;
    public (int, int) coords;

    public void SelectLetter()
    {
        if (!selected)
        {
            selected = true;
            GetComponent<Image>().color = new Color32(200, 100, 100, 200);
            transform.parent.transform.parent.GetComponent<CrosswordDialogue>().Selection(gameObject);
        }
    }

    public void Reset()
    {
        selected = false;
        GetComponent<Image>().color = new Color32(255, 255, 255, 30);
    }

    public void OnPointerEnter(PointerEventData pointerData)
    {
        AkSoundEngine.PostEvent("Play_UI_select_03", gameObject);
    }
}   
