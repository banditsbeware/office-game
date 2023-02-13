using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SpeakEasy.Data.Save
{
    public class SEGraphSaveDataSO : ScriptableObject
    {
        [field: SerializeField] public string FileName {get; set;}
        [field: SerializeField] public List<SEGroupSaveData> Groups {get; set;}
        [field: SerializeField] public List<SENodeSaveData> Nodes {get; set;}
        [field: SerializeField] public List<string> OldGroupNames {get; set;}
        [field: SerializeField] public List<string> OldUngroupedNames {get; set;}
        [field: SerializeField] public SerializableDictionary<string, List<string>> OldGroupedNames {get; set;}

        public void Initialize(string fileName)
        {
            FileName = fileName;

            Groups = new List<SEGroupSaveData>();
            Nodes = new List<SENodeSaveData>();
        }
    }
}
