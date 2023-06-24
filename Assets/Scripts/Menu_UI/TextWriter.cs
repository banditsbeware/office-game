using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//this is direct from this tutorial: https://www.youtube.com/watch?v=ZVh4nH8Mayg
public class TextWriter : MonoBehaviour {

    private static TextWriter       instance;
    private List<TextWriterSingle>  writerList;

    private void Awake() 
    {
        instance = this;
        writerList = new List<TextWriterSingle>();
    }

    /// AddWriter
    /// Creates a new TextWriterSingle
    public static void AddWriter(TMP_Text   uiText
                                ,string     text
                                ,float      dt=0.05f
                                ,bool       invis=true) 
    {
        instance._AddWriter(uiText, text, dt, invis);
    }

    private void _AddWriter(TMP_Text    uiText
                           ,string      text
                           ,float       dt=0.05f 
                           ,bool        invis=true) 
    {
        writerList.Add(new TextWriterSingle(uiText, text, dt, invis));
    }

    private void Update() 
    {
        // iterate over writerList each frame
        for (int i = 0; i < writerList.Count; i++) {

            // true if the writer is finished printing its text
            bool destroyInstance = writerList[i].Update();

            if (destroyInstance) {
                writerList.RemoveAt(i);
                i--;
            }
        }
    }

    private class TextWriterSingle 
    {
        private TMP_Text    uiText;
        private string      currentText;
        private string      text;
        private float       dt;
        private bool        invis;
        private int         charIndex;
        private float       timer;

        public TextWriterSingle(TMP_Text    _uiText
                               ,string      _text
                               ,float       _dt=0.05f
                               ,bool        _invis=true)
        {
            uiText      = _uiText;
            text        = _text;
            dt          = _dt;
            invis       = _invis;
            charIndex   = 0;
            timer       = -.1f; 
        }
        
        // Return true when finished writing text
        public bool Update()
        {
            // Time.deltaTime gives the time between frames, i.e. the time
            // since the last call to Update, so use a while loop to account
            // for any characters that "should" have been printed in case
            // of a slow frame rate.
            timer -= Time.deltaTime;
            while (timer <= 0f)
            {
                timer += dt;
                charIndex++;

                // The visible portion of text
                currentText = text.Substring(0, charIndex);

                // Add the rest of text with zero alpha so that the length
                // of the text in the UI is constant throughout printing
                if (invis) currentText += "<alpha=#00>" + text.Substring(charIndex) + "</alpha>";

                // Write text to the UI element
                uiText.text = currentText;

                if (charIndex >= text.Length)
                {
                    uiText = null;
                    return true;
                }
            }
            return false;
        }
    }
}