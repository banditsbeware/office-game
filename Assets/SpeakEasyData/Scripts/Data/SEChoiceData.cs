using System;
using UnityEngine;

namespace SpeakEasy.Data
{
    using ScriptableObjects;

    //held within the node Serializable Obejct to assist with runtime logic
    [Serializable]
    public class SEChoiceData
    {
        [field: SerializeField] public string Text {get; set;}
        [field: SerializeField] public SENodeSO NextDialogue {get; set;}
        
    }
}
