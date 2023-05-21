using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class workWindow : MonoBehaviour, IPointerDownHandler
{
    //window contents
    public List<string> possiblePhrases;
    public List<string> onScreenPhrases;
    public string activePhrase;
    public bool backRequired;

    //window parameters
    protected Image imageComponent;
    protected Color32 defaultWindowColor = new Color32(255, 121, 121, 255);
    protected Color32 transparentWindowColor;

    public void Start() {
        imageComponent = gameObject.GetComponent<Image>();
        SetColorToDefault();

        transparentWindowColor = defaultWindowColor;
        transparentWindowColor.a = 200;

        
    }

    public void SetColorTransparent()
    {
        imageComponent.color = transparentWindowColor;
    }
    public void SetColorToDefault()
    {
        imageComponent.color = defaultWindowColor;
    }

    public virtual void Complete()
    {
        return;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        transform.SetAsLastSibling();
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }
}

