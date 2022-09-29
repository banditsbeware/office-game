using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Work : MonoBehaviour
{
    private char nextLetter;
    private bool completed = false;
    private int charIndex;
    private int lettersTyped;
    private bool backRequired = false;

    [SerializeField] private TMP_Text baseText;
    [SerializeField] private TMP_Text topText;
     [SerializeField] private TMP_Text test;
    [SerializeField] private TextWriter textWriter;
    [TextArea] [SerializeField] private string phrase;

    void OnEnable()
    {
        beginPhrase();
    }

    void beginPhrase()
    {
        textWriter.AddWriter(baseText, phrase);
        topText.text = "";
        nextLetter = phrase[0];
    }

    void Update()
    {
        if (!completed && !backRequired && Input.inputString != "")
        {
            foreach (char c in Input.inputString)
            {
                if (c == nextLetter)
                {
                    charIndex++;
                    nextLetter = phrase[charIndex];
                    topText.text = "<color=green>" + phrase.Substring(0, charIndex) + "</color>" + "<alpha=#00>" + phrase.Substring(charIndex) + "</alpha>";
                }
                else
                {
                    topText.text = "<color=green>" + phrase.Substring(0, charIndex) + "</color><color=red><mark=#ff000080>" + phrase.Substring(charIndex, 1)
                    + "</mark></color>" + "<alpha=#00>" + phrase.Substring(charIndex) + "</alpha>";

                    backRequired = true;
                }
            }
        }
        else if (backRequired && Input.GetKeyDown(KeyCode.Backspace))
        {
            backRequired = false;
            topText.text = "<color=green>" + phrase.Substring(0, charIndex) + "</color>" + "<alpha=#00>" + phrase.Substring(charIndex) + "</alpha>";
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
