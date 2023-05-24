using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class workSheet : workWindow
{
    private List<string> cellPossibilities = new List<string>()
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
    
    public sheetsCell activeCell;

    [SerializeField] private GameObject cellTemplate;

    //text objects
    public TMP_Text textplate;
    public TMP_Text sheetTitle;
    
    public override void Start()
    {
        StartCoroutine(StartCor());
    }

    public IEnumerator StartCor()
    {
        SetButtonObject();

        base.Start();

        chain = cellPossibilities[Random.Range(0, cellPossibilities.Count)];

        chainTitle.text = chain[0];
        SetIncomingText(chain[1]);
        activePhrase = new Phrase(chain[2], textplate, currentOutgoing.transform, this);

        currentOutgoing.SetActive(false);

        yield return new WaitForSeconds(timeAfterRecieve);

        currentOutgoing.SetActive(true);
        
    }

    public override void Complete()
    {
        work.windows.Remove(this);
        StartCoroutine(FadeDestroy(timeAfterCompletion));
    }
    
    public override void InputRecieved(string input)
    {
        if(!messageComplete) 
        {
            activePhrase.CheckInput(input, this);
        }
    }

    public override void Back()
    {
        if(!messageComplete) 
        {
            activePhrase.Backspace();
        }
    }

    public override void Enter()
    {
        if(!messageComplete) 
        {
            activePhrase.CheckInput("\n");
            return;
        }

        if (sendReady)
        {
            StartCoroutine(SendEmail());
            sendReady = false;
        }
    }

    public override void PhraseComplete()
    {
        messageComplete = true;
        sendReady = true;
        sendButton.SetActive(true);
    }

    public IEnumerator SendEmail()
    {
        //send action
        sendButton.SetActive(false);
        activePhrase.Solifify();

        if (threadNumber == chain.Count)
        {
            Complete();
            yield break;
        }

        yield return new WaitForSeconds(timeAfterSend);

        //update incoming email
        currentIncoming = Instantiate(incomingTemplate, contents.transform);

        SetIncomingText(chain[threadNumber]);
        threadNumber++;

        UpdateColoredObjects();
        
        if (threadNumber == chain.Count)
        {
            Complete();
            yield break;
        }
        

        yield return new WaitForSeconds(timeAfterRecieve);

        //update next outgoing
        messageComplete = false;
        currentOutgoing = Instantiate(outgoingTemplate, contents.transform);
        activePhrase = new Phrase(chain[threadNumber], textplate, currentOutgoing.transform, this);
        threadNumber++;

        SetButtonObject();

        UpdateColoredObjects();
    }

    private void SetIncomingText(string txt)
    {
        currentIncoming.transform.Find("emailText").GetComponent<TMP_Text>().text = txt;
    }

    private void SetButtonObject()
    {
        sendButton = currentOutgoing.transform.Find("sendButton").gameObject;
        sendButton.SetActive(false);
    }
}

