using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpeakEasy.Elements
{
    using Enumerations;
    using Utilities;
    using Windows;
    using Data.Save;

    //base class for all nodes on graph
     public class SENode : Node
    {
        public string ID {get; set;}  //unique ID
        public string NodeName {get; set;}   //node title, used for playing audio/identifying
        public string DialogueText {get; set;}    //text to be shown on screen, dialogue contents

        public bool IsPlayer;   //dialogue in node is spoken by player

        public List<SEChoiceSaveData> Choices {get; set;}   //list of choice output ports. Used by SingleChoice and MultiChoice (and technically entry node)
        public List<SEIfSaveData> IfStatements {get; set;}   //list of logic output ports. Used by IfNode
        public SENodeType NodeType {get; set;}   //See Enumerations.SENodeType
        public SEGroup Group {get; set;}   //group the node is inside
        protected SEGraphView graphView;   //reference to the graph
        private Color defaultBackgroundColor;

        public virtual void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            ID = Guid.NewGuid().ToString();
            NodeName = nodeName;
            Choices = new List<SEChoiceSaveData>();
            IfStatements = new List<SEIfSaveData>();
            
            graphView = seGraphView;
            defaultBackgroundColor  = new Color(29f / 255f, 29 / 255f, 30 / 255f);

            SetPosition(new Rect(position, new Vector2(2000, 200)));

            mainContainer.AddToClassList("se-node__main-container");   //style stuff
            extensionContainer.AddToClassList("se-node__extension-container");
        }

        public virtual void Draw()
        {
            // Title Contianer //
            TextField nodeNameTextField = SEElementUtility.CreateTextField(NodeName, null, callback =>
            {
                TextField target = (TextField) callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                //adds error to graph if, and only if, you delete the last character from the node title
                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(NodeName))
                    {

                        graphView.NameErrors++;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(NodeName))
                    {
                        graphView.NameErrors--;
                    }
                }

                //NodeName has to be taken out of graphview, then updated, then put back in, so that old versions of nodes aren't left in the lists in graphView
                if (Group == null)
                {
                    graphView.UnlogUngroupedNode(this);

                    NodeName = target.value; //value of changed text field when finished editing

                    graphView.LogUngroupedNode(this);

                    return;
                }

                SEGroup currentGroup = Group;

                graphView.UnlogGroupedNode(this, Group);
                NodeName = target.value;
                graphView.LogGroupedNode(this, currentGroup);
            }
            );

            nodeNameTextField.AddClasses(
                "se-node__text-field",
                "se-node__filename-text-field",
                "se-node__text-field__hidden"
            );

            titleContainer.Insert(0, nodeNameTextField);  //titleContainer is part of the inherited Node class, it's essentially the header

            // Input Container //
            Port inputPort = this.CreatePort("in", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);  //inputContainer is a child of the middle container in the Node, on the left side
        }

        //menu that appears when you right click on the graph
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Inputs", actionEvent => DisconnectPorts(inputContainer));

            base.BuildContextualMenu(evt);   
        }

        #region Utilities
        public void DisconnectAllPorts()
        {
            DisconnectPorts(inputContainer);
            DisconnectPorts(outputContainer);
        }

        private void DisconnectPorts(VisualElement container)
        {
            foreach (Port port in container.Children())
            {
                if (!port.connected)
                {
                    continue;
                }

                graphView.DeleteElements(port.connections);
            }
        }

        public void SetErrorColor(Color color)
        {
            mainContainer.style.backgroundColor = color;
        }

        public void ResetColor()
        {
            mainContainer.style.backgroundColor = defaultBackgroundColor;
        }
        #endregion
    }
}
