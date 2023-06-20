using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using System;

namespace SpeakEasy.Utilities
{
    using Data.Save;
    using ScriptableObjects;
    using Windows;
    using Elements;
    using SpeakEasy.Data;
    using Enumerations;
  
  //all the methods required for saving and loading the graph window, and saving and loading to scriptable objects
  public static class SEIOUtility
    {
        private static SEGraphView graphView;
        public static string graphFileName;
        private static string containerFolderPath;
        private static string bufferFolderPath;

        private static List<SEGroup> groups;   //used when getting elements from graphView
        private static List<SENode> nodes;

        private static Dictionary<string, SEGroupSO> createdGroups;   //dictionaries of element IDs and elements. Used for making ScriptableObjects
        private static Dictionary<string, SENodeSO> createdNodes;

        private static Dictionary<string, SEGroup> loadedGroups;   //used in Loading process
        private static Dictionary<string, SENode> loadedNodes;

        private static SEGraphSaveDataSO bufferGraph;

        public static void Initialize(SEGraphView seGraphView, string graphName)
        {
            graphView = seGraphView;
            graphFileName = graphName;
            containerFolderPath = $"Assets/SpeakEasyData/Dialogues/{graphFileName}";
            bufferFolderPath = $"Assets/SpeakEasyData/CopyPasteBuffer";

            groups = new List<SEGroup>();
            nodes = new List<SENode>();

            createdGroups = new Dictionary<string, SEGroupSO>();
            createdNodes = new Dictionary<string, SENodeSO>();

            loadedGroups = new Dictionary<string, SEGroup>();
            loadedNodes = new Dictionary<string, SENode>();
        }

        #region Save Methods
        public static void Save()
        {
            CreatePermanentFolders();

            GetElementsFromGraphView();

            SEGraphSaveDataSO graphData = CreateAsset<SEGraphSaveDataSO>("Assets/SpeakEasy/Graphs", graphFileName);
            graphData.Initialize(graphFileName);

            SEContainerSO container = CreateAsset<SEContainerSO>(containerFolderPath, graphFileName);
            container.Initialize(graphFileName);

            SaveGroups(graphData, container);
            SaveNodes(graphData, container);

            SaveAsset(graphData);
            SaveAsset(container);
        }

        private static void SaveAsset(UnityEngine.Object asset)
        {
            EditorUtility.SetDirty(asset);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        #region Groups
        private static void SaveGroups(SEGraphSaveDataSO graphData, SEContainerSO dialogueContainer)
        {
            List<string> groupNames = new List<string>();

            foreach (SEGroup group in groups)
            {
                SaveGroupToGraph(group, graphData);
                SaveGroupToSO(group, dialogueContainer);

                groupNames.Add(group.title);
            }

            UpdateOldGroups(groupNames, graphData);
        }

        public static void SaveGroupToGraph(SEGroup group, SEGraphSaveDataSO graphData)
        {
            SEGroupSaveData groupData = new SEGroupSaveData()
            {
                ID = group.ID,
                Name = group.title,
                Position = group.GetPosition().position
            };

            graphData.Groups.Add(groupData);
        }

        private static void SaveGroupToSO(SEGroup group, SEContainerSO dialogueContainer)
        {
            string groupName = group.title;

            CreateFolder($"{containerFolderPath}/Groups", groupName);
            CreateFolder($"{containerFolderPath}/Groups/{groupName}", "Nodes");

            SEGroupSO dialogueGroup = CreateAsset<SEGroupSO>($"{containerFolderPath}/Groups/{groupName}", groupName);

            dialogueGroup.Initialize(groupName);

            createdGroups.Add(group.ID, dialogueGroup);

            dialogueContainer.NodeGroups.Add(dialogueGroup, new List<SENodeSO>());

            SaveAsset(dialogueGroup);
        }
        
        private static void UpdateOldGroups(List<string> currentGroupNames, SEGraphSaveDataSO graphData)
        {
            if (graphData.OldGroupNames != null && graphData.OldGroupNames.Count != 0)
            {
                List<string> groupsToRemove = graphData.OldGroupNames.Except(currentGroupNames).ToList();

                foreach (string groupToRemove in groupsToRemove)
                {
                    RemoveFolder($"{containerFolderPath}/Groups/{groupToRemove}");
                }
            }

            graphData.OldGroupNames = new List<string>(currentGroupNames);
        }

        #endregion

        #region Nodes

        private static void SaveNodes(SEGraphSaveDataSO graphData, SEContainerSO container)
        {
            List<string> ungroupedNodeNames = new List<string>();
            SerializableDictionary<string, List<string>> groupedNodeNames = new SerializableDictionary<string, List<string>>();

            foreach (SENode node in nodes)
            {
                SaveNodeToGraph(node, graphData);
                SaveNodeToSO(node, container);

                if (node.Group != null)
                {
                    groupedNodeNames.AddItem(node.Group.title, node.NodeName);

                    continue;
                }

                ungroupedNodeNames.Add(node.NodeName);
            }

            UpdateChoiceConnections();

            UpdateOldGroupedNodes(groupedNodeNames, graphData);

            UpdateOldUngroupedNodes(ungroupedNodeNames, graphData);
        }

        public static void SaveNodeToGraph(SENode node, SEGraphSaveDataSO graphData)
        {
            List<SEChoiceSaveData> choices = CloneChoices(node.Choices);
            List<SEIfSaveData> ifs = CloneIfs(node.IfStatements);
            List<SECallbackSaveData> callbacks = CloneCallbacks(node.Callbacks);

            SENodeSaveData nodeData = new SENodeSaveData()
            {
                ID = node.ID,
                Name = node.NodeName,
                Choices = choices,
                IfStatements = ifs,
                Callbacks = callbacks,
                Text = node.DialogueText,
                SpeechTime = node.speechTime,
                GroupID = node.Group?.ID,
                NodeType = node.NodeType,
                Position = node.GetPosition().position,
                IsPlayer = node.IsPlayer
            };

            graphData.Nodes.Add(nodeData);
        }

        private static void SaveNodeToSO(SENode node, SEContainerSO container)
        {
            SENodeSO nodeSO;

            if (node.Group != null)
            {
                nodeSO = CreateAsset<SENodeSO>($"{containerFolderPath}/Groups/{node.Group.title}/Nodes", node.NodeName);

                container.NodeGroups.AddItem(createdGroups[node.Group.ID], nodeSO);
            }
            else
            {
                nodeSO = CreateAsset<SENodeSO>($"{containerFolderPath}/Global/Nodes", node.NodeName);

                container.UngroupedNodes.Add(nodeSO);
            }

            nodeSO.Initialize(node.NodeName, node.DialogueText, ConvertSaveDataToChoiceData(node.Choices, node.IfStatements), node.Callbacks, node.NodeType, node.IsPlayer, node.speechTime);

            createdNodes.Add(node.ID, nodeSO);

            SaveAsset(nodeSO);
        }

        private static (List<SEChoiceData>, List<SEIfData>) ConvertSaveDataToChoiceData(List<SEChoiceSaveData> choices, List<SEIfSaveData> ifStatements)
        {
            List<SEChoiceData> dialogueChoices = new List<SEChoiceData>();
            List<SEIfData> dialogueIfs = new List<SEIfData>();

            foreach (SEChoiceSaveData choice in choices)
            {
                SEChoiceData choiceData = new SEChoiceData()
                {
                    Text = choice.Text
                };
                dialogueChoices.Add(choiceData);
            }
            foreach (SEIfSaveData ifStatement in ifStatements)
            {
                SEIfData ifData = new SEIfData()
                {
                    Text = ifStatement.Text,
                    contextVariableName = ifStatement.contextVariableName,
                    comparisonSign = ifStatement.comparisonSign,
                    comparisonValue = ifStatement.comparisonValue,
                    isMetaVariableComparison = ifStatement.isMetaVariableComparison
                };

                dialogueIfs.Add(ifData);
            }

            return (dialogueChoices, dialogueIfs);
        }

        private static void UpdateChoiceConnections()
        {
            foreach (SENode node in nodes)
            {
                SENodeSO nodeSO = createdNodes[node.ID];

                for (int choiceIndex = 0; choiceIndex < node.Choices.Count; choiceIndex++)
                {
                    SEChoiceSaveData choice = node.Choices[choiceIndex];

                    if (string.IsNullOrEmpty(choice.NodeID))
                    {
                        continue;
                    }

                    nodeSO.Choices[choiceIndex].NextDialogue = createdNodes[choice.NodeID];
                }

                for (int ifIndex = 0; ifIndex < node.IfStatements.Count; ifIndex++)
                {
                    SEIfSaveData ifStatement = node.IfStatements[ifIndex];

                    if (string.IsNullOrEmpty(ifStatement.NodeID))
                    {
                        continue;
                    }

                    nodeSO.IfStatements[ifIndex].NextDialogue = createdNodes[ifStatement.NodeID];
                }

                SaveAsset(nodeSO);
            }
        }
        
        private static void UpdateOldUngroupedNodes(List<string> currrentUngroupedNames, SEGraphSaveDataSO graphData)
        {
            if (graphData.OldUngroupedNames != null && graphData.OldUngroupedNames.Count != 0)
            {
                List<string> nodesToRemove = graphData.OldUngroupedNames.Except(currrentUngroupedNames).ToList();

                foreach (string nodeToRemove in nodesToRemove)
                {
                    RemoveAsset($"{containerFolderPath}/Global/Nodes", nodeToRemove);
                }
            }

            graphData.OldUngroupedNames = new List<string>(currrentUngroupedNames);
        }

        private static void UpdateOldGroupedNodes(SerializableDictionary<string, List<string>> currentGroupedNames, SEGraphSaveDataSO graphData)
        {
            if (graphData.OldGroupedNames != null && graphData.OldGroupedNames.Count != 0)
            {
                foreach (KeyValuePair<string, List<string>> oldGroupedNode in graphData.OldGroupedNames)
                {
                    List<string> nodesToRemove = new List<string>();

                    if (currentGroupedNames.ContainsKey(oldGroupedNode.Key))
                    {
                        nodesToRemove = oldGroupedNode.Value.Except(currentGroupedNames[oldGroupedNode.Key]).ToList();
                    }

                    foreach (string nodeToRemove in nodesToRemove)
                    {
                        RemoveAsset($"{containerFolderPath}/Groups/{oldGroupedNode.Key}/Nodes", nodeToRemove);
                    }
                }
            }

            graphData.OldGroupedNames = new SerializableDictionary<string, List<string>>(currentGroupedNames);
        }
        #endregion
        
        private static List<SEChoiceSaveData> CloneChoices(List<SEChoiceSaveData> nodeChoices)
        {
            List<SEChoiceSaveData> choices = new List<SEChoiceSaveData>();

            foreach (SEChoiceSaveData choice in nodeChoices)
            {
                SEChoiceSaveData choiceData = new SEChoiceSaveData()
                {
                    Text = choice.Text,
                    NodeID = choice.NodeID
                };
                choices.Add(choiceData);
            }

            return choices;
        }
        private static List<SEIfSaveData> CloneIfs(List<SEIfSaveData> nodeIfs)
        {
            List<SEIfSaveData> ifs = new List<SEIfSaveData>();

            foreach (SEIfSaveData ifStatement in nodeIfs)
            {
                SEIfSaveData choiceData = new SEIfSaveData()
                {
                    Text = ifStatement.Text,
                    NodeID = ifStatement.NodeID,
                    contextVariableName = ifStatement.contextVariableName,
                    comparisonSign = ifStatement.comparisonSign,
                    comparisonValue = ifStatement.comparisonValue,
                    isMetaVariableComparison = ifStatement.isMetaVariableComparison
                };
                ifs.Add(choiceData);
            }

            return ifs;
        }

        private static List<SECallbackSaveData> CloneCallbacks(List<SECallbackSaveData> nodeCallbacks)
        {
            List<SECallbackSaveData> callbacks = new List<SECallbackSaveData>();

            foreach (SECallbackSaveData callback in nodeCallbacks)
            {
                SECallbackSaveData callbackData = new SECallbackSaveData()
                {
                    callbackVariableName = callback.callbackVariableName,
                    callbackAction = callback.callbackAction,
                    callbackValue = callback.callbackValue
                };
                callbacks.Add(callbackData);
            }

            return callbacks;
        }
        
        #endregion

        #region Load Methods

        public static void Load()
        {
            SEGraphSaveDataSO graphData = LoadAsset<SEGraphSaveDataSO>("Assets/SpeakEasy/Graphs", graphFileName);

            if (graphData == null)
            {
                EditorUtility.DisplayDialog("Could not load file", $"This file could not be found:\n\nAssets/SpeakEasy/Graphs/{graphFileName}\n\nMake sure you're at the right path", "K");

                return;
            }

            SEEditorWindow.UpdateFileName(graphData.FileName);

            LoadGroups(graphData.Groups);
            LoadNodes(graphData.Nodes);
            LoadNodeConnections();

            //for some reason, the nodes' positions aren't updated immediately, so you can't calculate which edges go backwards.
            //thus, this method has to be delayed
            EditorApplication.delayCall += () =>
            {
                graphView.UpdateEdgeColors();
            };
        }

        private static void LoadGroups(List<SEGroupSaveData> groups)
        {
            foreach (SEGroupSaveData groupData in groups)
            {
                SEGroup group = graphView.CreateGroup(groupData.Name, groupData.Position);

                group.ID = groupData.ID;

                loadedGroups.Add(group.ID, group);
            }
        }

        private static void LoadNodes(List<SENodeSaveData> nodes)
        {
            graphView.NameErrors = 0;

            foreach (SENodeSaveData nodeData in nodes)
            {
                List<SEChoiceSaveData> choices = new List<SEChoiceSaveData>();
                List<SEIfSaveData> ifs = new List<SEIfSaveData>();
                List<SECallbackSaveData> callbacks = new List<SECallbackSaveData>();

                if (nodeData.NodeType != SENodeType.Exit)
                {
                    choices = CloneChoices(nodeData.Choices);
                    ifs = CloneIfs(nodeData.IfStatements);
                }

                callbacks = CloneCallbacks(nodeData.Callbacks);

                SENode node = graphView.CreateNode(nodeData.NodeType, nodeData.Position, nodeData.Name, nodeData.IsPlayer, false);

                node.NodeName = nodeData.Name;
                node.ID = nodeData.ID;
                node.Choices = choices;
                node.IfStatements = ifs;
                node.Callbacks = callbacks;
                node.DialogueText = nodeData.Text;
                node.speechTime = nodeData.SpeechTime;

                node.Draw();

                graphView.AddElement(node);

                loadedNodes.Add(node.ID, node);

                if (string.IsNullOrEmpty(nodeData.GroupID))
                {
                    continue;
                }

                SEGroup group = loadedGroups[nodeData.GroupID];

                node.Group = group;

                group.AddElement(node);
            }
        }

        private static void LoadNodeConnections(bool isPaste = false)
        {
            foreach (KeyValuePair<string, SENode> loadedNode in loadedNodes)
            {
                foreach (Port outPort in loadedNode.Value.outputContainer.Children())
                {
                    SEChoiceSaveData choiceData = (SEChoiceSaveData) outPort.userData;

                    if (string.IsNullOrEmpty(choiceData.NodeID) || !loadedNodes.ContainsKey(choiceData.NodeID))
                    {
                        continue;
                    }

                    SENode nextNode = loadedNodes[choiceData.NodeID];

                    Port nextNodeInput = (Port) nextNode.inputContainer.Children().First();

                    Edge newEdge = outPort.ConnectTo(nextNodeInput);

                    graphView.AddElement(newEdge);
                    
                    if (loadedNode.Value.GetPosition().x < newEdge.output.node.GetPosition().x)
                    {
                        newEdge.input.portColor = new Color32(100, 100, 100, 100);
                        newEdge.output.portColor = new Color32(100, 100, 100, 100);
                    }

                    if (isPaste)
                    {
                        graphView.AddToSelection(newEdge);
                    }

                    loadedNode.Value.RefreshPorts();
                }
            }
        }

        #endregion

        #region Creation Methods

        public static void CreatePermanentFolders()
        {
            CreateFolder("Assets/SpeakEasy", "Graphs");
            CreateFolder("Assets", "SpeakEasyData");                        
            CreateFolder("Assets/SpeakEasyData", "Dialogues");              //the folder containing every graph data folder
            CreateFolder("Assets/SpeakEasyData/Dialogues", graphFileName);  //the container folder for the single graph, the container folder
            CreateFolder(containerFolderPath, "Global");                    //total graph data, including filename, lists of groups and nodes, etc
            CreateFolder(containerFolderPath, "Groups");                    //group and grouped node data for each graph
            CreateFolder($"{containerFolderPath}/Global", "Nodes");         //ungrouped node data for each graph
        }

        public static void CreateFolder(string path, string folderName)
        {
            if (AssetDatabase.IsValidFolder($"{path}/{folderName}"))
            {
                return;
            }

            AssetDatabase.CreateFolder(path, folderName);
        }
        
        public static T CreateAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            T asset = LoadAsset<T>(path, assetName);

            if (asset == null)
            {
                asset = ScriptableObject.CreateInstance<T>();
                AssetDatabase.CreateAsset(asset, fullPath);
            }

            return asset;
        }

        public static T LoadAsset<T>(string path, string assetName) where T : ScriptableObject
        {
            string fullPath = $"{path}/{assetName}.asset";

            return AssetDatabase.LoadAssetAtPath<T>(fullPath);
        }

        #endregion
        
        #region Deletion Methods

        public static void RemoveAsset(string path, string assetName)
        {
            AssetDatabase.DeleteAsset($"{path}/{assetName}.asset");
        }

        public static void RemoveFolder(string fullPath)
        {
            FileUtil.DeleteFileOrDirectory($"{fullPath}.meta");
            FileUtil.DeleteFileOrDirectory($"{fullPath}/");
            AssetDatabase.Refresh();
        }
        
        #endregion

        #region Copy & Paste

        //returns the GraphSO that you'll save nodes and groups to
        public static SEGraphSaveDataSO CreateBufferElements()
        {
            CreateFolder("Assets/SpeakEasyData", "CopyPasteBuffer");
            return CreateAsset<SEGraphSaveDataSO>(bufferFolderPath, "BufferGraph");
        }

        public static void Copy(IEnumerable<VisualElement> selection)
        {
                RemoveFolder("Assets/SpeakEasyData/CopyPasteBuffer");
                bufferGraph = SEIOUtility.CreateBufferElements();
                bufferGraph.Initialize("BUFFER");

                List<SENode> nodesToCopy = new List<SENode>();
                List<SEGroup> groupsToCopy = new List<SEGroup>();

                foreach (VisualElement element in selection)
                {
                    if (element is SENode)
                    {
                        SENode reference = (SENode) element;
                        nodesToCopy.Add(reference);
                    }
                    else if (element is SEGroup)
                    {
                        SEGroup reference = (SEGroup) element;
                        groupsToCopy.Add(reference);
                    }
                }

                //copies group with same ID adds _copy
                foreach (SEGroup group in groupsToCopy)
                {
                    SEGroupSaveData groupData = new SEGroupSaveData()
                    {
                        ID = group.ID,
                        Name = group.title + "_copy",
                        Position = group.GetPosition().position
                    };

                    bufferGraph.Groups.Add(groupData);
                }

                //copies node with same data and ID and add _copy
                foreach (SENode node in nodesToCopy)
                {
                    List<SEChoiceSaveData> choices = CloneChoices(node.Choices);
                    List<SEIfSaveData> ifs = CloneIfs(node.IfStatements);
                    List<SECallbackSaveData> callbacks = CloneCallbacks(node.Callbacks);
                    SENodeSaveData nodeData = new SENodeSaveData()
                    {
                        ID = node.ID,
                        Name = node.NodeName + "_copy",
                        Text = node.DialogueText,
                        Choices = choices,
                        IfStatements = ifs,
                        Callbacks = callbacks,
                        NodeType = node.NodeType,
                        Position = node.GetPosition().position + new Vector2(5, 5),
                        IsPlayer = node.IsPlayer
                    };

                    bufferGraph.Nodes.Add(nodeData);
                }
        }

        public static void Paste()
        {
            //Update IDs
            Dictionary<string, string> oldIDNewID = new Dictionary<string, string>();

            foreach (SEGroupSaveData group in bufferGraph.Groups)
            {
                string newID = Guid.NewGuid().ToString();

                oldIDNewID.Add(group.ID, newID);

                group.ID = newID;
            }

            foreach (SENodeSaveData node in bufferGraph.Nodes)
            {
                string newID = Guid.NewGuid().ToString();

                oldIDNewID.Add(node.ID, newID);
                
                node.ID = newID;

                if(node.GroupID != null) 
                {
                    if(oldIDNewID.ContainsKey(node.GroupID)) 
                    {
                        node.GroupID = oldIDNewID[node.GroupID];
                    }
                }
            }

            //updates choice data for each node (must happen after all node IDs are updated)
            foreach (SENodeSaveData nodeData in bufferGraph.Nodes)
            {
                foreach (SEChoiceSaveData choiceData in nodeData.Choices)
                {
                    if (choiceData.NodeID == null)
                    {
                        continue;
                    }

                    if (oldIDNewID.ContainsKey(choiceData.NodeID))
                    {
                        choiceData.NodeID = oldIDNewID[choiceData.NodeID];
                    }
                    else
                    {
                        choiceData.NodeID = null;
                    }
                }
                foreach (SEIfSaveData ifData in nodeData.IfStatements)
                {
                    if (ifData.NodeID == null)
                    {
                        continue;
                    }

                    if (oldIDNewID.ContainsKey(ifData.NodeID))
                    {
                        ifData.NodeID = oldIDNewID[ifData.NodeID];
                    }
                    else
                    {
                        ifData.NodeID = null;
                    }
                }
            }

            if (bufferGraph == null)
            {
                EditorUtility.DisplayDialog("Silly Goose!", "Copy Something First!", "K");
                return;
            }

            graphView.ClearSelection();

            loadedGroups = new Dictionary<string, SEGroup>();
            loadedNodes = new Dictionary<string, SENode>();

            LoadGroups(bufferGraph.Groups);
            LoadNodes(bufferGraph.Nodes);
            LoadNodeConnections(true);

            foreach (SENode node in loadedNodes.Values)
            {
                graphView.AddToSelection(node);
            }
            foreach (SEGroup group in loadedGroups.Values)
            {
                graphView.AddToSelection(group);
            }
        }

        #endregion

        #region Inquiry Methods

        private static void GetElementsFromGraphView()
        {
            graphView.graphElements.ForEach(graphElement =>
            {
                if (graphElement is SENode node)
                {
                    nodes.Add(node);

                    return;
                }

                if (graphElement is SEGroup group)
                {
                    groups.Add(group);

                    return;
                }
            });
        }
    
        #endregion
        }
    }
