using System;
using System.Collections.Generic;
using UnityEngine;

namespace SpeakEasy.Data.Save
{
  using Enumerations;
  //holds data to be put in the GraphSaveDataSO, for rebuilding the graph on load
  [Serializable]
  public class SENodeSaveData 
  {
      [field: SerializeField] public string ID {get; set;}
      [field: SerializeField] public string Name {get; set;}
      [field: SerializeField] public string Text {get; set;}
      [field: SerializeField] public float SpeechTime {get; set;}
      [field: SerializeField] public bool IsPlayer {get; set;}
      [field: SerializeField] public List<SEChoiceSaveData> Choices {get; set;}
      [field: SerializeField] public List<SEIfSaveData> IfStatements {get; set;}
      [field: SerializeField] public List<SECallbackSaveData> Callbacks {get; set;}
      [field: SerializeField] public string GroupID {get; set;}
      [field: SerializeField] public SENodeType NodeType {get; set;}
      [field: SerializeField] public Vector2 Position {get; set;}
  }
}
