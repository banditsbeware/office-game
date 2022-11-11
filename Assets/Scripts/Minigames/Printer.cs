using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class Printer : MonoBehaviour
{
    public TMP_Text numScreen;
    public void keypadInput(int num)
    {
        if (numScreen.text.Length < 8)
        {
            numScreen.text += num;
        }
        
    }

    public void back()
    {
        if (numScreen.text.Length > 0)
        {
            numScreen.text = numScreen.text.Remove(numScreen.text.Length - 1);
        }
    }

    public void enter()
    {
        numScreen.text = "";
    }

    void Update()
    {
        foreach (char c in Input.inputString)
            {
                if (Char.IsDigit(c))
                {
                    keypadInput((int) Char.GetNumericValue(c));
                }
            }
    }
}
