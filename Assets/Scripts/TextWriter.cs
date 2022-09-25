using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextWriter : MonoBehaviour
{
    private TMP_Text uiText;
    private string textToWrite;
    private int charIndex;
    private float timePerChar;
    private float timer;
    bool invisChars;
    public void AddWriter(TMP_Text uiText, string text, float time, bool invisChars)
    {
        this.uiText = uiText;
        this.textToWrite = text;
        this.timePerChar = time;
        this.invisChars = invisChars;
        charIndex = 0;
    }

    public void AddWriter(TMP_Text uiText, string text)
    {
        this.uiText = uiText;
        this.textToWrite = text;
        timePerChar = .1f;
        invisChars = true;
        charIndex = 0;
    }

    private void Update()
    {
        if (uiText != null)
        {
            timer -= Time.deltaTime;
            while (timer <= 0f)
            {
                //display next char
                timer += timePerChar;
                charIndex++;
                string text = textToWrite.Substring(0, charIndex);
                if (invisChars)
                {
                    text += "<alpha=#00>" + textToWrite.Substring(charIndex) + "</color>";
                }
                uiText.text = text;

                if (charIndex >= textToWrite.Length)
                {
                    uiText = null;
                    return;
                }
            }
        }
    }
}
