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
    using UnityEngine.EventSystems;
    using UnityEngine.InputSystem;


    //The Class you place on a Dialogue Handler to operate in Game
    public class SEDialogue : MonoBehaviour
    {
        //Dialogue ScriptableObjects
        [SerializeField] internal SEContainerSO container;
        [SerializeField] internal SEGroupSO group;
        [SerializeField] internal SENodeSO node;
        [SerializeField] internal SENodeSO entryNode;

        //Filters
        [SerializeField] internal bool startingNodesOnly;

        //Indexes
        [SerializeField] internal int selectedGroupIndex;
        [SerializeField] internal int selectedNodeIndex;
        [SerializeField] internal bool overlappingDialogues = true;

        //Objects
        [SerializeField] internal GameObject playerSpeechBubble;
        [SerializeField] internal TMP_Text playerSpeechText;

        [SerializeField] internal GameObject npcSpeechBubble;
        [SerializeField] internal TMP_Text npcSpeechText;
        [SerializeField] internal Animator npcAnimator;

        [SerializeField] public List<GameObject> choiceButtons = new List<GameObject>();

        [HideInInspector] internal Image playerBubbleImage;
        [HideInInspector] internal Image npcBubbleImage;

        [HideInInspector] internal Sprite playerBubbleSprite;
        [HideInInspector] internal Sprite npcBubbleSprite;
        [HideInInspector] internal Sprite emptySprite;
        [HideInInspector] internal DialogueActions actions;
    

        //Speaker tracking
        internal Coroutine speakingCoroutine;
        internal uint activeWwiseEvent;

        internal virtual void Awake() 
        {
            playerSpeechText = playerSpeechBubble.GetComponentInChildren<TMP_Text>();
            npcSpeechText = npcSpeechBubble.GetComponentInChildren<TMP_Text>();

            npcBubbleImage = npcSpeechBubble.GetComponent<Image>();
            playerBubbleImage = playerSpeechBubble.GetComponent<Image>();

            npcBubbleSprite = npcBubbleImage.sprite;
            playerBubbleSprite = playerBubbleImage.sprite;

            emptySprite = Resources.Load<Sprite>("Assets/Sprites/empty.png");

            actions = GetComponent<DialogueActions>();
        }

        //sets all the default sprites based on what is set up in the editor, starts the graph at _entry
        internal virtual void BeginDialogue()
        {
            playerSpeechText.text = "";
            playerBubbleImage.sprite = emptySprite;
            ClearNPC();

            HideChoices();

            node = entryNode;

            PerformCallbacks();
            
            node = NextNode();

            BeginNode();
        }

        #region Event Handling

        public virtual void BeginNode()
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
                    PresentChoices();
                    break;

                case If:
                case IfElseIf:
                    ParseIfLogic();
                    break;

                case WeightedRandom:
                    ChooseWeightedRandom();
                    break;

                case Exit:
                    UIManager.ExitWindow();
                    transform.parent.parent.gameObject.SetActive(false);
                    break;

                case Delay:
                    float delayTime = float.Parse(node.Choices[0].Text);
                    StartCoroutine(DelayNode(delayTime));
                    break;
                
                case Connector:
                    node = NextNode();
                    BeginNode();
                    break;
                
                case Animate:
                    StartCoroutine(TriggerAnimation());
                    break;
                
                //Next dialog will not be called until the function called in Action says so
                case Action:
                    actions.Invoke(node.DialogueText, 0);
                    node = NextNode();
                    break;

                default:
                    Debug.Log("Whoops, this node shouldn't exist!");
                    break;
            } 
        }

        internal virtual void OnDisable() 
        {
            if (SomeoneIsSpeaking())
            {
                AkSoundEngine.StopPlayingID(activeWwiseEvent);
            }
        }


        internal virtual SENodeSO NextNode(int choiceIndex = 0)
        {
            SENodeSO nextNode = node.Choices[choiceIndex].NextDialogue;

            return nextNode;
        }

        internal virtual SENodeSO NextLogicStep(int choiceIndex = 0)
        {
            SENodeSO nextNode = node.IfStatements[choiceIndex].NextDialogue;

            return nextNode;
        }

        //sees which button in choices list was selected, moved to that node in the dialogue tree
        public virtual void ChoiceMade(GameObject button)
        {
            int choiceIndex = choiceButtons.IndexOf(button); 

            if (choiceIndex < node.Choices.Count)
            {
                node = NextNode(choiceIndex);
            }
            else
            {
                node = NextLogicStep(choiceIndex - node.Choices.Count);
            }

            HideChoices();
            ClearNPC();

            BeginNode();
        }

        public virtual void ChoiceMade(string stringInput)
        {
            int choiceIndex = 0;

            foreach (SEChoiceData choice in node.Choices)
            {
                if (stringInput == choice.Text)
                {
                    choiceIndex = node.Choices.IndexOf(choice);
                    break;
                }
            }
    
            node = NextNode(choiceIndex);

            HideChoices();
            ClearNPC();

            BeginNode();
        }

        public void PerformCallbacks()
        {
            foreach (SECallbackSaveData callback in node.Callbacks)
            {
                Dictionary<string, dynamic> blackboard = Meta.BlackboardThatContains(callback.callbackVariableName);

                switch (callback.callbackAction)
                {
                    case "SetValue":
                        Meta.SetValue(callback.callbackVariableName, callback.callbackValue, blackboard);
                        break;
                    case "Increment":
                        if (blackboard[callback.callbackVariableName] is float)
                        {
                            blackboard[callback.callbackVariableName] += float.Parse(callback.callbackValue);
                            break;
                        }

                        blackboard[callback.callbackVariableName] += int.Parse(callback.callbackValue);
                        break;
                    case "Decrement":
                        if (blackboard[callback.callbackVariableName] is float)
                        {
                            blackboard[callback.callbackVariableName] -= float.Parse(callback.callbackValue);
                            break;
                        }
                        blackboard[callback.callbackVariableName] -= int.Parse(callback.callbackValue);
                        break;
                }
            }
        }

        #endregion

        #region Speaking

        internal virtual IEnumerator PlayerSpeak()
        {
            if (!overlappingDialogues)
            {
                ClearNPC();
            }
            
            TextWriter.AddWriter_Static(playerSpeechText, node.DialogueText);
            playerBubbleImage.sprite = playerBubbleSprite;

            activeWwiseEvent = AkSoundEngine.PostEvent("Play_Player", gameObject);

            yield return new WaitForSeconds(node.SpeechTime);

            AkSoundEngine.PostEvent("Stop_Player", gameObject);

            yield return new WaitForSeconds(1f);
        
            playerSpeechText.text = "";
            playerBubbleImage.sprite = emptySprite;

            speakingCoroutine = null;

            node = NextNode();
            BeginNode();
        }


        public IEnumerator NpcSpeak()
        {
            npcAnimator.SetBool("isSpeaking", true);
            npcBubbleImage.sprite = npcBubbleSprite;

            //for implementing dialogue prior to recording. only tries playing if event exists
            if (AkSoundEngine.PrepareEvent(AkPreparationType.Preparation_Load, new string[]{$"Play_{node.NodeName}"}, 1) == AKRESULT.AK_IDNotFound)
            {
                Debug.Log("Whoops, Wwise event doesn't exist yet");
            }
            else
            {
                activeWwiseEvent = AkSoundEngine.PostEvent($"Play_{node.NodeName}", gameObject);
            }

            TextWriter.AddWriter_Static(npcSpeechText, node.DialogueText);
            
            yield return new WaitForSeconds(node.SpeechTime);

            npcAnimator.SetBool("isSpeaking", false);
            speakingCoroutine = null;

            node = NextNode();
            BeginNode();
        }

        public IEnumerator TriggerAnimation()
        {
            npcAnimator.SetTrigger(node.DialogueText);

            yield return new WaitForSeconds(node.SpeechTime);

            node = NextNode();
            BeginNode();
        }

        #endregion

        #region Logic-ing

        internal void ParseIfLogic()
        {
            for (int i = 0; i < node.IfStatements.Count; i++)
            {
                if (i == node.IfStatements.Count - 1) //don't send the else statement thru Meta.isIfStatementTrue
                {
                    node = NextLogicStep(i);
                    break;
                }
                if (Meta.isIfStatementTrue(node.IfStatements[i]))
                {
                    node = NextLogicStep(i);
                    break;
                }
            }
            BeginNode();
        }

        internal void ChooseWeightedRandom()
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

        internal virtual void PresentChoices()
        {
            
            int numberOfChoices = node.Choices.Count;
            for (int i = 0; i < numberOfChoices; i++)
            {
                choiceButtons[i].SetActive(true);
                choiceButtons[i].GetComponentInChildren<TMP_Text>().text = node.Choices[i].Text;  
            }

            int numberOfDependentChoices = node.IfStatements.Count;
            for (int i = 0; i < numberOfDependentChoices; i++)
            {
                if(Meta.isIfStatementTrue(node.IfStatements[i])) 
                {
                    choiceButtons[i + numberOfChoices].SetActive(true);
                    choiceButtons[i + numberOfChoices].GetComponentInChildren<TMP_Text>().text = node.IfStatements[i].Text;
                }  
            }
        }

        private void HideChoices()
        {
            EventSystem.current.SetSelectedGameObject(null);

            foreach (GameObject button in choiceButtons)
            {
                button.SetActive(false);
            }
        }

        IEnumerator DelayNode(float delay)
        {
            node = NextNode();

            yield return new WaitForSeconds(delay);

            BeginNode();
        }

        internal void ClearNPC()
        {
            npcSpeechText.text = "";
            npcBubbleImage.sprite = emptySprite;
        }
 
        #endregion
    }
}
