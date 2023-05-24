using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public abstract class workWindow : MonoBehaviour, IPointerDownHandler
{
    //reference to minigame script
    protected work work_ref;

    //window parameters
    protected Image imageComponent;
    protected Color32 defaultWindowColor = new Color32(0, 0, 0, 255);
    protected byte transparentAlpha = 200;
    protected List<Image> images;
    protected List<TMP_Text> texts;

    //window contents
    protected Phrase activePhrase;
    protected int cursorFrames = 25;

    public virtual void Start() {
        work_ref = transform.parent.parent.GetComponent<work>(); 

        imageComponent = gameObject.GetComponent<Image>();
        
        UpdateColoredObjects();
        
        SetColorToDefault();

        transform.SetAsLastSibling();
        work.activeWindow = this;
    }

    private void FixedUpdate() 
    {
        if(activePhrase != null) 
        {
            if (cursorFrames > 0)
            {
                cursorFrames -= 1;
                return;
            }
    
            cursorFrames = 25;
            string drawnText = activePhrase.cursorText.text;
            if (drawnText.Length > 0)
            {
                if (drawnText[drawnText.Length - 1] == '|')
                {
                    activePhrase.cursorText.text = drawnText.Remove(drawnText.Length - 1);
                    return;
                }
            }
            activePhrase.cursorText.text += '|';
        }
    }

    public void SetColorTransparent()
    {
        foreach (Image img in images)
        {
            Color32 clr = img.color;
            img.color = new Color32(clr.r, clr.g, clr.b, transparentAlpha);
        }
        foreach (TMP_Text txt in texts)
        {
            Color32 clr = txt.color;
            txt.color = new Color32(clr.r, clr.g, clr.b, transparentAlpha);
        }
    }
    public void SetColorToDefault()
    {
        foreach (Image img in images)
        {
            Color32 clr = img.color;
            img.color = new Color32(clr.r, clr.g, clr.b, 255);
        }
        foreach (TMP_Text txt in texts)
        {
            Color32 clr = txt.color;
            txt.color = new Color32(clr.r, clr.g, clr.b, 255);
        }
    }

    public abstract void InputRecieved(string input);

    public abstract void PhraseComplete();

    public abstract void Back();

    public abstract void Enter();

    public abstract void Complete();
    

    public void OnPointerDown(PointerEventData eventData)
    {
        work.activeWindow = this;
        transform.SetAsLastSibling();
    }

    public void UpdateColoredObjects()
    {
        images = gameObject.GetComponentsInChildren<Image>().ToList();
        images.Add(imageComponent);
        texts = gameObject.GetComponentsInChildren<TMP_Text>().ToList();
    }

    protected IEnumerator FadeDestroy(float delayTime = 0f)
    {
        List<Image> images = gameObject.GetComponentsInChildren<Image>().ToList();
        images.Add(imageComponent);
        List<TMP_Text> texts = gameObject.GetComponentsInChildren<TMP_Text>().ToList();

        byte alp = 255;

        yield return new WaitForSeconds(delayTime);

        while (imageComponent.color.a > 0f)
        {
            alp = (byte) ((float) alp - 51);
            
            foreach (Image img in images)
            {
                Color32 clr = img.color;
                img.color = new Color32(clr.r, clr.g, clr.b, alp);
            }
            foreach (TMP_Text txt in texts)
            {
                Color32 clr = txt.color;
                txt.color = new Color32(clr.r, clr.g, clr.b, alp);
            }

            yield return new WaitForSeconds(.2f);
        }
        
        GameObject.Destroy(gameObject);
    }

    public void Destroy()
    {
        GameObject.Destroy(gameObject);
    }




    public class Phrase  //Only within workWindow because somehow Instantiate works if this class is a child of workWindow
    {
        public Transform parent;
        public workWindow parentWindow;
        public TMP_Text baseText;
        public TMP_Text topText;
        public TMP_Text cursorText;
        public string words;
        public string typed = "";

        //tracking contents
        public bool backRequired = false;
        public char nextLetter;
        public int charIndex = 0;
        public int selectionIndex = -1;  //selection index is ++ed before comparisons are made, charIndex is only ++ed in certain circumstances

        public Phrase(string w, TMP_Text template, Transform parent, workWindow parentWindow, bool withCursor = true)
        {
            this.parent = parent;
            this.parentWindow = parentWindow;

            this.words = w;
            this.baseText = Instantiate<TMP_Text>(template, parent);
            this.baseText.text = words;
            this.topText = Instantiate<TMP_Text>(template, parent);
            if (withCursor)
            {
                this.cursorText = Instantiate<TMP_Text>(template, parent);
                this.cursorText.rectTransform.localPosition = new Vector3(this.cursorText.rectTransform.localPosition.x - 5, this.cursorText.rectTransform.localPosition.y, 0);
            }

            parentWindow.UpdateColoredObjects();

            this.nextLetter = words[0];
        }

        public Phrase(string w, Vector3 pos, TMP_Text template, Transform parent, workWindow parentWindow)
        {
            this.parent = parent;
            this.parentWindow = parentWindow;

            this.words = w;
            this.baseText = Instantiate<TMP_Text>(template, parent);
            this.baseText.rectTransform.localPosition = pos;
            this.baseText.text = words;
            this.topText = Instantiate<TMP_Text>(template, parent);
            this.topText.rectTransform.localPosition = pos;

            parentWindow.UpdateColoredObjects();

            this.nextLetter = words[0];
        }

        public void CheckInput(string input, bool skipEnter = false)
        {
            foreach (char c in input)
            {
                if (c == '\b') continue;
                if (c == '\n' && skipEnter) continue;

                typed += c;
                selectionIndex++;

                if(selectionIndex == charIndex) 
                {
                    if (c == nextLetter)
                    {
                        charIndex++;
                        topText.text = "<color=green>" + typed + "</color>";
                        UpdateCursorPos();

                        if (charIndex == words.Length)
                        {
                            parentWindow.PhraseComplete();
                            return;
                        }

                        nextLetter = words[charIndex];
                        return;
                    }
                }
                
                topText.text = "<color=green>" + words.Substring(0, charIndex) + "</color><color=red><mark=#ff000080>" + typed.Substring(charIndex)
                + "</mark></color>";
                UpdateCursorPos();
            }
        }

        public void UpdateCursorPos()
        {
            if (cursorText != null)
            {
                cursorText.text = "<color=#00000000>" + typed + "</color>";
            }
        }

        public void Backspace()
        {
            if(typed.Length > 0) 
            {
                typed = typed.Remove(typed.Length - 1, 1);
                selectionIndex--;
                UpdateCursorPos();

                if(selectionIndex == charIndex - 2)  //you have no missed chars
                {
                    charIndex--;
                    nextLetter = words[charIndex];

                    topText.text = "<color=green>" + words.Substring(0, charIndex) + "</color>";
                    return;
                }
                
                topText.text = "<color=green>" + words.Substring(0, charIndex) + "</color><color=red><mark=#ff000080>" + typed.Substring(charIndex)
                + "</mark></color>";
            }
        }

        public void Solifify()
        {
            GameObject.Destroy(cursorText.gameObject);
            GameObject.Destroy(topText.gameObject);
        }

        IEnumerator fadeOutText()
        {
            byte alp = 255;

            GameObject.Destroy(baseText.gameObject);

            while (topText.color.a > 0f)
            {
                alp = (byte) ((float) alp - 51);
                
                topText.color = new Color32(255, 255, 255, alp);
                yield return new WaitForSeconds(.2f);
            }
            
            GameObject.Destroy(topText.gameObject);
        }
    }
}

