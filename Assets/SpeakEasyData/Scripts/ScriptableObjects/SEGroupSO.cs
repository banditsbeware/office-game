using System;
using UnityEngine;

namespace SpeakEasy.ScriptableObjects
{
    [Serializable]
    public class SEGroupSO : ScriptableObject
    {
        
        [field: SerializeField] public string GroupName {get; set;}

        public void Initialize(string groupName)
        {
            GroupName = groupName;
        }
    }
}
