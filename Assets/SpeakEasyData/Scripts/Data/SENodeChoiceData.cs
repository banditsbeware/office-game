using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpeakEasy.Data
{
    using ScriptableObjects;

    [Serializable]
    public class SEChoiceData
    {
        [field: SerializeField] public string Text {get; set;}
        [field: SerializeField] public SENodeSO NextDialogue {get; set;}
    }
}
