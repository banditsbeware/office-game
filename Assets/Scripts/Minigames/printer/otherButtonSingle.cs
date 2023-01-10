using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class otherButtonSingle : MonoBehaviour
{
    private bool pressed;
    private Image img;

    private TMP_Text buttonText;
    [SerializeField] private Sprite downSprite;
    [SerializeField] private Sprite onSprite;
    [SerializeField] private Sprite offSprite;

    void Start()
    {
        pressed = false;
        img = GetComponent<Image>();
        buttonText = GetComponentInChildren<TMP_Text>();
        ErrorPuzzle.OnComplete += ResetOtherButtons;
        ErrorPuzzle.OnFailed += ResetOtherButtons;
    }

    public void OnPress()
    {
        if (pressed)
        {
            StartCoroutine(pressedOff());
            //checks if these wires complete current printer task
            AkSoundEngine.PostEvent("Play_printer_click_other_off", gameObject);
            ErrorPuzzle ce = Printer.currentError;
            ce.Completed(ce.currentTask().Check(gameObject.name, false));
        }
        else
        {
            StartCoroutine(pressedOn());
            AkSoundEngine.PostEvent("Play_printer_click_other_on", gameObject);
            ErrorPuzzle ce = Printer.currentError;
            ce.Completed(ce.currentTask().Check(gameObject.name, true));
        }   
    }

    private void ResetOtherButtons(object sender, EventArgs e)
    {
        pressed = false;
        img.sprite = offSprite;
    }

    IEnumerator pressedOn()
    {
        pressed = true;
        buttonText.GetComponent<RectTransform>().localPosition += new Vector3(0, -5, 0);
        img.sprite = downSprite;
        yield return new WaitForSeconds(.3f);
        buttonText.GetComponent<RectTransform>().localPosition += new Vector3(0, 5, 0);
        if (pressed)
        {
            img.sprite = onSprite;
        }
        else
        {
            img.sprite = offSprite;
        }
        

    }
    IEnumerator pressedOff()
    {
        pressed = false;
        buttonText.GetComponent<RectTransform>().localPosition += new Vector3(0, -5, 0);
        img.sprite = downSprite;
        yield return new WaitForSeconds(.3f);
        buttonText.GetComponent<RectTransform>().localPosition += new Vector3(0, 5, 0);
        img.sprite = offSprite;
        
    }
}
