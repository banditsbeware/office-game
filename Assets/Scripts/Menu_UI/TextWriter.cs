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

    public static void ClearAllWriters_Static() {
        for (int i = 0; i < instance?.writerList.Count; i++)
        {
                instance?.writerList.RemoveAt(i);
        }
    }

    public class TextWriterSingle 
    {
        private TMP_Text uiText;
        public string textToWrite;
        private int charIndex;
        private float timePerChar;
        private float timer;

        public TextWriterSingle(TMP_Text uiText, string text, float time)
        {
            this.uiText = uiText;
            textToWrite = text;
            timePerChar = time;
            charIndex = 0;
            timer = -.1f; 
        }
        
        public TextWriterSingle(TMP_Text uiText, string text)
        {
            this.uiText = uiText;
            textToWrite = text;
            timePerChar = .05f;
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
                text += " <color=#00000000>" + textToWrite.Substring(charIndex) + "</color>";
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
