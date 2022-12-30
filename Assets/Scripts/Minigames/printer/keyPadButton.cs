using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class keyPadButton : MonoBehaviour
{
    private TMP_Text buttonText;
    private Image buttonImg;
    private Sprite upButton;
    private string buttonContents;
    private numPad pad;

    void Start()
    {
        buttonContents = gameObject.name;

        buttonText = GetComponentInChildren<TMP_Text>();
        buttonImg = GetComponentInChildren<Image>();
        upButton = buttonImg.sprite;

        buttonText.text = buttonContents;

        pad = GetComponentInParent<numPad>();
    }

    public void Clicky()
    {
        StartCoroutine(clickAnimation());
        if (buttonContents == "enter")
        {
            pad.enter();
        } 
        else if (buttonContents == "back")
        {
            pad.back();
        }
        else
        {
            pad.keypadInput(int.Parse(buttonContents));
        }
    }

    IEnumerator clickAnimation()
    {
        buttonImg.color = new Color(buttonImg.color.r, buttonImg.color.g, buttonImg.color.b, 0f);;
        buttonText.GetComponent<RectTransform>().localPosition += new Vector3(0, -5, 0);
        yield return new WaitForSeconds(.2f);
        buttonImg.color = new Color(buttonImg.color.r, buttonImg.color.g, buttonImg.color.b, 1f);;
        buttonText.GetComponent<RectTransform>().localPosition += new Vector3(0, 5, 0);
    }
}
