using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class TextWriter : MonoBehaviour
{
    private static TextWriter instance;
    private List<TextWriterSingle> writerList;

    private void Awake()
    {
        instance = this;
        writerList = new List<TextWriterSingle>();
    }

    public static void AddWriter_Static(TMP_Text uiText, string text, float time, bool invisChars)
    {
        instance?.AddWriter(uiText, text, time, invisChars);
    }

    public static void AddWriter_Static(TMP_Text uiText, string text)
    {
        instance?.AddWriter(uiText, text);
    }

    public void AddWriter(TMP_Text uiText, string text, float time, bool invisChars)
    {
        writerList.Add(new TextWriterSingle(uiText, text, time, invisChars));
    }

    public void AddWriter(TMP_Text uiText, string text)
    {
        writerList.Add(new TextWriterSingle(uiText, text));
    }

    private void Update() {
        for (int i = 0; i < writerList.Count; i++)
        {
            bool destroyInstance = writerList[i].Update();
            if (destroyInstance)
            {
                writerList.RemoveAt(i);
                i--;
            }
        }
    }

    public class TextWriterSingle 
    {
        private TMP_Text uiText;
        public string textToWrite;
        private int charIndex;
        private float timePerChar;
        private float timer;
        bool invisChars;

        public TextWriterSingle(TMP_Text uiText, string text, float time, bool invisChars)
        {
            this.uiText = uiText;
            this.textToWrite = text;
            this.timePerChar = time;
            this.invisChars = invisChars;
            charIndex = 0;
        }
        public TextWriterSingle(TMP_Text uiText, string text)
        {
            this.uiText = uiText;
            this.textToWrite = text;
            timePerChar = .05f;
            invisChars = true;
            charIndex = 0;       
        }
        
        //return true on isComplete
        public bool Update()
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
                    text += "<alpha=#00>" + textToWrite.Substring(charIndex) + "</alpha>";
                }
                uiText.text = text;

                if (charIndex >= textToWrite.Length)
                {
                    uiText = null;
                    return true;
                }
            }
            return false;
        }
    }
}
