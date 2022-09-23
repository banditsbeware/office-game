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


  [SerializeField] public GameObject[,] letterMatrix;

  // objects for instantiating buttons
  public GameObject exampleButton;
  private GameObject button;
  private List<GameObject> buttons;
  private Image buttonImage;

  //word placement variables
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
    "SW",
    "NE",
    "SE"
  };

  void Start()
  {
    CreateGrid();

    //debugging stuff
    PlaceWord("XXXXXXXXXX", "W", (0, 5));
    PlaceWord("BEEF");
    // letterMatrix[4, 4].GetComponent<Image>().color = new Color32(150, 255, 0, 100);
  }

  void Update() {
      
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

  void PlaceWord(string word, string facing, (int, int) origin)
  {
    (int, int) direction = dirDict[facing];

    //iterate through each button object in a direction from an origin, change its text to word letter
    for (int x = 0; x < word.Length; x++)
    {
      button = letterMatrix[origin.Item1 + direction.Item1 * x, origin.Item2 + direction.Item2 * x];
      ButtonTextComponent(button).text = word[x].ToString();
      button.GetComponent<WordsearchButton>().usedByOtherWord = true;
    }
  }
  void PlaceWord(string word)
  {
    (int, int) direction = dirDict[dirList[Random.Range(0, 7)]];
    (int, int) origin = (Random.Range(0, size), Random.Range(0, size));
    bool overlap = true;
    int counter = 0;
    
    //check bounds of grid, and whether any letters overlap existing words
    while (overlap == true)
    {
      while ((0 > origin.Item1 + direction.Item1 * word.Length) || (origin.Item1 + direction.Item1 * word.Length > size) ||
            (0 > origin.Item2 + direction.Item2 * word.Length) || (origin.Item2 + direction.Item2 * word.Length > size))
      {
        direction = dirDict[dirList[Random.Range(0, 7)]];
        origin = (Random.Range(0, size), Random.Range(0, size));
        Debug.Log("Attempt to be in square!");

        //break for over 200 attempts
        counter ++;
        if (counter >= 200)
        {
          Debug.Log("Too many tries :(");
          break;
        }
      }

      //check overlap
      overlap = false;
      for (int x = 0; x < word.Length; x++)
      {
        WordsearchButton heIs = letterMatrix[origin.Item1 + direction.Item1 * x, origin.Item2 + direction.Item2 * x].GetComponent<WordsearchButton>();
    
        if (heIs.usedByOtherWord) //gets variable from button script within matrix
        {
          overlap = true;
        }
      }
      if (overlap == true)
      {
        origin = (Random.Range(0, size), Random.Range(0, size));
        direction = dirDict[dirList[Random.Range(0, 7)]];
      }

      Debug.Log("Check for overlap!");
    }
    //iterate through each button object in a direction from an origin, change its text to word letter
    for (int x = 0; x < word.Length; x++)
    {
      button = letterMatrix[origin.Item1 + direction.Item1 * x, origin.Item2 + direction.Item2 * x];
      ButtonTextComponent(button).text = word[x].ToString();
      button.GetComponent<WordsearchButton>().usedByOtherWord = true;

      Debug.Log(origin.ToString() + direction.ToString());
    }
  }

  TMP_Text ButtonTextComponent(GameObject button)
  {
    return button.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
  }
}
