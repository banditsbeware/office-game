using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Crossword : MonoBehaviour {
  private static char[] alpha = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' }; 
  
  //grid variables
  public int size;
  public GameObject gridObject;
  private GridLayoutGroup gridComponent;
  private RectTransform gridTrans;

  //matrix of child objects
  public GameObject[,] letterMatrix;

  // objects for instantiating buttons
  public GameObject exampleButton;
  private GameObject button;

  private static Dictionary<string, (int, int)> dirDict = new Dictionary<string, (int, int)>()
  {
    {"N", (0, -1)},
    {"S", (0, 1)},
    {"E", (-1, 0)},
    {"W", (1, 0)},
    {"NW", (1, -1)},
    {"SW", (1, 1)},
    {"NE", (-1, -1)},
    {"SE", (-1, 1)}
  };
  private string[] dirList = new string[]
  {
    "N",
    "S",
    "E",
    "W",
    "NW",
    "NW",
    "SW",
    "SW",
    "NE",
    "NE",
    "SE",
    "SE"
  };

  //selection variables 
  private bool wordCompleted = true;
  private GameObject firstSel;
  private GameObject secondSel;

  //words 
  public GameObject response;
  public Word[] wordList = new Word[5];

  void Start()
  {
    CreateGrid();
    for (int x = 0; x < wordList.Length; x++)
    {
      PlaceWord(wordList[x]);
    }

    //debugging stuff
    // PlaceWord("XXXXXXXXXX", "W", (0, 5));
    // PlaceWord("COOMER")
  }

  void CreateGrid() 
  {
    //create matrix for manipulating letters
    letterMatrix = new GameObject[size, size];

    //get grid components
    gridTrans = gridObject.GetComponent<RectTransform>();

    //set buttons to fill in grid entirely
    gridObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(gridTrans.sizeDelta.x / size, gridTrans.sizeDelta.y / size);

    for (int j = 0; j < size; j++)
    {
      for (int i = 0; i < size; i++)
      {
        //select random char from alphabet
        char letter = alpha[Random.Range(0, 25)];

        //create copy of button prefab
        button = Instantiate(exampleButton, gridObject.transform);

        //change text of the button to random char
        ButtonTextComponent(button).text = char.ToString(letter);

        //add to independent matrix
        letterMatrix[i, j] = button;
      }
    }
  }

  void PlaceWord(Word it, string facing, (int, int) origin)
  {
    it.direction = dirDict[facing];
    ChangeLetters(it);
  }
  void PlaceWord(Word it)
  {
    string word = it.word;
    it.direction = dirDict[dirList[Random.Range(0, dirList.Length)]];
    it.origin = (Random.Range(0, size), Random.Range(0, size));
    bool overlap = true;
    int counter = 0;
    
    //check bounds of grid, and whether any letters overlap existing words
    while (overlap == true)
    {
      while ((it.origin.Item1 + it.direction.Item1 * word.Length < 0) || (it.origin.Item1 + it.direction.Item1 * word.Length > size) ||
            (it.origin.Item2 + it.direction.Item2 * word.Length < 0) || (it.origin.Item2 + it.direction.Item2 * word.Length > size))
      {
        it.direction = dirDict[dirList[Random.Range(0, 7)]];
        it.origin = (Random.Range(0, size), Random.Range(0, size));
        Debug.Log("Attempt to be in square!");

        //break for over 200 attempts
        counter ++;
        if (counter >= 200)
        {
          Debug.Log("Too many tries :(");
          goto skip;
        }
      }

      //check overlap
      overlap = false;
      for (int x = 0; x < word.Length; x++)
      {
        WordsearchButton heIs = letterMatrix[it.origin.Item1 + it.direction.Item1 * x, it.origin.Item2 + it.direction.Item2 * x].GetComponent<WordsearchButton>(); //gets button script to use variable
    
        if (heIs.usedByOtherWord) 
        {
          overlap = true;
        }
      }
      if (overlap == true)
      {
        it.origin = (Random.Range(0, size), Random.Range(0, size));
        // direction = dirDict[dirList[Random.Range(0, 7)]];
      }

      Debug.Log("Check for overlap!");
    }
    skip:
    ChangeLetters(it);
  }

  //iterate through each button object in a direction from an origin, change its text to word letter
  void ChangeLetters(Word it)
  {
    string word = it.word;
    for (int x = 0; x < word.Length; x++)
    {
      button = letterMatrix[it.origin.Item1 + it.direction.Item1 * x, it.origin.Item2 + it.direction.Item2 * x];
      ButtonTextComponent(button).text = word[x].ToString();
      button.GetComponent<WordsearchButton>().usedByOtherWord = true;
      
      //assign first and last button to Word object
      if (x == 0)
      {
        it.startButton = button;
      }
      else if (x == word.Length - 1)
      {
        it.endButton = button;
      }

      // Debug.Log(origin.ToString() + direction.ToString());
    }
  }

  TMP_Text ButtonTextComponent(GameObject button)
  {
    return button.transform.GetChild(0).gameObject.GetComponent<TextMeshProUGUI>();
  }

//sent by Wordsearch button script when button is pressed
  public void Selection(GameObject button)
  {
    if (wordCompleted)
    {
      wordCompleted = false;
      firstSel = button;
    }
    else
    {
      wordCompleted = true;
      secondSel = button;
      CheckWord();
    }
  }

  public void CheckWord()
  {
    bool aWordWasFound = false;

    //compare first and last button of selection and Word's button objects assigned in ChangeLetters()
    foreach (Word he in wordList)
    {
      if ((firstSel == he.endButton || firstSel == he.startButton) && (secondSel == he.endButton || secondSel == he.startButton))
      {
        firstSel.GetComponent<Button>().enabled = false;
        secondSel.GetComponent<Button>().enabled = false;

        he.found = true;
        aWordWasFound = true;
        Respond(he);

        //take out buttons in middle of word
        for (int x = 1; x < he.word.Length - 1; x++)
        {
          button = letterMatrix[he.origin.Item1 + he.direction.Item1 * x, he.origin.Item2 + he.direction.Item2 * x];
          button.GetComponent<Image>().color = new Color32(200, 100, 100, 200);
          button.GetComponent<Button>().enabled = false;
            
        }
        break;
      }
    }

    if (!aWordWasFound)
    {
      firstSel.GetComponent<WordsearchButton>().Reset();
      secondSel.GetComponent<WordsearchButton>().Reset();
    }
  }

  //set text in minigame canvas to response in Word object
  public void Respond(Word he)
  {
    response.GetComponent<TextMeshProUGUI>().text = he.responses[0];
  }


  [System.Serializable] public class Word
  {
    public string word;
    public string[] responses;
    public bool found = false;
    public (int, int) direction;
    public (int, int) origin;
    [System.NonSerialized] public GameObject startButton;
    [System.NonSerialized] public GameObject endButton;
  }
}

