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
    private Color32 defaultColor;
    private Color32 selectedColor;
    private Image cellImage;

    public Phrase cellPhrase;

    void Start()
    {
        parentWindow = transform.parent.parent.GetComponent<workSheet>();

        cellImage = GetComponent<Image>();
        defaultColor = cellImage.color;
        selectedColor = new Color32((byte) ((int) defaultColor.r - 15), defaultColor.g, defaultColor.b, defaultColor.a);
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        parentWindow.CellSelected(this);
    }

    public void Deselect()
    {
        cellImage.color = defaultColor;
    }

    public void SelectCell()
    {
        cellImage.color = selectedColor;
    }
}
