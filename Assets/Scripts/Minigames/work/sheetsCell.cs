using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class sheetsCell : MonoBehaviour, IPointerDownHandler
{
    private workSheet parentWindow;
    private TMP_Text cellContent;

    void Start()
    {
        parentWindow = transform.parent.parent.GetComponent<workSheet>();
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        parentWindow.activeCell = this;
    }
}
