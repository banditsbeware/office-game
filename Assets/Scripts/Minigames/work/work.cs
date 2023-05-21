using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;


public class work : MonoBehaviour
{
    //references
    private GameObject screenSpace;
    public Canvas canvas;
    [SerializeField] private GameObject referenceWindow;

    //windows
    public List<workWindow> windows;
    public workWindow activeWindow;


    private void OnEnable() 
    {
        canvas = gameObject.GetComponent<Canvas>();
        screenSpace = GameObject.Find("computerScreen");
        CreateWindow();
    }

    private void OnDisable() {
        foreach (workWindow window in windows)
        {
            window.Destroy();
        }
        windows.Clear();
    }

    void Update()
    {
        if (!activeWindow.backRequired && Input.inputString != "")
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
    }

    private void CreateWindow()
    {
        workWindow window = Instantiate(referenceWindow, screenSpace.transform).GetComponent<workWindow>();
        windows.Add(window);
    }
}
