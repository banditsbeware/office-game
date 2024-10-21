using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class workSheet : workWindow
{
    #region
    private List<List<string>> phraseGroups;
    private List<string> businessPhrases = new List<string>()
    {
        "Return On Investment (ROI)",
        "Synergy",
        "Core Competency",
        "unmatched logistics",
        "holistic customer experience",
        "work ecosystem",
        "restructuring",
        "AI revolution",
        "target consumer",
        "colloquial pricing",
        "cloud-based computing",
        "let's touch base",
        "shift the paradigm",
        "growth hacking",
        "machine learning algorithm",
        "blockchain standard",
        "110%",
        "Elite Onboarding",
        "diversity quota",
        "data-driven insight",
        "mass market consumerism",
        "operational integrity",
        "vertical integration",
        "create scarcity",
        "please the stakeholders",
        "balance sheet",
        "public relations nightmare",
        "team players only!",
        "pivot",
        "brand identity",
        "#1 in innovation",
        "Mobilize and Optimize",
        "Real-time engagement",
        "cash flow report",
        "quarterly earnings",
        "investment analysis",
        "trend forecasting",
        "risk aversion",
    };

    private List<string> accountingPhrases = new List<string>()
    {
        "10.002",
        "pi * omega^2",
        "the integral of 4",
        "68.99",
        ".04% return",
        "12% APR",
        "tax rebate"
    };
    #endregion
    private List<string> phrases = new List<string>();


    
    public List<sheetsCell> cells;
    public List<sheetsCell> cellsWithoutPhrases;
    public List<sheetsCell> openCells;
    public sheetsCell activeCell;

    private int phraseCounter = 200;
    private int phraseTimingMinimum = 500; //200 was default before debugging
    [SerializeField] private int delayPerPhrase = 80;

    [SerializeField] private GameObject cellTemplate;

    private bool sheetComplete = false;

    //text objects
    public TMP_Text sheetTitle;
    public TMP_Text textplate;
    
    public override void Start()
    {   
        cells = new List<sheetsCell>();
        openCells = new List<sheetsCell>();

        phraseGroups = new List<List<string>>()
        {
            businessPhrases,
            accountingPhrases
        };

        phrases.AddRange(phraseGroups[Random.Range(0, phraseGroups.Count)]);

        Transform contentObject = transform.Find("contents");
        for (int i = 0; i < 30; i++)
        {
            GameObject tempCellObject = Instantiate(cellTemplate, contentObject);
            cells.Add(tempCellObject.GetComponent<sheetsCell>());
        }

        cellsWithoutPhrases = new List<sheetsCell>();
        cellsWithoutPhrases.AddRange(cells);

        AddRandPhraseToRandCell();
        AddRandPhraseToRandCell();
        AddRandPhraseToRandCell();

        base.Start();
    }

    protected override void FixedUpdate() 
    {
        if(work.activeWindow == this) 
        {
            base.FixedUpdate();
    
            if (phraseCounter > 0)
            {
                phraseCounter--;
                return;
            }

            AddRandPhraseToRandCell();
            phraseCounter = phraseTimingMinimum + delayPerPhrase * openCells.Count;
        }
    }

    private void AddRandPhraseToRandCell()
    {
        int phraseIndex = Random.Range(0, phrases.Count - 1);
        int cellIndex = Random.Range(0, cellsWithoutPhrases.Count - 1);

        sheetsCell c = cellsWithoutPhrases[cellIndex];

        c.cellPhrase = new Phrase(phrases[phraseIndex], textplate, c.transform, this);

        openCells.Add(c);
        cellsWithoutPhrases.RemoveAt(cellIndex);
    }

    public override void Complete()
    {
        work.windows.Remove(this);
        StartCoroutine(FadeDestroy(timeAfterCompletion));
        TaskManager.TaskComplete(TaskType.Spreadsheet);
    }
    
    public void CellSelected(sheetsCell cell)
    {
        foreach(sheetsCell c in cells)
        {
            if(!c.complete) 
            {
                c.Deselect();
            }
        }

        activeCell = cell;
        activePhrase = cell.cellPhrase;
        activeCell.SelectCell();
    }
    
    public override void InputRecieved(string input)
    {
        if(activeCell != null && activeCell.cellPhrase != null) 
        {
            activePhrase.CheckInput(input, this);
        }
    }

    public override void Back()
    {
        if(activeCell != null && activeCell.cellPhrase != null) 
        {
            activePhrase.Backspace();
        }
    }

    public override void Enter()
    {
        if(activeCell != null && activeCell.cellPhrase != null) 
        {
            activePhrase.CheckInput("\n");
        }
    }

    public override void PhraseComplete()
    {
        Debug.Log("Phrase Complete");
        activeCell.cellComplete();
        openCells.Remove(activeCell);
        activeCell = null;
        if(openCells.Count == 0)
        {
            Debug.Log("openCells 0");
            work.activeWindow = null;
            Complete();
        }
    }
}

