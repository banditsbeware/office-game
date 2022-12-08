using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class otherButtonSingle : MonoBehaviour
{
    private bool pressed;
    private Image img;
    private Color32 unpressedColor;
    [SerializeField] private Color32 pressedColor;

    void Start()
    {
        img = GetComponent<Image>();
        unpressedColor = img.color;
        ErrorPuzzle.OnComplete += ResetOtherButtons;
        ErrorPuzzle.OnFailed += ResetOtherButtons;
    }

    public void OnPress()
    {
        pressed = !pressed;

        if (pressed)
        {
            img.color = pressedColor;

            //checks if these wires complete current printer task
            ErrorPuzzle ce = Printer.currentError;
            ce.Completed(ce.currentTask().Check(gameObject.name, true));
        }
        else
        {
            img.color = unpressedColor;
            ErrorPuzzle ce = Printer.currentError;
            ce.Completed(ce.currentTask().Check(gameObject.name, false));
        }   
    }

    private void ResetOtherButtons(object sender, EventArgs e)
    {
        pressed = false;
        img.color = unpressedColor;
    }
}
