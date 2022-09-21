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
  private List<GameObject> buttons;
  private Image buttonImage;

    void Start()
    {
      CreateGrid();
      
      // letterMatrix[4, 4].GetComponent<Image>().color = new Color32(150, 255, 0, 100);
    }

    void Update() {
        
    }

    void CreateGrid() 
    {
      //create matrix for manipulating letters
      GameObject[,] letterMatrix = new GameObject[size, size];

      //get grid components
      gridTrans = gridObject.GetComponent<RectTransform>();

      //set buttons to fill in grid entirely
      gridObject.GetComponent<GridLayoutGroup>().cellSize = new Vector2(gridTrans.sizeDelta.x / size, gridTrans.sizeDelta.y / size);

      for (int i = 0; i < size; i++)
      {
        for (int j = 0; j < size; j++)
        {
          //select random char from alphabet
          char letter = alpha[Random.Range(0, 25)];

          //create copy of button prefab
          GameObject button;
          button = Instantiate(exampleButton, gridObject.transform);

          //change text of the button to random char
          TMP_Text text = button.transform.GetChild(0).gameObject.GetComponent<TMPro.TextMeshProUGUI>();
          text.text = char.ToString(letter);

          //add to independent matrix
          letterMatrix[i, j] = button;
        }
      }
    }
}
