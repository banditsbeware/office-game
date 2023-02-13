using System;
using System.Linq;
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

  public class SENode : Node
    {
        public string ID {get; set;}
        public string DialogueName {get; set;}
        public List<SEChoiceSaveData> Choices {get; set;}
        public string Text {get; set;}
        public SENodeType NodeType {get; set;}
        public SEGroup Group {get; set;}
        protected SEGraphView graphView;
        private Color defaultBackgroundColor;

        public virtual void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName)
        {
            ID = Guid.NewGuid().ToString();
            DialogueName = nodeName;
            Choices = new List<SEChoiceSaveData>();
            Text = "Dialoge text.";
            
            graphView = seGraphView;
            defaultBackgroundColor  = new Color(29f / 255f, 29 / 255f, 30 / 255f);

            SetPosition(new Rect(position, new Vector2(2000, 200)));

            mainContainer.AddToClassList("se-node__main-container");   //style stuff
            extensionContainer.AddToClassList("se-node__extension-container");
        }

        public virtual void Draw()
        {
            // Title Contianer //
            TextField dialogueNameTextField = SEElementUtility.CreateTextField(DialogueName, null, callback =>
            {
                TextField target = (TextField) callback.target;
                target.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(target.value))
                {
                    if (!string.IsNullOrEmpty(DialogueName))
                    {

                        graphView.NameErrors++;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(DialogueName))
                    {
                        graphView.NameErrors--;
                    }
                }

                if (Group == null)
                {
                    graphView.UnlogUngroupedNode(this);

                    DialogueName = target.value; //value of changed text field when finished editing

                    graphView.LogUngroupedNode(this);

                    return;
                }

                SEGroup currentGroup = Group;

                graphView.UnlogGroupedNode(this, Group);
                DialogueName = target.value;
                graphView.LogGroupedNode(this, currentGroup);
            }
            );

            dialogueNameTextField.AddClasses(
                "se-node__text-field",
                "se-node__filename-text-field",
                "se-node__text-field__hidden"
            );

            titleContainer.Insert(0, dialogueNameTextField);  //titleContainer is part of the Node class, it's essentially the header

            // Input Container //
            Port inputPort = this.CreatePort("in", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);  //inputContainer is a child of the middle container in the Node, on the left side

            // Extensions Container //
            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("se-node__custom-data-container");

            Foldout textFoldout = SEElementUtility.CreateFoldout("Dialogue Text");
            TextField textTextField = SEElementUtility.CreateTextArea(Text, null, callback =>
            {
                Text = callback.newValue;
            });

            textTextField.AddClasses(
                "se-node__text-field",
                "se-node__quote-text-field"
            );
            
            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);
        }

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

        public bool IsStartingNode()
        {
            Port inputPort = (Port) inputContainer.Children().First();

            return !inputPort.connected;
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
