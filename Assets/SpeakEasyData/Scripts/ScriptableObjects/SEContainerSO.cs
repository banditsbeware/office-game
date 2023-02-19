using System.Collections.Generic;
using UnityEngine;

namespace SpeakEasy.ScriptableObjects
{
    using Enumerations;
    //holds information about each dialogue graph to be used during runtime
    public class SEContainerSO : ScriptableObject
    {
        [field: SerializeField] public string FileName {get; set;}
        [field: SerializeField] public SerializableDictionary<SEGroupSO, List<SENodeSO>> NodeGroups {get; set;}
        [field: SerializeField] public List<SENodeSO> UngroupedNodes {get; set;}
        
        public void Initialize(string fileName)
        {
            FileName = fileName;

            NodeGroups = new SerializableDictionary<SEGroupSO, List<SENodeSO>>();
            UngroupedNodes = new List<SENodeSO>();
        }

        public List<string> GetGroupNames()
        {
            List<string> groupNames = new List<string>();

            foreach (SEGroupSO group in NodeGroups.Keys)
            {
                groupNames.Add(group.GroupName);
            }

            return groupNames;
        }

        public Dictionary<string, string> GetNodeNamesAndPaths(bool startingNodesOnly)
        {
            Dictionary<string, string> nodeNames = new Dictionary<string, string>();
            
            foreach (KeyValuePair<SEGroupSO, List<SENodeSO>> group in NodeGroups) 
            {
                List<SENodeSO> groupedNodes = group.Value;
    
                foreach (SENodeSO groupedNode in groupedNodes)
                {
                    if (startingNodesOnly && groupedNode.NodeType != SENodeType.Entry)
                    {
                        continue;
                    }
    
                    nodeNames.Add(groupedNode.NodeName, $"Groups/{group.Key.GroupName}/Nodes/");
                }
            }

            foreach (SENodeSO ungroupedNode in UngroupedNodes)
            {
                if (startingNodesOnly && ungroupedNode.NodeType != SENodeType.Entry)
                {
                    continue;
                }

                nodeNames.Add(ungroupedNode.NodeName, $"Global/Nodes/");
            }

            return nodeNames;
        }
    }
}