using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WordsearchButton : MonoBehaviour
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
            transform.parent.transform.parent.GetComponent<Crossword>().Selection(gameObject);
        }
    }

    public void Reset()
    {
        selected = false;
        GetComponent<Image>().color = new Color32(255, 255, 255, 30);
    }
}   
