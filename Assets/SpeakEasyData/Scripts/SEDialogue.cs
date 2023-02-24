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
  

  //The Class you place on a Dialogue Handler to operate in Game
  public class SEDialogue : MonoBehaviour
    {
        //Dialogue ScriptableObjects
        [SerializeField] private SEContainerSO container;
        [SerializeField] private SEGroupSO group;
        [SerializeField] private SENodeSO node;
        [SerializeField] private SENodeSO entryNode;

        //Filters
        [SerializeField] private bool startingNodesOnly;

        //Indexes
        [SerializeField] private int selectedGroupIndex;
        [SerializeField] private int selectedNodeIndex;

        //Objects
        [SerializeField] private GameObject playerSpeechBubble;
        [SerializeField] private TMP_Text playerSpeechText;

        [SerializeField] private GameObject npcSpeechBubble;
        [SerializeField] private TMP_Text npcSpeechText;
        [SerializeField] private Animator npcAnimator;

        [SerializeField] private List<GameObject> choiceButtons;

        private Coroutine speakingCoroutine;

        private Image playerBubbleImage;
        private Image npcBubbleImage;

        private Sprite playerBubbleSprite;
        private Sprite npcBubbleSprite;
        private Sprite emptySprite;

        private void Awake() 
        {
            playerSpeechText = playerSpeechBubble.GetComponentInChildren<TMP_Text>();
            npcSpeechText = npcSpeechBubble.GetComponentInChildren<TMP_Text>();

            npcBubbleImage = npcSpeechBubble.GetComponent<Image>();
            playerBubbleImage = playerSpeechBubble.GetComponent<Image>();

            npcBubbleSprite = npcBubbleImage.sprite;
            playerBubbleSprite = playerBubbleImage.sprite;

            emptySprite = AssetDatabase.LoadAssetAtPath<Sprite>("Assets/Sprites/empty.png");
        }

        //sets all the default sprites based on what is set up in the editor, starts the graph at _entry
        private void OnEnable() 
        {
            npcSpeechText.text = "";
            playerSpeechText.text = "";

            npcBubbleImage.sprite = emptySprite;
            playerBubbleImage.sprite = emptySprite;

            HideChoices();

            node = entryNode;
            node = NextNode();

            StartCoroutine(BeginningDelay(1f));
        }

        // private void Update() 
        // {
        //     if (Input.anyKeyDown)
        //     {
        //         Debug.Log(speakingCoroutine != null);
        //     }
        // }

        #region Event Handling

        private void BeginNode()
        {
            PerformCallbacks();

            if (node.NodeType == SingleChoice || node.NodeType == MultiChoice) //if it's a speaking node
            {
                if (node.IsPlayer)
                {
                    speakingCoroutine = StartCoroutine(PlayerSpeak());
                }
                else
                {
                    speakingCoroutine = StartCoroutine(NpcSpeak());
                }
                return;
            }
            else if (node.NodeType == If)
            {
                ParseLogic();
            }
            else if (node.NodeType == WeightedRandom)
            {
                ChooseWeightedRandom();
            }
            else if (node.NodeType == Exit)
            {
                UIManager.gameState = "play";
                transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log("Whoops, this node shouldn't exist!");
            }
            
        }

        private SENodeSO NextNode(int choiceIndex = 0)
        {
            SENodeSO nextNode = node.Choices[choiceIndex].NextDialogue;

            return nextNode;
        }

        private SENodeSO NextLogicStep(SENodeSO currentNode, int choiceIndex = 0)
        {

            SENodeSO nextNode = currentNode.IfStatements[choiceIndex].NextDialogue;

            return nextNode;
        }

        public void ChoiceMade(GameObject button)
        {
            int choiceIndex = choiceButtons.IndexOf(button);

            node = NextNode(choiceIndex);

            HideChoices();

            npcSpeechText.text = "";
            npcBubbleImage.sprite = emptySprite;

            BeginNode();
        }

        public void PerformCallbacks()
        {
            foreach (SECallbackSaveData callback in node.Callbacks)
            {
                Type variableType = meta.Variables[callback.callbackVariableName].GetType();

                switch (callback.callbackAction)
                {
                    case "SetValue":
                        if (variableType == typeof(string)) meta.Variables[callback.callbackVariableName] = callback.callbackValue;
                        if (variableType == typeof(int)) meta.Variables[callback.callbackVariableName] = int.Parse(callback.callbackValue);
                        if (variableType == typeof(bool)) meta.Variables[callback.callbackVariableName] = bool.Parse(callback.callbackValue);
                        break;
                    case "Increment":
                        meta.Variables[callback.callbackVariableName] += int.Parse(callback.callbackValue);
                        break;
                    case "Decrement":
                        meta.Variables[callback.callbackVariableName] -= int.Parse(callback.callbackValue);
                        break;
                }
            }
        }

        #endregion

        #region Speaking

        IEnumerator PlayerSpeak()
        {
            TextWriter.AddWriter_Static(playerSpeechText, node.DialogueText);
            playerBubbleImage.sprite = playerBubbleSprite;

            AkSoundEngine.PostEvent("Play_Player", gameObject);

            yield return new WaitForSeconds(node.speechTime);

            AkSoundEngine.PostEvent("Stop_Player", gameObject);

            yield return new WaitForSeconds(1.5f);
        
            playerSpeechText.text = "";
            playerBubbleImage.sprite = emptySprite;

            speakingCoroutine = null;

            node = NextNode();

            BeginNode();
        }


        IEnumerator NpcSpeak()
        {
            npcAnimator.SetBool("isSpeaking", true);
            npcBubbleImage.sprite = npcBubbleSprite;

            //AkSoundEngine.PostEvent(word.post, gameObject);

            TextWriter.AddWriter_Static(npcSpeechText, node.DialogueText);
            

            yield return new WaitForSeconds(node.speechTime);

            npcAnimator.SetBool("isSpeaking", false);

            speakingCoroutine = null;

            if (node.NodeType == MultiChoice)
            {
                PresentChoices();
            }
            else
            {
                node = NextNode();
                BeginNode();
            }   
        }

        #endregion

        #region Logic-ing

        private void ParseLogic()
        {
            if (meta.IfStatement(node.IfStatements[0])) //checks the truth of the if statement in an if node
            {
                node = NextLogicStep(node, 0);
            }
            else
            {
                node = NextLogicStep(node, 1);
            }

            BeginNode();
        }

        private void ChooseWeightedRandom()
        {
            int totalWeight = 0;
            int runningTotal = 0;

            foreach (SEChoiceData possibility in node.Choices)
            {
                totalWeight += int.Parse(possibility.Text);
            }

            int randy = UnityEngine.Random.Range(0, totalWeight);

            for (int i = 0; i < node.Choices.Count; i++)
            {
                runningTotal += int.Parse(node.Choices[i].Text);

                if (randy < runningTotal)
                {
                    node = NextNode(i);
                    BeginNode();
                }
            }
        }

        #endregion

        #region Utility

        private bool SomeoneIsSpeaking()
        {
            return speakingCoroutine != null;
        }

        private void PresentChoices()
        {
            int numberOfChoices = node.Choices.Count;
            for (int i = 0; i < numberOfChoices; i++)
            {
                choiceButtons[i].SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = node.Choices[i].Text;  
            }
        }

        private void HideChoices()
        {
            foreach (GameObject button in choiceButtons)
            {
                button.SetActive(false);
            }
        }

        IEnumerator BeginningDelay(float delay)
        {
            yield return new WaitForSeconds(delay);

            BeginNode();
        }
 
        #endregion
    }
}
