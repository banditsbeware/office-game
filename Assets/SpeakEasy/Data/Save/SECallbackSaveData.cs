using System;
using UnityEngine;

namespace SpeakEasy.Data.Save
{
    //holds data about callbacks on speaking nodes to be put in the NodeSaveData
    [Serializable]
    public class SECallbackSaveData
    {
        [field: SerializeField] public string callbackVariableName {get; set;}
        [field: SerializeField] public string callbackAction {get; set;}
        [field: SerializeField] public string callbackValue {get; set;}
    }
}
