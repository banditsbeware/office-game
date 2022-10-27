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
  public List<Word> avalibleWords = new List<Word>();
  public List<Word> allWords = new List<Word>();
  public TMP_Text postIt;
  //goodbye only global word to be added post-randomization
  private Word goodbye;
  private Word hello;

  //WWise
  public AK.Wwise.Bank crossBank;
  public AK.Wwise.RTPC goodbyeRTPC;
  public AK.Wwise.RTPC helloRTPC;

  IEnumerator ChangeBoards(Word he)
  {
    TextWriter.AddWriter_Static(playerSpeech, he.playerTalk_normal);
    AkSoundEngine.PostEvent("Play_Player", gameObject);
    yield return new WaitForSeconds(he.playerTime);

    AkSoundEngine.PostEvent("Stop_Player", gameObject);
    yield return new WaitForSeconds(1.5f);
  
    AkSoundEngine.PostEvent(he.post, gameObject);
    TextWriter.AddWriter_Static(haroldSpeech, he.haroldTalk_normal);
    yield return new WaitForSeconds(he.haroldTime);

    //add word options if not already used in coversation
    avalibleWords = new List<Word>();
    foreach (Word word in he.normalOptions)
    {
      if (!word.used)
      {
        avalibleWords.Add(word);
      }
    }
      
    DoGrid();
    yield return new WaitForSeconds(.5f);
    DoGrid();
    yield return new WaitForSeconds(.5f);
    DoGrid();
    PlaceWords();
  }
  IEnumerator Goodbye(Word he)
  {
    TextWriter.AddWriter_Static(playerSpeech, he.playerTalk_normal);
    AkSoundEngine.PostEvent("Play_Player", gameObject);
    yield return new WaitForSeconds(he.playerTime);

    AkSoundEngine.PostEvent("Stop_Player", gameObject);
    yield return new WaitForSeconds(1.5f);

    AkSoundEngine.PostEvent(he.post, gameObject);
    TextWriter.AddWriter_Static(haroldSpeech, he.haroldTalk_normal);
    yield return new WaitForSeconds(he.haroldTime);

    UIManager.gameState = "play";
    gameObject.SetActive(false);
  }

  void OnEnable()
  {
    crossBank.Load();

    haroldSpeech.text = "";
    playerSpeech.text = "";
    avalibleWords = new List<Word>();

    CreateWords();
    DoGrid();
    PlaceWords();

    AkSoundEngine.PostEvent(hello.post, gameObject);
    TextWriter.AddWriter_Static(haroldSpeech, hello.haroldTalk_normal);
  }

  void OnDisable()
  {
    crossBank.Unload();

  }

  void PlaceWords()
  {
    List<int> wordsInPlay = new List<int>();

    //apply postIt header
    postIt.text = "acceptable topics\nof conversation:\n\n";

    //display no more than 3 words from list of avalible words
    if (avalibleWords.Count > 2)
    {
      while (wordsInPlay.Count < 3)
      {
        int index = Random.Range(0, avalibleWords.Count);
        
        if (!wordsInPlay.Contains(index))
        {
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
    //always avalible
    avalibleWords.Add(goodbye);
    PlaceWord(goodbye);
    
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

        //break for over 500 attempts
        counter ++;
        if (counter >= 500)
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
          StartCoroutine(Goodbye(he));
        }
        else
        {
          StartCoroutine(ChangeBoards(he));
          he.used = true;
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
    public string post;
    public string playerTalk_normal;
    public float playerTime;
    public string haroldTalk_normal;
    public float haroldTime;

    //additions for when it gets chaotic

    // public List<Word> oddOptions;
    // public List<Word> whackOptions;
    // public string playerTalk_odd;
    // public string playerTalk_whack;
    // public string haroldTalk_odd;
    // public string haroldTalk_wack;
    public bool used = false;
    public (int, int) direction;
    public (int, int) origin;
    public GameObject startButton;
    public GameObject endButton;

    public Word(List<Word> allWords)
    {
      allWords.Add(this);
    }
    public Word(List<Word> allWords, List<Word> first)
    {
      allWords.Add(this);
      first.Add(this);
    }
  }


///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


public void CreateWords()
{
//define every word and its responses
hello = new Word(allWords){
  word = "hello",
  playerTalk_normal = "",
  haroldTalk_normal = "",};

goodbye = new Word (allWords) {
  word = "GOODBYE",
  postItDes = "end the exchange",
  playerTalk_normal = "alright, bud, I should get back to work",
  haroldTalk_normal = "",};

Word weather = new Word (allWords, avalibleWords) {
  word = "WEATHER",
  postItDes = "the climate",
  playerTalk_normal = "so, how about that weather, huh?",
  haroldTalk_normal = "yeah, quite the weather",};

  Word snow = new Word (allWords) {
    word = "SNOW",
    postItDes = "precipitation",
    playerTalk_normal = "I really wish it snowed more, I quite like snow",
    haroldTalk_normal = "I'm more of a sunshine guy myself"};

  Word clothes = new Word (allWords) {
    word = "CLOTHES",
    postItDes = "garments",
    playerTalk_normal = "have enough clothes for the upcoming season?",
    haroldTalk_normal = "plan on going to the store with my kids to get jackets next week"};

  Word coffee = new Word (allWords) {
    word = "COFFEE",
    postItDes = "warm drinks",
    playerTalk_normal = "cup o' joe really helps with the cold",
    haroldTalk_normal = "I take mine with 2 sugars"};
  
Word child = new Word (allWords, avalibleWords) {
  word = "CHILD",
  postItDes = "harold's offspring",
  playerTalk_normal = "how are the kids doing these days?",
  haroldTalk_normal = "little Jimmy's playing soccer, he's quite the young man"};

  Word school = new Word (allWords) {
    word = "SCHOOL",
    postItDes = "education",
    playerTalk_normal = "nice, how is the schooling coming along?",
    haroldTalk_normal = "very well. B's across the board"};

  Word growth = new Word (allWords) {
    word = "GROWTH",
    postItDes = "how tall Jimmy is",
    playerTalk_normal = "he's growing up so fast, pretty tall for his age",
    haroldTalk_normal = "yep, soon enough he'll pass me up"};

  Word soccer = new Word (allWords) {
    word = "SOCCER",
    postItDes = "Jimmy's athletics",
    playerTalk_normal = "Jimmy still playing ball with the team?",
    haroldTalk_normal = "oh, yeah, he's got a scrimmage next week"};

  Word internet = new Word (allWords) {
    word = "INTERNET",
    postItDes = "kids these days",
    playerTalk_normal = "he on that tink tonk like all the other kids",
    haroldTalk_normal = "tink tonk, instant grams, all the same to me"};

Word work = new Word (allWords, avalibleWords) {
      word = "WORK",
      postItDes = "the job",
      playerTalk_normal = "work's really been grinding my gears lately",
      haroldTalk_normal = "tell me about it"};

  Word boss = new Word (allWords) {
    word = "BOSS",
    postItDes = "the big man",
    playerTalk_normal = "boss man's been breathing down my neck",
    haroldTalk_normal = "guess that's why they pay him the big bucks"};
  
  Word hours = new Word (allWords) {
    word = "HOURS",
    postItDes = "9 to 5",
    playerTalk_normal = "can't wait till we're off at 5",
    haroldTalk_normal = "yep, I'll be going home to watch some ball"};
  
  Word merger = new Word (allWords) {
    word = "MERGER",
    postItDes = "getting bought out",
    playerTalk_normal = "any updates on the merger?",
    haroldTalk_normal = "no updates yet from the man upstairs"};
  
  Word client = new Word (allWords) {
    word = "CLIENT",
    postItDes = "the people you sell to",
    playerTalk_normal = "I dealth with a pretty annoying client today",
    haroldTalk_normal = "they whine like children, I tell ya"};

Word movies = new Word (allWords, avalibleWords) {
      word = "MOVIES",
      postItDes = "entertainment",
      playerTalk_normal = "you a movie guy?",
      haroldTalk_normal = "love the big screen"};

  Word release = new Word (allWords) {
      word = "RELEASE",
      postItDes = "new to theaters",
      playerTalk_normal = "see any new ones letely?",
      haroldTalk_normal = "the family did a movie marathon when it snowed a few days ago"};
  
  Word actor = new Word (allWords) {
      word = "ACTOR",
      postItDes = "the guy on screen",
      playerTalk_normal = "that one guy's getting really famous, huh",
      haroldTalk_normal = "he's in everything these days"};

  Word scifi = new Word (allWords) {
      word = "SCIFI",
      postItDes = "movies in space",
      playerTalk_normal = "I miss all the good scifi from our childhood",
      haroldTalk_normal = "visual effects have come such a long way"};

  Word prices = new Word (allWords) {
      word = "PRICES",
      postItDes = "ticket costs",
      playerTalk_normal = "theaters are really ripping us off these days",
      haroldTalk_normal = "back when we were kids, tickets were just a dollar, remember?"};

Word sports = new Word (allWords) {
      word = "SPORTS",
      postItDes = "athletics",
      playerTalk_normal = "how bout them sports",
      haroldTalk_normal = "yup"};

  Word game = new Word (allWords) {
      word = "GAME",
      postItDes = "a recent competition",
      playerTalk_normal = "did you watch the game last night?",
      haroldTalk_normal = "oh yeah, what a comeback"};

  Word team = new Word (allWords) {
      word = "TEAM",
      postItDes = "blue jerseys",
      playerTalk_normal = "I'm a pretty big fan of the blue team",
      haroldTalk_normal = "I'm a red team fan myself"};

  Word varsity = new Word (allWords) {
      word = "VARSITY",
      postItDes = "back in high school",
      playerTalk_normal = "reminds me of the good ol' days as a lineman for the high school",
      haroldTalk_normal = "that sure takes me back"};




//add branches to each word
//check milanote for map
weather.normalOptions = new List<Word>(){snow, clothes, coffee};
  snow.normalOptions = new List<Word>(){clothes, coffee};
  clothes.normalOptions = new List<Word>(){child, coffee, snow};
  coffee.normalOptions = new List<Word>(){clothes, snow};
child.normalOptions = new List<Word>(){school, growth, soccer, internet};
  school.normalOptions = new List<Word>(){growth, soccer, internet};
  growth.normalOptions = new List<Word>(){school, soccer, internet};
  soccer.normalOptions = new List<Word>(){growth, internet, school, sports};
  internet.normalOptions = new List<Word>(){growth, soccer, school};
work.normalOptions = new List<Word>(){boss, hours, merger, client};
  boss.normalOptions = new List<Word>(){hours, merger, client};
  hours.normalOptions = new List<Word>(){boss, merger, sports};
  merger.normalOptions = new List<Word>(){boss, client};
  client.normalOptions = new List<Word>(){boss, hours, merger, child};
movies.normalOptions = new List<Word>(){release, actor, scifi, prices};
  release.normalOptions = new List<Word>(){actor, scifi, prices, snow, child};
  actor.normalOptions = new List<Word>(){release, scifi, prices};
  scifi.normalOptions = new List<Word>(){release, actor, prices};
  prices.normalOptions = new List<Word>(){release, actor, scifi, prices};
sports.normalOptions = new List<Word>(){game, team, varsity};
  game.normalOptions = new List<Word>(){team, varsity};
  team.normalOptions = new List<Word>(){game, varsity};
  varsity.normalOptions = new List<Word>(){game, team, soccer};


foreach (Word he in allWords)
{
  he.post = "Play_" + he.word.ToLower();
  he.playerTime = ((float)he.playerTalk_normal.Length * .05f) + Random.Range(0f, 1f);
  he.haroldTime = ((float)he.haroldTalk_normal.Length * .05f);
}

int goodbyeIndex = Random.Range(0, 4);
goodbyeRTPC.SetValue(gameObject, goodbyeIndex);
goodbye.haroldTime = 2f;
switch (goodbyeIndex)
{
case 0:
goodbye.haroldTalk_normal = "adios amigo";
break;

case 1:
goodbye.haroldTalk_normal = "alrighty";
break;

case 2:
goodbye.haroldTalk_normal = "see ya";
break;

case 3:
goodbye.haroldTalk_normal = "till next time";
break;
}

int helloIndex = Random.Range(0, 4);
helloRTPC.SetValue(gameObject, helloIndex);
hello.haroldTime = 2f;
switch (helloIndex)
{
case 0:
hello.haroldTalk_normal = "hello";
break;

case 1:
hello.haroldTalk_normal = "howdy";
break;

case 2:
hello.haroldTalk_normal = "good to see ya again";
break;

case 3:
hello.haroldTalk_normal = "oh, hey";
break;
}
}
}