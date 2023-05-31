using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;
using static workWindow;

public class sheetsCell : MonoBehaviour, IPointerDownHandler
{
    private workSheet parentWindow;
    private Image cellImage;
    public Phrase cellPhrase;
    public bool complete;

    private Color32 defaultColor;
    private Color32 selectedColor;
    [SerializeField] private Color32 completedColor;

    void Awake()
    {
        parentWindow = transform.parent.parent.GetComponent<workSheet>();

        cellImage = GetComponent<Image>();
        defaultColor = cellImage.color;
        selectedColor = new Color32((byte) ((int) defaultColor.r - 15), defaultColor.g, defaultColor.b, defaultColor.a);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        parentWindow.OnPointerDown(eventData);

        if(!complete) 
        {
            parentWindow.CellSelected(this);
        }
    }

    public void Deselect()
    {
        cellImage.color = defaultColor;
        
        if(cellPhrase != null) 
        {
            cellPhrase.RemoveCursor();
        }
    }

    public void SelectCell()
    {
        cellImage.color = selectedColor;
    }

    public void cellComplete()
    {
        cellPhrase.Solifify();
        cellImage.color = completedColor;
        complete = true;
    }
}
