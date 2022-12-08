using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class numPad : MonoBehaviour
{
    [SerializeField] private TMP_Text numScreen;
    private int number;

    void Start()
    {
        ErrorPuzzle.OnFailed += ResetNumbers;
    }

    //called by button component on each key
    public void keypadInput(int num)
    {
        if (numScreen.text.Length < 8)
        {
            numScreen.text += num;
            number = int.Parse(numScreen.text);
        }
        
    }

    //called by back button
    public void back()
    {
        if (numScreen.text.Length > 0)
        {
            numScreen.text = numScreen.text.Remove(numScreen.text.Length - 1);
            number = int.Parse(numScreen.text);
        }
    }

    //called by enter button
    public void enter()
    {
        //checks if these number completes current printer task
        ErrorPuzzle ce = Printer.currentError;
        ce.Completed(ce.currentTask().Check(number));

        ResetNumbers(this, EventArgs.Empty);
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

    private void ResetNumbers(object sender, EventArgs e)
    {
        number = 0;
        numScreen.text = "";
    }
}
