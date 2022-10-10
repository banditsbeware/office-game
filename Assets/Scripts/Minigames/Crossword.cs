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

  //objects for instantiating buttons
  public GameObject exampleButton;
  private GameObject button;
  private static int fontConst = 600;

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
  private static string[] dirList = new string[]
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
  public TMP_Text haroldSpeech;
  public TMP_Text playerSpeech;
  public List<Word> wordsInPlay = new List<Word>();
  public TMP_Text postIt;

  IEnumerator ChangeBoards(Word he, string stn)
  {
    yield return new WaitForSeconds(4f);
    TextWriter.AddWriter_Static(haroldSpeech, stn);
    wordsInPlay = he.normalOptions;
    yield return new WaitForSeconds(5f);
    DestroyGrid();
    CreateGrid();
    yield return new WaitForSeconds(.5f);
    DestroyGrid();
    CreateGrid();
    yield return new WaitForSeconds(.5f);
    DestroyGrid();
    CreateGrid();
    PlaceWords();
  }

  void OnEnable()
  {
    wordsInPlay = new List<Word>();
    CreateWords();
    DestroyGrid();
    CreateGrid();
    PlaceWords();
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.P))
    	{
          firstSel = wordsInPlay[0].endButton;
          secondSel = wordsInPlay[0].startButton;
        	CheckWord();
   		}
  }

  void PlaceWords()
  {
    for (int i = 0; i < wordsInPlay.Count; i++)
    {
      PlaceWord(wordsInPlay[i]);
    }
  }

  void DestroyGrid()
  {
    foreach (Transform butt in gridObject.transform)
    {
      GameObject.Destroy(butt.gameObject);
    }
    
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
        ButtonTextComponent(button).fontSize = fontConst / size;

        //add to independent matrix
        letterMatrix[i, j] = button;
      }
    }

    postIt.text = "acceptable topics\nof conversation:\n";
    foreach (Word word in wordsInPlay) {
      postIt.text += "-" + word.postItDes + "\n";
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
    foreach (Word he in wordsInPlay)
    {
      if ((firstSel == he.endButton || firstSel == he.startButton) && (secondSel == he.endButton || secondSel == he.startButton))
      {
        firstSel.GetComponent<Button>().enabled = false;
        secondSel.GetComponent<Button>().enabled = false;
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
    TextWriter.AddWriter_Static(playerSpeech, he.playerTalk_normal);
    StartCoroutine(ChangeBoards(he, he.haroldTalk_normal));
  }


  public class Word
  {
    public string word;
    public string postItDes;
    public List<Word> normalOptions;
    public List<Word> oddOptions;
    public List<Word> whackOptions;
    public string playerTalk_normal;
    public string playerTalk_odd;
    public string playerTalk_whack;
    public string haroldTalk_normal;
    public string haroldTalk_odd;
    public string haroldTalk_wack;
    public (int, int) direction;
    public (int, int) origin;
    [System.NonSerialized] public GameObject startButton;
    [System.NonSerialized] public GameObject endButton;
  }


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


  public void CreateWords()
  {
    //define every word and its responses
    Word weather = new Word {
      word = "WEATHER",
      postItDes = "the climate",
      playerTalk_normal = "So, how about that weather, huh?",
      haroldTalk_normal = "Yeah, quite the weather"};

      Word snow = new Word {
      word = "SNOW",
      postItDes = "precipitation",
      playerTalk_normal = "I really wish it snowed more, I quite like snow",
      haroldTalk_normal = "I'm more of a sunshine guy myself"};

      Word clothes = new Word {
      word = "CLOTHES",
      postItDes = "garments",
      playerTalk_normal = "have enough clothes for the upcoming season?",
      haroldTalk_normal = "I was going to head shopping with my kids just next week"};
    
    Word child = new Word {
      word = "CHILD",
      postItDes = "Harold's offspring",
      playerTalk_normal = "How are the kids doing these days?",
      haroldTalk_normal = "Little Jimmy just started playing soccer, and he's super excited to start middle school"};

    
    


    


    //add options to each word
    weather.normalOptions = new List<Word>(){snow};
    //oddOptions = (warming, storm, venus),
    //whackOptions = (death, corp, coal),

    snow.normalOptions = new List<Word>(){clothes};
    clothes.normalOptions = new List<Word>(){child};
    child.normalOptions = new List<Word>(){};
      



    wordsInPlay.Add(weather);
    wordsInPlay.Add(child);
  }

  
    
  }
  
