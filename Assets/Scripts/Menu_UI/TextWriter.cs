using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//this is direct from this tutorial: https://www.youtube.com/watch?v=ZVh4nH8Mayg
public class TextWriter : MonoBehaviour
{
    private static TextWriter instance;
    private List<TextWriterSingle> writerList;

    private void Awake()
    {
        instance = this;
        writerList = new List<TextWriterSingle>();
    }

    public static void AddWriter_Static(TMP_Text uiText, string text, float time)
    {
        instance?.AddWriter(uiText, text, time);
    }

    public static void AddWriter_Static(TMP_Text uiText, string text)
    {
        instance?.AddWriter(uiText, text);
    }

    public void AddWriter(TMP_Text uiText, string text, float time)
    {
        writerList.Add(new TextWriterSingle(uiText, text, time));
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

        public TextWriterSingle(TMP_Text uiText, string text, float time)
        {
            this.uiText = uiText;
            textToWrite = text;
            timePerChar = time;
            invisChars = true;
            charIndex = 0;
            timer = -.1f; 
        }
        
        public TextWriterSingle(TMP_Text uiText, string text)
        {
            this.uiText = uiText;
            textToWrite = text;
            timePerChar = .05f;
            invisChars = true;
            charIndex = 0;    
            timer = 0f;   
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
                    text += " <alpha=#00>" + textToWrite.Substring(charIndex) + "</alpha>";
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
