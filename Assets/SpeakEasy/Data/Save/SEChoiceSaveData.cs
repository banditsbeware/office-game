using System;
using UnityEngine;

namespace SpeakEasy.Data.Save
{
    using ScriptableObjects;
    [Serializable]
    public class SEChoiceSaveData
    {
        [field: SerializeField] public string Text {get; set;}
        [field: SerializeField] public string NodeID {get; set;}
    }
}
