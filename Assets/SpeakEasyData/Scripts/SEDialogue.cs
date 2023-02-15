using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SpeakEasy
{
    using ScriptableObjects;
    using Enumerations;
    using Data;
    using UnityEngine.UIElements;

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


        private void OnEnable() 
        {
            playerSpeechText = playerSpeechBubble.GetComponentInChildren<TMP_Text>();
            npcSpeechText = npcSpeechBubble.GetComponentInChildren<TMP_Text>();

            HideChoices();

            node = NextNode(entryNode);

            BeginNode();
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
            if (node.NodeType == SENodeType.SingleChoice || node.NodeType == SENodeType.MultiChoice) //if it's a speaking node
            {
                if (node.IsPlayer)
                {
                    speakingCoroutine = StartCoroutine(PlayerSpeak(node));
                }
                else
                {
                    speakingCoroutine = StartCoroutine(NpcSpeak(node));
                }

                return;
            }
        }

        private SENodeSO NextNode(SENodeSO currentNode, int choiceIndex = 0)
        {
            SENodeSO nextNode = currentNode.Choices[choiceIndex].NextDialogue;

            return nextNode;
        }

        public void ChoiceMade(GameObject button)
        {
            int choiceIndex = choiceButtons.IndexOf(button);

            node = NextNode(node, choiceIndex);

            HideChoices();

            npcSpeechBubble.SetActive(false);
            playerSpeechBubble.SetActive(false);

            BeginNode();
        }

        #endregion

        #region Speaking

        IEnumerator PlayerSpeak(SENodeSO currentNode)
        {

            playerSpeechBubble.SetActive(true);

            TextWriter.AddWriter_Static(playerSpeechText, currentNode.DialogueText);

            AkSoundEngine.PostEvent("Play_Player", gameObject);

            yield return new WaitForSeconds(currentNode.speechTime);

            AkSoundEngine.PostEvent("Stop_Player", gameObject);

            yield return new WaitForSeconds(1.5f);
        
            playerSpeechBubble.SetActive(false);
            playerSpeechText.text = "";

            speakingCoroutine = null;

            node = NextNode(currentNode);

            BeginNode();
        }


        IEnumerator NpcSpeak(SENodeSO currentNode)
        {
            //npcAnimator.SetBool("isSpeaking", true);
            npcSpeechBubble.SetActive(true);

            //AkSoundEngine.PostEvent(word.post, gameObject);

            TextWriter.AddWriter_Static(npcSpeechText, currentNode.DialogueText);

            yield return new WaitForSeconds(currentNode.speechTime);

            //npcAnimator.SetBool("isSpeaking", false);

            speakingCoroutine = null;

            if (NextNode(currentNode).IsPlayer)
            {
                PresentChoices();
            }
            else
            {
                node = NextNode(currentNode);

                BeginNode();
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

        #endregion
    }
}
