using System;
using UnityEngine;

namespace SpeakEasy.Data
{

    [Serializable]
    public class SEIfData : SEChoiceData
    {
        [field: SerializeField] public string contextVariableName {get; set;}
        [field: SerializeField] public string comparisonSign {get; set;}
        [field: SerializeField] public string comparisonValue {get; set;}
        [field: SerializeField] public bool isMetaVariableComparison {get; set;}

        
    }
}
