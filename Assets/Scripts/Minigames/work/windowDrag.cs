using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class windowDrag : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform windowTransform;
    private Canvas canvas;
    private workWindow window;


    private void Start() {
        //get reference to window
        windowTransform = transform.parent.GetComponent<RectTransform>();
        window = transform.parent.GetComponent<workWindow>();

        //get reference to workMini canvas
        canvas = windowTransform.parent.parent.GetComponent<Canvas>();
    }
    public void OnDrag(PointerEventData eventData)
    {
        windowTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        window.SetColorTransparent();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        window.SetColorToDefault();
        if (!GameObject.ReferenceEquals(eventData.pointerCurrentRaycast.gameObject, gameObject))
        {
            window.DestroyWindow();
        }
    }
}
