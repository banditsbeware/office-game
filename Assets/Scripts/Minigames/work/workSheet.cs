using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class workSheet : workWindow
{
    private List<string> phrases = new List<string>()
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
    
    public List<sheetsCell> cells;
    public List<sheetsCell> cellsWithoutPhrases;
    public sheetsCell activeCell;

    private int phraseCounter = 300;
    private (int, int) phraseTimingBoundaries = (50, 150);
    [SerializeField] private int delayPerPhrase = 20;

    [SerializeField] private GameObject cellTemplate;

    //text objects
    public TMP_Text sheetTitle;
    public TMP_Text textplate;
    
    public override void Start()
    {   
        cells = new List<sheetsCell>();

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
        base.FixedUpdate();

        if (phraseCounter > 0)
        {
            phraseCounter--;
            return;
        }

        AddRandPhraseToRandCell();
        phraseCounter = Random.Range(phraseTimingBoundaries.Item1, phraseTimingBoundaries.Item2) + delayPerPhrase * cellsWithoutPhrases.Count;
    }

    private void AddRandPhraseToRandCell()
    {
        Debug.Log("e");

        int phraseIndex = Random.Range(0, phrases.Count - 1);
        int cellIndex = Random.Range(0, cellsWithoutPhrases.Count - 1);

        sheetsCell c = cellsWithoutPhrases[cellIndex];

        c.cellPhrase = new Phrase(phrases[phraseIndex], textplate, c.transform, this);
    }

    public override void Complete()
    {
        work.windows.Remove(this);
        StartCoroutine(FadeDestroy(timeAfterCompletion));
    }

    public void CellSelected(sheetsCell cell)
    {
        foreach(sheetsCell c in cells)
        {
            c.Deselect();
        }

        activeCell = cell;
        activePhrase = cell.cellPhrase;
        activeCell.SelectCell();
    }
    
    public override void InputRecieved(string input)
    {
        activePhrase.CheckInput(input, this);
    }

    public override void Back()
    {
        activePhrase.Backspace();
    }

    public override void Enter()
    {
        activePhrase.CheckInput("\n");
    }

    public override void PhraseComplete()
    {

    }
}

