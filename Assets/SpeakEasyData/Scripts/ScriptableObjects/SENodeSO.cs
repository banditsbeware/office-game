using System.Collections.Generic;
using UnityEngine;

namespace SpeakEasy.ScriptableObjects
{
    using Enumerations;
    using static Enumerations.SENodeType;
    using Data;
    using Data.Save;
    public class SENodeSO : ScriptableObject
    {
        [field: SerializeField] public string NodeName {get; set;}
        [field: SerializeField] public bool IsPlayer {get; set;}
        [field: SerializeField] [field: TextArea] public string DialogueText {get; set;}
        [field: SerializeField] public List<SEChoiceData> Choices {get; set;}
        [field: SerializeField] public List<SEIfData> IfStatements {get; set;}
        [field: SerializeField] public List<SECallbackSaveData> Callbacks {get; set;}
        [field: SerializeField] public SENodeType NodeType {get; set;}
        [field: SerializeField] public float SpeechTime {get; set;}

        public void Initialize(string nodeName, string text, (List<SEChoiceData>, List<SEIfData>) choices, List<SECallbackSaveData> callbacks, SENodeType nodeType, bool isPlayer, float speechTime = 0f)
        {
            NodeName = nodeName;
            DialogueText = text;
            Choices = choices.Item1;
            IfStatements = choices.Item2;
            Callbacks = callbacks;
            NodeType = nodeType;
            IsPlayer = isPlayer;
            SpeechTime = speechTime;

            //if speech time has not been set by the player, it will automatically generate in the SO, but not the graphData
            if (nodeType == Speaking && SpeechTime == 0f)
            {
                SpeechTime = (float)text.Length * .05f + Random.Range(0f, 1f);
            }
        }


    }
}
