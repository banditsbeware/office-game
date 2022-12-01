using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class numPad : MonoBehaviour
{
    public TMP_Text numScreen;

    //called bu button component on each key
    public void keypadInput(int num)
    {
        if (numScreen.text.Length < 8)
        {
            numScreen.text += num;
        }
        
    }

    //called by back button
    public void back()
    {
        if (numScreen.text.Length > 0)
        {
            numScreen.text = numScreen.text.Remove(numScreen.text.Length - 1);
        }
    }

    //called by enter button
    public void enter()
    {
        numScreen.text = "";
    }

    void Update()
    {
        //translates keyboard input onto keypad
        foreach (char c in Input.inputString)
            {
                if (Char.IsDigit(c))
                {
                    keypadInput((int) Char.GetNumericValue(c));
                }
            }
    }
}
