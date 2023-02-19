using System;
using UnityEngine;

namespace SpeakEasy.Data.Save
{
    //holds data about ports to be put in the NodeSaveData, for rebuilding the graph on load
    [Serializable]
    public class SEChoiceSaveData 
    {
        [field: SerializeField] public string Text {get; set;}
        [field: SerializeField] public string NodeID {get; set;}
    }
}
