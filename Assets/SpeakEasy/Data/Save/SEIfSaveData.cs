using System;
using UnityEngine;

namespace SpeakEasy.Data.Save
{
    //holds data about ports to be put in the NodeSaveData, for rebuilding the graph on load
    [Serializable]
    public class SEIfSaveData : SEChoiceSaveData
    {
        [field: SerializeField] public string contextVariableName {get; set;}
        [field: SerializeField] public string comparisonSign {get; set;}
        [field: SerializeField] public string comparisonValue {get; set;}
    }
}
