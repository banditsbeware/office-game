using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeakEasy.ScriptableObjects
{
    using Enumerations;
    using Data;
    public class SENodeSO : ScriptableObject
    {
        [field: SerializeField] public string NodeName {get; set;}
        [field: SerializeField] [field: TextArea] public string Text {get; set;}
        [field: SerializeField] public List<SEChoiceData> Choices {get; set;}
        [field: SerializeField] public SENodeType NodeType {get; set;}
        [field: SerializeField] public bool IsStartingDialogue {get; set;}

        public void Initialize(string nodeName, string text, List<SEChoiceData> choices, SENodeType nodeType, bool isStartingDialogue)
        {
            NodeName = nodeName;
            Text = text;
            Choices = choices;
            NodeType = nodeType;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}
