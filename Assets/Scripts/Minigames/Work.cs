using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Work : MonoBehaviour
{
    private char nextLetter;
    private string completed;
    private int charIndex;
    private int lettersTyped;
    [SerializeField] private TMP_Text computerText;
    [SerializeField] private TextWriter textWriter;
    [TextArea] [SerializeField] private string phrase;

    void OnEnable()
    {
        beginPhrase();
        Debug.Log("Work begun");
    }

    void beginPhrase()
    {
        textWriter.AddWriter(computerText, phrase);
        nextLetter = phrase[0];

    }

    void Update()
    {
        if (Input.inputString != "")
        {
            foreach (char c in Input.inputString)
            {
                if (c == nextLetter)
                {

                }
            }
        }
        
        

        // charIndex++;
        // completed = phrase.Substring(0, charIndex);
        // if (invisChars)
        //         {
        //             text += "<alpha=#00>" + textToWrite.Substring(charIndex) + "</color>";
        //         }
        //         uiText.text = text;

        //         if (charIndex >= textToWrite.Length)
        //         {
        //             uiText = null;
        //             return;
        //         }
    }

    
}
