using System.Collections.Generic;
using UnityEngine;

namespace SpeakEasy.ScriptableObjects
{
    using Enumerations;
    using static Enumerations.SENodeType;
    using Data;
    public class SENodeSO : ScriptableObject
    {
        [field: SerializeField] public string NodeName {get; set;}
        [field: SerializeField] public bool IsPlayer {get; set;}
        [field: SerializeField] [field: TextArea] public string DialogueText {get; set;}
        [field: SerializeField] public List<SEChoiceData> Choices {get; set;}
        [field: SerializeField] public List<SEIfData> IfStatements {get; set;}
        [field: SerializeField] public SENodeType NodeType {get; set;}
        [field: SerializeField] public float speechTime {get; set;}

        public void Initialize(string nodeName, string text, (List<SEChoiceData>, List<SEIfData>) choices, SENodeType nodeType, bool isPlayer)
        {
            NodeName = nodeName;
            DialogueText = text;
            Choices = choices.Item1;
            IfStatements = choices.Item2;
            NodeType = nodeType;
            IsPlayer = isPlayer;

            if (nodeType == SingleChoice || nodeType == MultiChoice)
            {
                speechTime = (float)text.Length * .05f + Random.Range(0f, 1f);
            }
        }


    }
}
