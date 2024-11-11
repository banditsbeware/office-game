using System;
using UnityEngine;

namespace SpeakEasy.Data.Save
{
    //holds data to be put in the GraphSaveDataSO, for rebuilding the graph on load
    [Serializable]
    public class SEGroupSaveData 
    {
        [field: SerializeField] public string ID {get; set;}
        [field: SerializeField] public string Name {get; set;}
        [field: SerializeField] public Vector2 Position {get; set;}
    }

}
