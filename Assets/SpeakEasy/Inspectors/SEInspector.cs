using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace SpeakEasy.Inspectors
{
    using SpeakEasy.ScriptableObjects;
    using Utilities;

    //creates custom inspector for showing SEDialogue on GameObject
    [CustomEditor(typeof(SEDialogue))]
    public class SEInspector : Editor
    {
        //Dialogue Scriptable Objects
        private SerializedProperty containerProperty;
        private SerializedProperty entryNodeProperty;

        //Filters
        private SerializedProperty startingNodesOnlyProperty;

        //Index
        private SerializedProperty selectedNodeIndexProperty;

        //Objects
        private SerializedProperty playerSpeechBubbleProperty;
        private SerializedProperty playerSpeechTextProperty;
        private SerializedProperty npcSpeechBubbleProperty;
        private SerializedProperty npcSpeechTextProperty;
        private SerializedProperty npcAnimatorProperty;

        private SerializedProperty choiceButtonsProperty;

        private void OnEnable() 
        {
            containerProperty = serializedObject.FindProperty("container");
            entryNodeProperty = serializedObject.FindProperty("entryNode");

            startingNodesOnlyProperty = serializedObject.FindProperty("startingNodesOnly");
            selectedNodeIndexProperty = serializedObject.FindProperty("selectedNodeIndex");

            playerSpeechBubbleProperty = serializedObject.FindProperty("playerSpeechBubble");
            npcSpeechBubbleProperty = serializedObject.FindProperty("npcSpeechBubble");
            npcAnimatorProperty = serializedObject.FindProperty("npcAnimator");

            choiceButtonsProperty = serializedObject.FindProperty("choiceButtons");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            DrawDialogueContainerArea();

            SEContainerSO container = (SEContainerSO) containerProperty.objectReferenceValue;

            if (container == null)
            {
                StopDrawing("Select a Container to move on.");
                return;
            }

            DrawFiltersArea();

            bool currentStartingNodesOnlyFilter = startingNodesOnlyProperty.boolValue;

            Dictionary<string, string> nodeNames;

            string nodeFolderPath = $"Assets/SpeakEasyData/Dialogues/{container.FileName}";
            string nodeInfoMessage;

            nodeNames = container.GetNodeNamesAndPaths(currentStartingNodesOnlyFilter);

            nodeInfoMessage = "There are no" + (currentStartingNodesOnlyFilter ? " Starting" : "") + " Nodes in this Container.";

            if (nodeNames.Count == 0)
            {
                StopDrawing(nodeInfoMessage);

                return;
            }

            DrawNodeArea(nodeNames, nodeFolderPath);

            DrawObjectsArea();

            serializedObject.ApplyModifiedProperties();
        }

    

        #region Draw Methods

        private void DrawDialogueContainerArea()
        {
            SEInspectorUtility.DrawHeader("Dialogue Container");

            containerProperty.DrawPropertyField();

            SEInspectorUtility.DrawSpace();
        }

        private void DrawFiltersArea()
        {
            SEInspectorUtility.DrawHeader("Filters");

            startingNodesOnlyProperty.DrawPropertyField();

            SEInspectorUtility.DrawSpace();
        }

        // private void DrawGroupArea(SEContainerSO container, List<string> groupNames)
        // {
        //     SEInspectorUtility.DrawHeader("Group");

        //     int oldSelectedGroupIndex = selectedGroupIndexProperty.intValue;
        //     SEGroupSO oldGroup = (SEGroupSO) groupProperty.objectReferenceValue;

        //     bool isOldGroupNull = oldGroup == null;
        //     string oldGroupName = isOldGroupNull ? "" : oldGroup.GroupName;
        //     UpdateIndexOnListUpdate(groupNames, selectedGroupIndexProperty, oldSelectedGroupIndex, oldGroupName, isOldGroupNull);

        //     selectedGroupIndexProperty.intValue = SEInspectorUtility.DrawPopup("Group", selectedGroupIndexProperty, groupNames.ToArray());

        //     string selectedGroupName = groupNames[selectedGroupIndexProperty.intValue];
        //     SEGroupSO selectedGroup = SEIOUtility.LoadAsset<SEGroupSO>($"Assets/SpeakEasyData/Dialogues/{container.FileName}/Groups/{selectedGroupName}", selectedGroupName);

        //     groupProperty.objectReferenceValue = selectedGroup;
        //     SEInspectorUtility.DrawDisabledFields(() => groupProperty.DrawPropertyField());

        //     SEInspectorUtility.DrawSpace();
        // }

        private void DrawNodeArea(Dictionary<string, string> nodeNamesAndPaths, string nodeFolderPath)
        {
            SEInspectorUtility.DrawHeader("Entry Node");

            List<string> nodeNames = nodeNamesAndPaths.Keys.ToList();

            int oldSelectedNodeIndex = selectedNodeIndexProperty.intValue;
            SENodeSO oldNode = (SENodeSO) entryNodeProperty.objectReferenceValue;

            bool isOldDialogueNull = oldNode == null;
            string oldNodeName = isOldDialogueNull ? "" : oldNode.NodeName;

            UpdateIndexOnListUpdate(nodeNames, selectedNodeIndexProperty, oldSelectedNodeIndex, oldNodeName, isOldDialogueNull);

            selectedNodeIndexProperty.intValue = SEInspectorUtility.DrawPopup("Node", selectedNodeIndexProperty, nodeNames.ToArray());

            string selectedNodeName = nodeNames[selectedNodeIndexProperty.intValue];

            SENodeSO selectedNode = SEIOUtility.LoadAsset<SENodeSO>($"{nodeFolderPath}/{nodeNamesAndPaths[selectedNodeName]}/", selectedNodeName);

            entryNodeProperty.objectReferenceValue = selectedNode;

            SEInspectorUtility.DrawDisabledFields(() => entryNodeProperty.DrawPropertyField());

            SEInspectorUtility.DrawSpace();
        }

        private void DrawObjectsArea()
        {
            SEInspectorUtility.DrawHeader("Objects");

            playerSpeechBubbleProperty.DrawPropertyField();
            npcSpeechBubbleProperty.DrawPropertyField();
            npcAnimatorProperty.DrawPropertyField();

            SEInspectorUtility.DrawSpace();

            choiceButtonsProperty.DrawPropertyField();
        }

        private void StopDrawing(string reason, MessageType messageType = MessageType.Info)
        {
            SEInspectorUtility.DrawHelpBox(reason, messageType);

            SEInspectorUtility.DrawSpace();

            SEInspectorUtility.DrawHelpBox("Select a Dialogue for this component to work properly at runtime!", MessageType.Warning);

            serializedObject.ApplyModifiedProperties();
        }

        #endregion

        #region Indexing Methods

        private void UpdateIndexOnListUpdate(List<string> optionNames, SerializedProperty indexProperty, int oldSelectedPropertyIndex, string oldPropertyName, bool isOldPropertyNull)
        {
            if (isOldPropertyNull)
            {
                indexProperty.intValue = 0;

                return;
            }


            bool oldIndexIsOutOfBounds = oldSelectedPropertyIndex > optionNames.Count - 1;
            bool oldNameIsDifferent = oldIndexIsOutOfBounds || oldPropertyName != optionNames[oldSelectedPropertyIndex];

            if (oldNameIsDifferent)
            {
                if (optionNames.Contains(oldPropertyName))
                {
                    indexProperty.intValue = optionNames.IndexOf(oldPropertyName);

                    return;
                }
                
                indexProperty.intValue = 0;


            }
        }

        #endregion
    }
}
