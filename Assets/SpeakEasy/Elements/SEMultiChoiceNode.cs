using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using UnityEditor.Experimental.GraphView;

namespace SpeakEasy.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;
  
    //node for choosing from a selection of dialogue choices
    public class SEMultiChoiceNode : SENode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName, isPlayer);

            NodeType = SENodeType.MultiChoice;

            SEChoiceSaveData choiceData = new SEChoiceSaveData()
            {
                Text = "new choice"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            // Main Container (above inputs/outputs) //
            Button addChoiceButton = SEElementUtility.CreateButton("Add Choice", () =>
            {
                SEChoiceSaveData choiceData = new SEChoiceSaveData()
                {
                    Text = "new choice"
                };

                Choices.Add(choiceData);

                Port choicePort = CreateChoicePort(choiceData);

                outputContainer.Add(choicePort);
            });

            Button addDependentButton = SEElementUtility.CreateButton("Add Dependent Choice", () =>
            {
                SEIfSaveData ifData = new SEIfSaveData()
                {
                    Text = "new choice",
                    contextVariableName = "chaos",
                    comparisonSign = "==",
                    comparisonValue = "1",
                    isMetaVariableComparison = false
                };

                IfStatements.Add(ifData);

                Port dependentPort = CreateDependentPort(ifData);

                outputContainer.Add(dependentPort);
            });

            addChoiceButton.AddToClassList("se-node__button");
            addDependentButton.AddToClassList("se-node__button");

            mainContainer.Insert(1, addChoiceButton);
            mainContainer.Insert(1, addDependentButton);
            
            // Output Container //
            foreach(SEChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            }
            foreach(SEIfSaveData ifStatement in IfStatements)
            {
                Port dependentPort = CreateDependentPort(ifStatement);

                outputContainer.Add(dependentPort);
            }

            RefreshExpandedState();
        }

        public Port CreateDependentPort(object userData)
        {
            Port ifPort = this.CreatePort();

            ifPort.userData = userData;  //storing save data about the if statement in the Port's userData

            SEIfSaveData ifData = (SEIfSaveData) userData;

            int contextVariableIndex = Meta.GetVaraibleKeys().IndexOf(ifData.contextVariableName);
            int comparisonIndex = comparisons.IndexOf(ifData.comparisonSign);

            // Port Contents //
            Button deletePortButton = SEElementUtility.CreateButton("x", () =>
            {
                if (Choices.Count == 1)
                {
                    return;
                }

                if (ifPort.connected)
                {
                    graphView.DeleteElements(ifPort.connections);
                }

                IfStatements.Remove(ifData);
                graphView.RemoveElement(ifPort);
            });
            deletePortButton.AddToClassList("se-node__button");

            TextField choiceTextField = SEElementUtility.CreateTextField(ifData.Text, null, callback =>
            {
                ifData.Text = callback.newValue;
            });

            choiceTextField.AddClasses(
                "se-node__text-field",
                "se-node__choice-text-field",
                "se-node__text-field__hidden"
            );

            ifPort.Add(choiceTextField);

            PopupField<string> contextVariables = SEElementUtility.CreatePopupField(Meta.GetVaraibleKeys(), contextVariableIndex);
            contextVariables.RegisterValueChangedCallback(evt => 
            {
                ifData.contextVariableName = evt.newValue;
            });

            PopupField<string> comparisonSigns = SEElementUtility.CreatePopupField(comparisons, comparisonIndex);
            comparisonSigns.RegisterValueChangedCallback(evt => 
            {
                ifData.comparisonSign = evt.newValue;
            });

            if (ifData.isMetaVariableComparison)
            {
                int comparisonVariableIndex = Meta.GetVaraibleKeys().IndexOf(ifData.comparisonValue);
                if (comparisonVariableIndex == -1) comparisonVariableIndex = 0;

                PopupField<string> comparisonVariablePopup = SEElementUtility.CreatePopupField(Meta.GetVaraibleKeys(), comparisonVariableIndex);
                comparisonVariablePopup.RegisterValueChangedCallback(evt => 
                {
                    ifData.comparisonValue = evt.newValue;
                });
                comparisonVariablePopup.contentContainer.AddToClassList("se-node__context-popup-field");

                ifPort.Add(comparisonVariablePopup);
                
            }
            else
            {
                TextField comparisonTextField = SEElementUtility.CreateTextField(ifData.comparisonValue, null, callback =>
                {
                    ifData.comparisonValue = callback.newValue;
                });
                comparisonTextField.AddClasses(
                    "se-node__text-field",
                    "se-node__choice-text-field",
                    "se-node__text-field__hidden"
                );

                ifPort.Add(comparisonTextField);
            }

            contextVariables.contentContainer.AddToClassList("se-node__context-popup-field");
            comparisonSigns.contentContainer.AddToClassList("se-node__symbol-popup-field");

            ifPort.Add(comparisonSigns);
            ifPort.Add(contextVariables);

            ifPort.Add(deletePortButton);

            return ifPort;
        }
    }
}
