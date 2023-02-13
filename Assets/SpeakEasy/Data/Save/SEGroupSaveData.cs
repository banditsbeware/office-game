using System;
using UnityEngine;

namespace SpeakEasy.Data.Save
{
    [Serializable]
    public class SEGroupSaveData
    {
        [field: SerializeField] public string ID {get; set;}
        [field: SerializeField] public string Name {get; set;}
        [field: SerializeField] public Vector2 Position {get; set;}
    }

}
