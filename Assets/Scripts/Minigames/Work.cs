using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class Work : MonoBehaviour
{
    //letter/word tracker
    private char nextLetter;
    private int charIndex;
    private int wordIndex;
    private int lettersTyped;
    private bool backRequired = false;

    //phrases
    public Phrase currentPhrase;
    public static List<string> phrases = new List<string>
    {
        "Return On Investment (ROI)",
        "Synergy",
        "Core Competency",
        "unmatched logistics",
        "holistic customer experience",
        "work ecosystem",
        "restructuring",
        "quota shaping",
        "target consumer",
        "colloquial pricing",
        "cloud-based computing",
        "touch base",
        "shift the paradigm",
        "growth hacking",
        "machine learning algorithm",
        "blockchain standard",
        "110%",
        "Elite Onboarding",
        "diversity quota",
        "data-driven insight"
    };
    public List<Phrase> activePhrases = new List<Phrase>();
    public List<string> activeStrings = new List<string>();

    //template for text objects
    public TMP_Text Textplate;

    //speed/progression vars
    public float textSpeed;
    public float wordSpeed;

    //short snippets to be typed on computer screen
    public class Phrase {
        public TMP_Text baseText;
        public TMP_Text topText;
        public string words;
        public bool completed = false;

        public Phrase(string w, List<Phrase> l, TMP_Text t)
        {
            
            float xbound = (GameObject.Find("computerScreen").GetComponent<RectTransform>().rect.width / 2) - (t.rectTransform.rect.width / 2);
            float ybound = (GameObject.Find("computerScreen").GetComponent<RectTransform>().rect.height / 2) - (t.rectTransform.rect.height / 2);

            Vector3 pos = new Vector3(Random.Range(-xbound, xbound), Random.Range(-ybound, ybound));

            this.words = w;
            this.baseText = Instantiate<TMP_Text>(t, GameObject.Find("computerScreen").transform);
            this.baseText.rectTransform.localPosition = pos;
            this.topText = Instantiate<TMP_Text>(t, GameObject.Find("computerScreen").transform);
            this.topText.rectTransform.localPosition = pos;

        }
    }

    IEnumerator bye(Phrase p)
    {
        byte alp = 255;

        GameObject.Destroy(p.baseText.gameObject);

        while (p.topText.color.a > 0f)
        {
            alp = (byte) ((float) alp - 51);
            
            p.topText.color = new Color32(255, 255, 255, alp);
            yield return new WaitForSeconds(.2f);
        }
        
        GameObject.Destroy(p.topText.gameObject);
    }

    void OnEnable()
    {
        textSpeed = .3f;
        wordSpeed = 5f;
        resetBoard(textSpeed);
        beginPhrase(currentPhrase);
    }

    public void resetBoard(float speed)
    {
        //clear computer screen
        GameObject screen = GameObject.Find("computerScreen");
        foreach (Transform text in screen.transform)
        {
            GameObject.Destroy(text.gameObject);
        }

        charIndex = 0;
        wordIndex = 0;
        textSpeed = speed;
        activePhrases = new List<Phrase>();
        currentPhrase = new Phrase(phrases[Random.Range(0, phrases.Count)], activePhrases, Textplate);
        nextLetter = currentPhrase.words[0];
    }

    void beginPhrase(Phrase p)
    {
        activePhrases.Add(p);
        activeStrings.Add(p.words);
        TextWriter.AddWriter_Static(p.baseText, p.words, textSpeed);
    }

    void FixedUpdate()
    {
        if (Time.fixedTime % wordSpeed == 0)
        {
            int index = Random.Range(0, phrases.Count);
            int counter = 0;
            
            while (activeStrings.Contains(phrases[index]))
            {
                counter++;
                index = Random.Range(0, phrases.Count);
                if (counter > 20)
                {
                    Debug.Log("too many");
                    break;
                }
            }

            beginPhrase(new Phrase(phrases[index], activePhrases, Textplate));
        
            
        }
    }
    void Update()
    {
        
        if (!backRequired && Input.inputString != "")
        {
            foreach (char c in Input.inputString)
            {
                if (c == nextLetter && charIndex == currentPhrase.words.Length - 1 && wordIndex + 1 >= activePhrases.Count)
                {
                    textSpeed -= .02f;
                    wordSpeed -= .5f;
                    resetBoard(textSpeed);
                    beginPhrase(currentPhrase);
                }
                else if (c == nextLetter && charIndex == currentPhrase.words.Length - 1)
                {
                    currentPhrase.topText.text = "<color=green>" + currentPhrase.words + " </color>";
                    currentPhrase.completed = true;
                    StartCoroutine(bye(currentPhrase));
                    charIndex = 0;
                    wordIndex++;
                    currentPhrase = activePhrases[wordIndex];
                    nextLetter = currentPhrase.words[charIndex];
                    
                    
                }
                else if (c == nextLetter)
                {
                    charIndex++;
                    nextLetter = currentPhrase.words[charIndex];
                    currentPhrase.topText.text = "<color=green>" + currentPhrase.words.Substring(0, charIndex) + " </color>" + "<alpha=#00>" + currentPhrase.words.Substring(charIndex) + "</alpha>";
                }
                else
                {
                    currentPhrase.topText.text = "<color=green>" + currentPhrase.words.Substring(0, charIndex) + "</color><color=red><mark=#ff000080>" + currentPhrase.words.Substring(charIndex, 1)
                    + "</mark> </color><alpha=#00>" + currentPhrase.words.Substring(charIndex) + "</alpha>";

                    backRequired = true;
                }
            }
        }
        else if (backRequired && Input.GetKeyDown(KeyCode.Backspace))
        {
            backRequired = false;
            currentPhrase.topText.text = "<color=green>" + currentPhrase.words.Substring(0, charIndex) + " </color>" + "<alpha=#00>" + currentPhrase.words.Substring(charIndex) + "</alpha>";
        }
        
        if(Input.GetKeyDown(KeyCode.Tab))
        {
             int index = Random.Range(0, phrases.Count);
            int counter = 0;
            
            while (activeStrings.Contains(phrases[index]))
            {
                counter++;
                index = Random.Range(0, phrases.Count);
                if (counter > 20)
                {
                    Debug.Log("too many");
                    break;
                }
            }

            beginPhrase(new Phrase(phrases[index], activePhrases, Textplate));
        }
        if(Input.GetKeyDown(KeyCode.PageUp))
        {
            for (int i = 0; i < phrases.Count; i++)
            {
                Debug.Log(phrases[i] + " " + i);
            }
        }
        if(Input.GetKeyDown(KeyCode.PageDown))
        {
            for (int i = 0; i < activePhrases.Count; i++)
            {
                Debug.Log(activePhrases[i] + " " + i);
            }
        }
    }
}
