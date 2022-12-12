using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class memo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Transform sheet;
    private Vector3 originalPosition;

    void Start()
    {
        sheet = transform.Find("sheet");
        originalPosition = sheet.localPosition;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        sheet.localScale = new Vector3(2, 2, 2);
        sheet.localPosition = new Vector3(0, 0, 0);
        transform.SetAsLastSibling();

    }
    public void OnPointerExit(PointerEventData eventData)
    {
        sheet.localScale = new Vector3(1, 1, 1);
        sheet.localPosition = new Vector3(0, originalPosition.y, 0);
    }
}
