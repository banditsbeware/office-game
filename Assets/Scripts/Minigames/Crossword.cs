using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Crossword : MonoBehaviour {
  private static char[] alpha = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' }; 

  public UIManager UIMan;

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
  public List<Word> avalibleWords = new List<Word>();
  public TMP_Text postIt;

  IEnumerator ChangeBoards(Word he)
  {
    yield return new WaitForSeconds(he.playerTime);
    TextWriter.AddWriter_Static(haroldSpeech, he.haroldTalk_normal);
    avalibleWords = he.normalOptions;
    yield return new WaitForSeconds(he.haroldTime);
    DoGrid();
    yield return new WaitForSeconds(.5f);
    DoGrid();
    yield return new WaitForSeconds(.5f);
    DoGrid();
    PlaceWords();
  }
  IEnumerator Goodbye(Word he)
  {
    yield return new WaitForSeconds(he.playerTime);
    TextWriter.AddWriter_Static(haroldSpeech, he.haroldTalk_normal);
    yield return new WaitForSeconds(he.haroldTime);
    UIMan.gameState = "play";
    gameObject.SetActive(false);
  }

  void OnEnable()
  {
    haroldSpeech.text = "";
    playerSpeech.text = "";
    avalibleWords = new List<Word>();
    CreateWords();
    DoGrid();
    PlaceWords();
    Debug.Log("words:" + avalibleWords);
  }

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.P))
    	{
          firstSel = avalibleWords[0].endButton;
          secondSel = avalibleWords[0].startButton;
        	CheckWord();
   		}
  }

  void PlaceWords()
  {
    List<int> wordsInPlay = new List<int>();

    //apply postIt header
    postIt.text = "acceptable topics\nof conversation:\n";

    //display no more than 3 words from list of avalible words
    if (avalibleWords.Count > 2)
    {
      while (wordsInPlay.Count < 3)
      {
        int index = Random.Range(0, avalibleWords.Count);
        
        if (!wordsInPlay.Contains(index))
        {
          Debug.Log(index);
          PlaceWord(avalibleWords[index]);
          wordsInPlay.Add(index);
        }
      }
    } 
    else
    {
      foreach (Word word in avalibleWords)
      {
        PlaceWord(word);
      }
    }
  }

  void DoGrid() 
  {
    //destroy prior grid
    foreach (Transform butt in gridObject.transform)
    {
      GameObject.Destroy(butt.gameObject);
    }

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
  }

  void PlaceWord(Word it, string facing, (int, int) origin)
  {
    it.direction = dirDict[facing];
    ChangeLetters(it);

    //add to list on postit
    postIt.text += "-" + it.postItDes + "\n";
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
    }
    skip:
    ChangeLetters(it);

    postIt.text += "-" + it.postItDes + "\n";
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
    foreach (Word he in avalibleWords)
    {
      if ((firstSel == he.endButton || firstSel == he.startButton) && (secondSel == he.endButton || secondSel == he.startButton))
      {
        firstSel.GetComponent<Button>().enabled = false;
        secondSel.GetComponent<Button>().enabled = false;
        aWordWasFound = true;

        //set text in minigame canvas to response in Word object
        if (he.word == "GOODBYE")
        {
          TextWriter.AddWriter_Static(playerSpeech, he.playerTalk_normal);
          StartCoroutine(Goodbye(he));
        }
        else
        {
          TextWriter.AddWriter_Static(playerSpeech, he.playerTalk_normal);
          StartCoroutine(ChangeBoards(he));
        }

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
    public float playerTime;
    public string haroldTalk_normal;
    public string haroldTalk_odd;
    public string haroldTalk_wack;
    public float haroldTime;
    public (int, int) direction;
    public (int, int) origin;
    [System.NonSerialized] public GameObject startButton;
    [System.NonSerialized] public GameObject endButton;
  }


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


  public void CreateWords()
  {
    //define every word and its responses
    Word goodbye = new Word {
      word = "GOODBYE",
      postItDes = "end the exchange",
      playerTalk_normal = "alright, bud, I should get back to work",
      haroldTalk_normal = "see ya round",
      playerTime = 4f,
      haroldTime = 3f};

    Word weather = new Word {
      word = "WEATHER",
      postItDes = "the climate",
      playerTalk_normal = "So, how about that weather, huh?",
      haroldTalk_normal = "Yeah, quite the weather",
      playerTime = 4f,
      haroldTime = 3f};

      Word snow = new Word {
      word = "SNOW",
      postItDes = "precipitation",
      playerTalk_normal = "I really wish it snowed more, I quite like snow",
      haroldTalk_normal = "I'm more of a sunshine guy myself",
      playerTime = 5f,
      haroldTime = 4f};

      Word clothes = new Word {
      word = "CLOTHES",
      postItDes = "garments",
      playerTalk_normal = "have enough clothes for the upcoming season?",
      haroldTalk_normal = "I was going to head shopping with my kids just next week",
      playerTime = 5f,
      haroldTime = 6f};
    
    Word child = new Word {
      word = "CHILD",
      postItDes = "harold's offspring",
      playerTalk_normal = "How are the kids doing these days?",
      haroldTalk_normal = "Little Jimmy's playing soccer, he's quite the young man",
      playerTime = 4f,
      haroldTime = 5f};

      Word school = new Word {
        word = "SCHOOL",
        postItDes = "education",
        playerTalk_normal = "nice, how is the schooling coming along?",
        haroldTalk_normal = "very well. Bs across the board",
        playerTime = 4f,
        haroldTime = 3f};
    
    Word work = new Word {
          word = "WORK",
          postItDes = "the job",
          playerTalk_normal = "work's really been grinding my gears lately",
          haroldTalk_normal = "tell me about it",
          playerTime = 4f,
          haroldTime = 2f};

    //add options to each word
    weather.normalOptions = new List<Word>(){snow, goodbye};
    snow.normalOptions = new List<Word>(){clothes, goodbye};
    clothes.normalOptions = new List<Word>(){child, goodbye};
    child.normalOptions = new List<Word>(){school, goodbye};
    school.normalOptions = new List<Word>(){work, goodbye};
    work.normalOptions = new List<Word>(){goodbye};
      
    avalibleWords.Add(weather);
    avalibleWords.Add(child);
    avalibleWords.Add(work);
  }

  
    
  }
  
