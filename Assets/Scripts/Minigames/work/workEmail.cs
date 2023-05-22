using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class workEmail : workWindow
{
    private List<List<string>> emailChains = new List<List<string>>()    //List is Thread title, incoming1, outgoing1, incoming2, outgoing2, etc.
    {
        new List<string>(){"Onboarding event --  bcc. Everyone", "To whom it may concern\n\nEat ass and die\n\nCheers,\nAccounting", "Test\n\nTest", "Test Test my ass\n\n-Acct.", "Literally go die"}
    };
    private bool emailComplete = false;
    private List<string> chain;
    private int threadNumber = 3;  //after instantiating title, incoming1, outgoing1, list index is 3
    private float timeAfterSend = 2f;
    private float timeAfterRecieve = 3f;


    [SerializeField] private GameObject incomingTemplate;
    [SerializeField] private GameObject outgoingTemplate;


    [SerializeField] private GameObject contents;
    [SerializeField] private GameObject currentIncoming;
    [SerializeField] private GameObject currentOutgoing;
    private GameObject sendButton;

    //text objects
    public TMP_Text textplate;
    public TMP_Text chainTitle;
    private TMP_Text incomingText;
    
    public override void Start()
    {
        StartCoroutine(StartCor());
    }

    public IEnumerator StartCor()
    {
        SetButtonObject();

        base.Start();

        chain = emailChains[Random.Range(0, emailChains.Count)];

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
        StartCoroutine(FadeDestroy());
    }
    
    public override void InputRecieved(string input)
    {
        if(!emailComplete) 
        {
            activePhrase.CheckInput(input, this);
        }
    }

    public override void Back()
    {
        if(!emailComplete) 
        {
            activePhrase.Backspace();
        }
    }

    public override void Enter()
    {
        if(!emailComplete) 
        {
            activePhrase.CheckInput("\n");
        }
    }

    public override void PhraseComplete()
    {
        emailComplete = true;
        sendButton.SetActive(true);
    }

    public IEnumerator SendEmail()
    {
        //send action
        sendButton.SetActive(false);
        activePhrase.Solifify();

        yield return new WaitForSeconds(timeAfterSend);

        //update incoming email
        currentIncoming = Instantiate(incomingTemplate, contents.transform);

        SetIncomingText(chain[threadNumber]);
        threadNumber++;

        UpdateColoredObjects();

        yield return new WaitForSeconds(timeAfterRecieve);

        //update next outgoing
        emailComplete = false;

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

