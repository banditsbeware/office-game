using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

namespace SpeakEasy.Elements
{
    using Enumerations;
    using Utilities;
    using Windows;
    using Data.Save;
  
  //base class for all nodes on graph
    public class SENode : Node
    {
        private static List<string> callbackActions = new List<string>(){"SetValue", "Increment", "Decrement"};   //used to draw callbacks

        public string ID {get; set;}  //unique ID
        public string NodeName {get; set;}   //node title, used for playing audio/identifying
        public string DialogueText {get; set;}    //text to be shown on screen, dialogue contents

        public float speechTime {get; set;}

        public bool IsPlayer;   //dialogue in node is spoken by player

        public List<SEChoiceSaveData> Choices {get; set;}   //list of choice output ports. Used by SingleChoice and MultiChoice (and technically entry node)
        public List<SEIfSaveData> IfStatements {get; set;}   //list of logic output ports. Used by IfNode
        public List<SECallbackSaveData> Callbacks {get; set;}   //list of callbacks on node
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
            Callbacks = new List<SECallbackSaveData>();
            speechTime = 0f;
            
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

            VisualElement callbackFoldout = new VisualElement();

            Button addCallbackButton = SEElementUtility.CreateButton("CB", () =>
            {
                if (Callbacks.Count == 0)
                {
                    callbackFoldout = CreateCallbackFoldout();

                    extensionContainer.Add(callbackFoldout);
                    RefreshExpandedState();
                }

                SECallbackSaveData callbackData = new SECallbackSaveData()
                {
                    callbackVariableName = "chaos",
                    callbackAction = "SetValue",
                    callbackValue = "0"
                };

                Callbacks.Add(callbackData);

                VisualElement callbackElement = CreateCallback(callbackData, callbackFoldout.contentContainer);

                callbackFoldout.Add(callbackElement);

                RefreshExpandedState();
                
            });
            addCallbackButton.AddToClassList("se-node__mini-button");

            Button addDialoguesButton = SEElementUtility.CreateButton("D", () =>
            {
                extensionContainer.Add(CreateDialogueFoldout());
                RefreshExpandedState();
            });
            addDialoguesButton.AddToClassList("se-node__mini-button");

            VisualElement buttonsContainer = new VisualElement();
            buttonsContainer.Add(addCallbackButton);
            buttonsContainer.Add(addDialoguesButton);
            buttonsContainer.AddToClassList("se-node__callback");

            titleContainer.Insert(1, buttonsContainer);
            titleContainer.Insert(0, nodeNameTextField);  //titleContainer is part of the inherited Node class, it's essentially the header

            // Input Container //
            Port inputPort = this.CreatePort("in", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);
            inputContainer.Add(inputPort);  //inputContainer is a child of the middle container in the Node, on the left side

            // Extensions Container //
            if (Callbacks.Count != 0)
            {
                callbackFoldout = CreateCallbackFoldout();
                extensionContainer.Add(callbackFoldout);
            }
            if (DialogueText != null && DialogueText != "")
            {
                extensionContainer.Add(CreateDialogueFoldout());
            }
            if (speechTime != 0)
            {
                extensionContainer.Add(CreateSpeechTimeFoldout());
            }
            RefreshExpandedState();
        }

        #region Elements

        #region Callbacks
        public Foldout CreateCallbackFoldout()
        {
            Foldout callbackFoldout = SEElementUtility.CreateFoldout("Callback");

            callbackFoldout.RegisterValueChangedCallback((evt) => this.BringToFront());

            // Contents //

            foreach(SECallbackSaveData callbackData in Callbacks)
            {
                callbackFoldout.Add(CreateCallback(callbackData, callbackFoldout.contentContainer));

                RefreshExpandedState();
            }

            callbackFoldout.value = false;
            
            callbackFoldout.Add(callbackFoldout);
            
            callbackFoldout.AddToClassList("se-node__custom-data-container");

            return callbackFoldout;
        }

        private VisualElement CreateCallback(SECallbackSaveData callbackData, VisualElement container = null) 
        {
            int callbackVariableIndex = Meta.GetVaraibleKeys().IndexOf(callbackData.callbackVariableName);
            int callbackActionIndex = callbackActions.IndexOf(callbackData.callbackAction);

            VisualElement callback = new VisualElement();

            Button deleteCallbackButton = SEElementUtility.CreateButton("x", () =>
            {
                Callbacks.Remove(callbackData);

                container.Remove(callback);

                if (Callbacks.Count == 0)
                {
                    extensionContainer.Remove(container.parent);
                }
            });

            PopupField<string> changeVariables = SEElementUtility.CreatePopupField(Meta.GetVaraibleKeys(), callbackVariableIndex);
            changeVariables.RegisterValueChangedCallback(evt => 
            {
                callbackData.callbackVariableName = evt.newValue;
            });

            changeVariables.RegisterValueChangedCallback(evt => 
            {
                callbackData.callbackVariableName = evt.newValue;
            });

            PopupField<string> changeAction = SEElementUtility.CreatePopupField(callbackActions, callbackActionIndex);
            changeAction.RegisterValueChangedCallback (evt => 
            {
                callbackData.callbackAction =  evt.newValue;
            });

            TextField changeValue = SEElementUtility.CreateTextField(callbackData.callbackValue, null, callback =>
            {
                callbackData.callbackValue = callback.newValue;
            });

            deleteCallbackButton.AddToClassList("se-node__button");
            changeVariables.AddToClassList("se-node__context-popup-field");
            changeAction.AddToClassList("se-node__action-popup-field");
            changeValue.AddClasses(
                "se-node__text-field",
                "se-node__choice-text-field",
                "se-node__text-field__hidden"
            );
            callback.AddToClassList("se-node__callback");

            callback.Add(deleteCallbackButton);
            callback.Add(changeVariables);
            callback.Add(changeAction);
            callback.Add(changeValue);

            return callback;
        }

        #endregion

        public Port CreateChoicePort(object userData)
        {
            Port choicePort = this.CreatePort();

            choicePort.userData = userData;

            SEChoiceSaveData choiceData = (SEChoiceSaveData) userData;

            Button deletePortButton = SEElementUtility.CreateButton("x", () =>
            {
                if (Choices.Count == 1)
                {
                    return;
                }

                if (choicePort.connected)
                {
                    graphView.DeleteElements(choicePort.connections);
                }

                Choices.Remove(choiceData);
                graphView.RemoveElement(choicePort);
            });

            deletePortButton.AddToClassList("se-node__button");
            
            TextField choiceTextField = SEElementUtility.CreateTextField(choiceData.Text, null, callback =>
            {
                choiceData.Text = callback.newValue;
            });

            choiceTextField.AddClasses(
                "se-node__text-field",
                "se-node__choice-text-field",
                "se-node__text-field__hidden"
            );

            choicePort.Add(choiceTextField);
            choicePort.Add(deletePortButton);

            return choicePort;
        }

        public Foldout CreateDialogueFoldout()
        {
            Foldout textFoldout = SEElementUtility.CreateFoldout("Dialogue Text");

            textFoldout.AddToClassList("se-node__custom-data-container");
            textFoldout.RegisterValueChangedCallback((evt) => this.BringToFront());

            TextField textTextField = SEElementUtility.CreateTextArea(DialogueText, null, callback => DialogueText = callback.newValue);

            textTextField.AddClasses(
                "se-node__text-field",
                "se-node__quote-text-field"
            );

            textFoldout.Add(textTextField);

            textFoldout.value = false;

            return textFoldout;
        }

        public Foldout CreateSpeechTimeFoldout()
        {
            Foldout timeFoldout = SEElementUtility.CreateFoldout("Custom Speech Time");

            timeFoldout.AddToClassList("se-node__custom-data-container");
            timeFoldout.RegisterValueChangedCallback((evt) => this.BringToFront());

            TextField timeTextField = SEElementUtility.CreateTextArea(speechTime.ToString(), null, callback => speechTime = float.Parse(callback.newValue));

            timeTextField.AddClasses(
                "se-node__text-field",
                "se-node__quote-text-field"
            );

            timeFoldout.Add(timeTextField);

            timeFoldout.value = false;

            return timeFoldout;
        }
        #endregion

        #region Utilities

        //menu that appears when you right click on the graph
        public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
        {
            evt.menu.AppendAction("Disconnect Inputs", actionEvent => DisconnectPorts(inputContainer));

            base.BuildContextualMenu(evt);   
        }

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
