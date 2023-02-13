using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace SpeakEasy.Data.Save
{
  
  using Enumerations;

    [Serializable]
    public class SENodeSaveData
    {
        public string ID {get; set;}
        [field: SerializeField] public string Name {get; set;}
        [field: SerializeField] public string Text {get; set;}
        [field: SerializeField] public List<SEChoiceSaveData> Choices {get; set;}
        [field: SerializeField] public string GroupID {get; set;}
        [field: SerializeField] public SENodeType NodeType {get; set;}
        [field: SerializeField] public Vector2 Position {get; set;}

    }
}
