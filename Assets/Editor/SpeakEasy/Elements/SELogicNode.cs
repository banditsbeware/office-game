using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

using UnityEngine;
using UnityEngine.UIElements;


namespace SpeakEasy.Elements
{
    using Data.Save;
    using Enumerations;
    using Windows;
    using Utilities;

    //node used for testing values in meta. changes dialogue based on game state
    public class SELogicNode : SENode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName, isPlayer);
        }

        #region Element Creation
        public Port CreateLogicPort(object userData)
        {
            Port choicePort = this.CreatePort();

            SEIfSaveData ifData = new SEIfSaveData();
            
            ifData = (SEIfSaveData) userData;
            choicePort.userData = ifData;  //storing save data about the if statement in the Port's userData

            int contextVariableIndex = Meta.GetVaraibleKeys().IndexOf(ifData.contextVariableName);
            int comparisonIndex = comparisons.IndexOf(ifData.comparisonSign);

            // Port Contents //

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

                choicePort.Add(comparisonVariablePopup);
                
            }
            else
            {
                TextField choiceTextField = SEElementUtility.CreateTextField(ifData.comparisonValue, null, callback =>
                {
                    ifData.comparisonValue = callback.newValue;
                });
                choiceTextField.AddClasses(
                    "se-node__text-field",
                    "se-node__choice-text-field",
                    "se-node__text-field__hidden"
                );

                choicePort.Add(choiceTextField);
            }

            

            contextVariables.contentContainer.AddToClassList("se-node__context-popup-field");
            comparisonSigns.contentContainer.AddToClassList("se-node__symbol-popup-field");

            choicePort.Add(comparisonSigns);
            choicePort.Add(contextVariables);

            return choicePort;
        }

        public Port CreateElseIfPort(object userData)
        {
            Port choicePort = this.CreatePort();

            SEIfSaveData ifData = new SEIfSaveData();
            
            ifData = (SEIfSaveData) userData;
            choicePort.userData = ifData;

            int contextVariableIndex = Meta.GetVaraibleKeys().IndexOf(ifData.contextVariableName);
            int comparisonIndex = comparisons.IndexOf(ifData.comparisonSign);

            // Port Contents //

            Button deletePortButton = SEElementUtility.CreateButton("x", () =>
            {
                if (IfStatements.Count == 2)
                {
                    return;
                }

                if (choicePort.connected)
                {
                    graphView.DeleteElements(choicePort.connections);
                }

                IfStatements.Remove(ifData);
                graphView.RemoveElement(choicePort);
            });

            deletePortButton.AddToClassList("se-node__button");

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
                int comparisonVariableIndex = Meta.GetVaraibleKeys().IndexOf(IfStatements[0].comparisonValue);
                if (comparisonVariableIndex == -1) comparisonVariableIndex = 0;

                PopupField<string> comparisonVariablePopup = SEElementUtility.CreatePopupField(Meta.GetVaraibleKeys(), comparisonVariableIndex);
                comparisonVariablePopup.RegisterValueChangedCallback(evt => 
                {
                    ifData.comparisonValue = evt.newValue;
                });
                comparisonVariablePopup.contentContainer.AddToClassList("se-node__context-popup-field");

                choicePort.Add(comparisonVariablePopup);
                
            }
            else
            {
                TextField choiceTextField = SEElementUtility.CreateTextField(ifData.comparisonValue, null, callback =>
                {
                    ifData.comparisonValue = callback.newValue;
                });
                choiceTextField.AddClasses(
                    "se-node__text-field",
                    "se-node__choice-text-field",
                    "se-node__text-field__hidden"
                );

                choicePort.Add(choiceTextField);
            }

            contextVariables.contentContainer.AddToClassList("se-node__context-popup-field");
            comparisonSigns.contentContainer.AddToClassList("se-node__symbol-popup-field");

            choicePort.Add(comparisonSigns);
            choicePort.Add(contextVariables);
            choicePort.Add(deletePortButton);

            return choicePort;
        }

        
        #endregion
    }
}
