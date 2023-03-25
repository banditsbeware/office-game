using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using TMPro;

namespace SpeakEasy
{
    using ScriptableObjects;
    using static Enumerations.SENodeType;
    using Data;
    using Data.Save;
  
    public class CrosswordDialogue : SEDialogue
    {
        //Jumped to Nodes, repeat tracker
        public SENodeSO goodbyeNode;
        public SENodeSO repeatWordNode;
        public bool wordRepeated;


        private static char[] alpha = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' }; 

        //grid variables
        public int size;
        private GameObject gridObject;
        private GridLayoutGroup gridComponent;
        private RectTransform gridTrans;

        //matrix of child objects
        public GameObject[,] letterMatrix;

        //objects for instantiating buttons
        public GameObject exampleButton;
        private GameObject button;
        private static int fontConst = 600;

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

        //selection variables 
        private bool wordCompleted = true;
        private GameObject firstSel;
        private GameObject secondSel;

        //words 
        public List<Word> avalibleWords = new List<Word>();
        public List<Word> allWords = new List<Word>();
        public List<string> usedWords = new List<string>();
        public TMP_Text postIt;

        //WWise
        public AK.Wwise.Bank crossBank;
    
        internal override void Awake() 
        {
            playerSpeechText = playerSpeechBubble.GetComponentInChildren<TMP_Text>();
            npcSpeechText = npcSpeechBubble.GetComponentInChildren<TMP_Text>();

            npcBubbleImage = npcSpeechBubble.GetComponent<Image>();
            playerBubbleImage = playerSpeechBubble.GetComponent<Image>();

            npcBubbleSprite = npcBubbleImage.sprite;
            playerBubbleSprite = playerBubbleImage.sprite;

            gridObject = transform.Find("wordGrid").gameObject;

            emptySprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/empty.png");
        }

        //sets all the default sprites based on what is set up in the editor, starts the graph at _entry
        internal override void OnEnable() 
        {
          if(transform.parent.parent.GetComponent<interact_minigame>().isInteractable) 
          {
              AkSoundEngine.SetState("room", "officeMinigame");
          }

          npcSpeechText.text = "";
          playerSpeechText.text = "";

          npcBubbleImage.sprite = emptySprite;
          playerBubbleImage.sprite = emptySprite;

          avalibleWords = new List<Word>();

          node = entryNode;

          if(transform.parent.parent.GetComponent<interact_minigame>().isInteractable) 
          {
              node = NextNode();
              DoGrid();
              BeginNode();
          }
            
        }

        void OnDisable()
        {
            AkSoundEngine.SetState("room", "office");
        }

        // private void Update() 
        // {
        //     if (Input.anyKeyDown)
        //     {
        //         Debug.Log(speakingCoroutine != null);
        //     }
        // }

        #region Event Handling

        internal override void BeginNode()
        {
            PerformCallbacks();

            switch (node.NodeType)
            {
                case Speaking:
                    if (node.IsPlayer)
                    {
                        speakingCoroutine = StartCoroutine(PlayerSpeak());
                    }
                    else
                    {
                        speakingCoroutine = StartCoroutine(NpcSpeak());
                    }
                    break;
                
                case MultiChoice:
                    StartCoroutine(ChangeBoards());
                    break;

                case If:
                case IfElseIf:
                    ParseIfLogic();
                    break;

                case WeightedRandom:
                    ChooseWeightedRandom();
                    break;

                case Exit:
                    UIManager.gameState = "play";
                    transform.parent.gameObject.SetActive(false);
                    break;

                case Delay:
                    float delayTime = float.Parse(node.Choices[0].Text);
                    StartCoroutine(DelayNode(delayTime));
                    break;
                
                case Connector:
                    node = NextNode();
                    BeginNode();
                    break;

                default:
                    Debug.Log("Whoops, this node shouldn't exist!");
                    break;
            } 
            
        }

        #endregion

        #region Utility

        IEnumerator DelayNode(float delay)
        {
            yield return new WaitForSeconds(delay);

            BeginNode();
        }

        #endregion
    
        internal override IEnumerator PlayerSpeak()
        {
            TextWriter.AddWriter_Static(playerSpeechText, node.DialogueText.Split("\n")[0]); //first half of text box, excludes post it description
            playerBubbleImage.sprite = playerBubbleSprite;

            AkSoundEngine.PostEvent("Play_Player", gameObject);

            yield return new WaitForSeconds(node.SpeechTime);

            AkSoundEngine.PostEvent("Stop_Player", gameObject);

            yield return new WaitForSeconds(1.5f);
        
            playerSpeechText.text = "";
            playerBubbleImage.sprite = emptySprite;

            speakingCoroutine = null;

            if (wordRepeated)
            {
              wordRepeated = false;
              node = repeatWordNode;
            }

            node = NextNode();
            BeginNode();
        }


        #region Crossword

        IEnumerator ChangeBoards()
        {
          DoGrid();
          yield return new WaitForSeconds(.5f);
          DoGrid();
          yield return new WaitForSeconds(.5f);
          DoGrid();
          PlaceWords();
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
              char letter = alpha[UnityEngine.Random.Range(0, 25)];

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

        void PlaceWords()
        {
          avalibleWords.Clear();
          List<int> wordsInPlay = new List<int>();

          //apply postIt header
          postIt.text = "acceptable topics\nof conversation:\n";
          List<string> postItDescriptions = new List<string>();

          foreach (SEChoiceData choice in node.Choices)
          {
            string correctPostIt = node.DialogueText.Split("\n")[node.Choices.IndexOf(choice)];
            postItDescriptions.Add(correctPostIt);

            avalibleWords.Add(new Word(){
              word = choice.Text.ToUpper(),
              postItDes = correctPostIt
              });
          }

          //display no more than 3 words from list of avalible words
          if (avalibleWords.Count > 2)
          {
            while (wordsInPlay.Count < 3)
            {
              int index = UnityEngine.Random.Range(0, avalibleWords.Count);
              
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
          Word goodbye = new Word()
          {
            word = "GOODBYE",
            postItDes = "-say your goodbyes"
          };

          avalibleWords.Add(goodbye);
          PlaceWord(goodbye);
        }

        void PlaceWord(Word it, string facing, (int, int) origin)
        {
          it.direction = dirDict[facing];
          ChangeLetters(it);

          //add to list on postit
          postIt.text += "-" + it.postItDes + "\n";
        }
        void PlaceWord(Word word)
        {
          string wordString = word.word;

          word.direction = dirDict[dirList[UnityEngine.Random.Range(0, dirList.Length)]];
          word.origin = (UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size));

          bool overlap = true;
          int counter = 0;
          
          //check bounds of grid, and whether any letters overlap existing words
          while (overlap == true)
          {
            while ((word.origin.Item1 + word.direction.Item1 * wordString.Length < 0) || (word.origin.Item1 + word.direction.Item1 * wordString.Length > size) ||
                  (word.origin.Item2 + word.direction.Item2 * wordString.Length < 0) || (word.origin.Item2 + word.direction.Item2 * wordString.Length > size))
            {
              word.direction = dirDict[dirList[UnityEngine.Random.Range(0, 7)]];
              word.origin = (UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size));

              //break for over 500 attempts
              counter ++;
              if (counter >= 500)
              {
                Debug.Log("Too many tries :(");
                return;
              }
            }

            //check overlap
            overlap = false;
            for (int x = 0; x < wordString.Length; x++)
            {
              WordsearchButton heIs = letterMatrix[word.origin.Item1 + word.direction.Item1 * x, word.origin.Item2 + word.direction.Item2 * x].GetComponent<WordsearchButton>(); //gets button script to use variable
          
              if (heIs.usedByOtherWord) 
              {
                overlap = true;
              }
            }

            if (overlap == true)
            {
              word.origin = (UnityEngine.Random.Range(0, size), UnityEngine.Random.Range(0, size));
              // direction = dirDict[dirList[UnityEngine.Random.Range(0, 7)]];
            }
          }
          ChangeLetters(word);

          postIt.text += word.postItDes + "\n";
        }

        //iterate through each button object in a direction from an origin, change its text to word letter
        void ChangeLetters(Word word)
        {
          string wordString = word.word;
          for (int x = 0; x < wordString.Length; x++)
          {
            button = letterMatrix[word.origin.Item1 + word.direction.Item1 * x, word.origin.Item2 + word.direction.Item2 * x];
            ButtonTextComponent(button).text = wordString[x].ToString();
            button.GetComponent<WordsearchButton>().usedByOtherWord = true;

            if (Meta.DebuggingMode)
            {
              button.GetComponent<Image>().color = new Color32(200, 200, 0, 150);
            }
            
            //assign first and last button to Word object
            if (x == 0)
            {
              word.startButton = button;
            }
            else if (x == wordString.Length - 1)
            {
              word.endButton = button;
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
          //compare first and last button of selection and Word's button objects assigned in ChangeLetters()
          foreach (Word word in avalibleWords)
          {
            if ((firstSel == word.endButton || firstSel == word.startButton) && (secondSel == word.endButton || secondSel == word.startButton))
            {
              firstSel.GetComponent<Button>().enabled = false;
              secondSel.GetComponent<Button>().enabled = false;

              //take out buttons in middle of word
              for (int x = 1; x < word.word.Length - 1; x++)
              {
                button = letterMatrix[word.origin.Item1 + word.direction.Item1 * x, word.origin.Item2 + word.direction.Item2 * x];
                button.GetComponent<Image>().color = new Color32(200, 100, 100, 200);
                button.GetComponent<Button>().enabled = false;
                  
              }

              if (word.word == "GOODBYE")
              {
                node = goodbyeNode;
                node = NextNode();

                ClearNPC();
                BeginNode();

                return;
              }

              if (usedWords.Contains(word.word))
              {
                wordRepeated = true;
              }
              else
              {
                usedWords.Add(word.word);
              }

              int choiceIndex = avalibleWords.IndexOf(word);
              node = NextNode(choiceIndex);

              

              ClearNPC();
              BeginNode();

              return;
            }
          }

          //if no word is found
          firstSel.GetComponent<WordsearchButton>().Reset();
          secondSel.GetComponent<WordsearchButton>().Reset();
        }

    #endregion

  }
}
 
        


  