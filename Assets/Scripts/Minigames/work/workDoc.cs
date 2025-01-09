using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class workDoc : workWindow
{
    private List<(string, string)> possibleDocs = new List<(string, string)>()  // (Doc Title, Contents)
    {
        ("Documents  --  Workplace Ecosystem Draft", "To our beloved shareholders at [insert company name here],\n\nWe are quite thrilled to present our newest innovation in company workplace ecosystem technology. Our teams have been working tirelessly to bring to you only the best, most quality-focused end product on the market today. Among our team's diverse creative potential, we explored collaborative pathways to achieve maximum efficiency in our product workspace, and the desired effect has not gone unnoticed.\n\nThank you for your time,\nWorkplace Innovation Team"),
        ("Documents  --  Manegement Review Memo", "Dear workplace peers,\n\nPlanning is underway for our regular review of management practices, and we encourage you to acclimate to the climate required of such a review. Please be mindful of this project as you continue your talented endeavors.\n\nManagement"),
        ("Documents  --  Ethic Memo", "Lovely Coworkers,\n\nIt has come to our attention that the workplace environment we are working hard to cultivate is hitting some hurdles. I just want to remind everyone that we are a family, and that the decisions we make really do impact each other, and the world. Much love,\n\nFamily Management"),
        ("Documents  --  Memo on Best Practices", "Hey spreadsheet users!\n\nI'd like to schedule a review of best practices when handling sensitive information in shared spreadsheets, please expect an email to coordinate soon\n\n--The Spreadsheet Guru")
        };

    private bool docComplete = false;
    private GameObject printButton;

    //template for text objects
    public TMP_Text textplate;
    public TMP_Text titleText;

    public override void Start()
    {
        printButton = transform.Find("sendButton").gameObject;
        printButton.SetActive(false);

        base.Start();

        (string, string) randomDoc = possibleDocs[Random.Range(0, possibleDocs.Count)];

        titleText.text = randomDoc.Item1;
        activePhrase = new Phrase(randomDoc.Item2, textplate, transform, this);
        activePhrase.nextLetter = activePhrase.words[0];
    }

    public override void Complete()
    {
        //send print job to printer

        work.windows.Remove(this);
        StartCoroutine(FadeDestroy());
    }

    
    public override void InputRecieved(string input)
    {
        if(!docComplete) 
        {
            activePhrase.CheckInput(input);
        }
    }

    public override void Back()
    {
        if(!docComplete) 
        {
            activePhrase.Backspace();
        }
    }

    public override void Enter()
    {
        if(!docComplete) 
        {
            activePhrase.CheckInput("\n");
        }
    }

    public override void PhraseComplete()
    {
        docComplete = true;
        printButton.SetActive(true);
    }
}

