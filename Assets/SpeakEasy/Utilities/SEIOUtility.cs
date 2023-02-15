using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace SpeakEasy.Utilities
{
    using Data.Save;
    using ScriptableObjects;
    using Windows;
    using Elements;
    using SpeakEasy.Data;
    using Enumerations;
    

  public static class SEIOUtility
    {
        private static SEGraphView graphView;
        public static string graphFileName;
        private static string containerFolderPath;

        private static List<SEGroup> groups;
        private static List<SENode> nodes;

        private static Dictionary<string, SEGroupSO> createdGroups;
        private static Dictionary<string, SENodeSO> createdNodes;

        private static Dictionary<string, SEGroup> loadedGroups;
        private static Dictionary<string, SENode> loadedNodes;

        public static void Initialize(SEGraphView seGraphView, string graphName)
        {
            graphView = seGraphView;
            graphFileName = graphName;
            containerFolderPath = $"Assets/SpeakEasyData/Dialogues/{graphFileName}";

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

        private static void SaveGroupToGraph(SEGroup group, SEGraphSaveDataSO graphData)
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

        private static void SaveNodeToGraph(SENode node, SEGraphSaveDataSO graphData)
        {
            List<SEChoiceSaveData> choices = CloneChoices(node.Choices);

            SENodeSaveData nodeData = new SENodeSaveData()
            {
                ID = node.ID,
                Name = node.NodeName,
                Choices = choices,
                Text = node.DialogueText,
                GroupID = node.Group?.ID,
                NodeType = node.NodeType,
                Position = node.GetPosition().position,
                IsPlayer = node.IsPlayer
            };

            graphData.Nodes.Add(nodeData);
        }

        private static void SaveNodeToSO(SENode node, SEContainerSO container)
        {
            SENodeSO dialogue;

            if (node.Group != null)
            {
                dialogue = CreateAsset<SENodeSO>($"{containerFolderPath}/Groups/{node.Group.title}/Nodes", node.NodeName);

                container.NodeGroups.AddItem(createdGroups[node.Group.ID], dialogue);
            }
            else
            {
                dialogue = CreateAsset<SENodeSO>($"{containerFolderPath}/Global/Nodes", node.NodeName);

                container.UngroupedNodes.Add(dialogue);
            }

            dialogue.Initialize(node.NodeName, node.DialogueText, ConvertChoiceDataToSaveData(node.Choices), node.NodeType, node.IsPlayer);

            createdNodes.Add(node.ID, dialogue);

            SaveAsset(dialogue);
        }

        private static List<SEChoiceData> ConvertChoiceDataToSaveData(List<SEChoiceSaveData> choices)
        {
            List<SEChoiceData> dialogueChoices = new List<SEChoiceData>();

            foreach (SEChoiceSaveData choice in choices)
            {
                SEChoiceData choiceData = new SEChoiceData()
                {
                    Text = choice.Text
                };

                dialogueChoices.Add(choiceData);
            }

            return dialogueChoices;
        }

        private static void UpdateChoiceConnections()
        {
            foreach (SENode node in nodes)
            {
                SENodeSO dialogue = createdNodes[node.ID];

                for (int choiceIndex = 0; choiceIndex < node.Choices.Count; choiceIndex++)
                {
                    SEChoiceSaveData choice = node.Choices[choiceIndex];

                    if (string.IsNullOrEmpty(choice.NodeID))
                    {
                        continue;
                    }

                    dialogue.Choices[choiceIndex].NextDialogue = createdNodes[choice.NodeID];

                    SaveAsset(dialogue);
                }
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

                if (nodeData.NodeType != SENodeType.Exit)
                {
                    choices = CloneChoices(nodeData.Choices);
                }

                SENode node = graphView.CreateNode(nodeData.NodeType, nodeData.Position, nodeData.Name, nodeData.IsPlayer, false);

                node.ID = nodeData.ID;
                node.Choices = choices;
                node.DialogueText = nodeData.Text;

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

        private static void LoadNodeConnections()
        {
            foreach (KeyValuePair<string, SENode> loadedNode in loadedNodes)
            {
                foreach (Port choicePort in loadedNode.Value.outputContainer.Children())
                {
                    SEChoiceSaveData choiceData = (SEChoiceSaveData) choicePort.userData;

                    if (string.IsNullOrEmpty(choiceData.NodeID))
                    {
                        continue;
                    }

                    SENode nextNode = loadedNodes[choiceData.NodeID];

                    Port nextNodeInput = (Port) nextNode.inputContainer.Children().First();

                    Edge newEdge = choicePort.ConnectTo(nextNodeInput);

                    graphView.AddElement(newEdge);

                    loadedNode.Value.RefreshPorts();
                }
            }
        }

        #endregion

        #region Creation Methods

        private static void CreatePermanentFolders()
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
