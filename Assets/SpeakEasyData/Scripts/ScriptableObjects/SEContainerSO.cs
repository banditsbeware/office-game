using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeakEasy.ScriptableObjects
{
    public class SEContainerSO : ScriptableObject
    {
        public string FileName {get; set;}
        public SerializableDictionary<SEGroupSO, List<SENodeSO>> DialogueGroups {get; set;}
        public List<SENodeSO> UngroupedNodes {get; set;}
        
        public void Initialize(string fileName)
        {
            FileName = fileName;

            DialogueGroups = new SerializableDictionary<SEGroupSO, List<SENodeSO>>();
            UngroupedNodes = new List<SENodeSO>();
        }
    }
}
