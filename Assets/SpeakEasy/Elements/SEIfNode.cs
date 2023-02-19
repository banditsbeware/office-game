using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;


namespace SpeakEasy.Elements
{
    using Data.Save;
    using Enumerations;
    using Windows;
    using Utilities;

    //node used for testing values in meta. changes dialogue based on game state
    public class SEIfNode : SENode
    {
        List<string> comparisons = new List<string>(){"==", "!=", "<", ">", "<=", ">="};

        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName, isPlayer);

            SEIfSaveData ifData = new SEIfSaveData()
            {
                contextVariableName = "chaos",
                comparisonSign = "==",
                comparisonValue = "1"
            };

            SEIfSaveData elseData = new SEIfSaveData();

            NodeType = SENodeType.If;

            IfStatements.Add(ifData);
            IfStatements.Add(elseData);
        }

        public override void Draw()
        {
            base.Draw();
            
            // Output Container //

                Port ifPort = this.CreateLogicPort(IfStatements[0]);
                
                outputContainer.Add(ifPort);

                Port elsePort = this.CreatePort("else:");

                elsePort.userData = IfStatements[1];
                
                outputContainer.Add(elsePort);
        

            RefreshExpandedState();
        }

        #region Element Creation
        private Port CreateLogicPort(object userData)
        {
            Port choicePort = this.CreatePort();

            SEIfSaveData ifData = new SEIfSaveData();
            
            ifData = (SEIfSaveData) userData;
            choicePort.userData = ifData;  //storing save data about the if statement in the Port's userData

            int contextVariableIndex = meta.getAllVariableNames().IndexOf(ifData.contextVariableName);
            int comparisonIndex = comparisons.IndexOf(ifData.comparisonSign);

            // Port Contents //

            PopupField<string> contextVariables = SEElementUtility.CreatePopupField(meta.getAllVariableNames(), contextVariableIndex);
            contextVariables.RegisterValueChangedCallback(evt => 
            {
                ifData.contextVariableName = evt.newValue;
            });

            PopupField<string> comparisonSigns = SEElementUtility.CreatePopupField(comparisons, comparisonIndex);
            comparisonSigns.RegisterValueChangedCallback(evt => 
            {
                ifData.comparisonSign = evt.newValue;
            });

            TextField choiceTextField = SEElementUtility.CreateTextField(ifData.comparisonValue, null, callback =>
            {
                ifData.comparisonValue = callback.newValue;
            });

            contextVariables.contentContainer.AddToClassList("se-node__context-popup-field");
            comparisonSigns.contentContainer.AddToClassList("se-node__symbol-popup-field");
            choiceTextField.AddClasses(
                "se-node__text-field",
                "se-node__choice-text-field",
                "se-node__text-field__hidden"
            );

            choicePort.Add(choiceTextField);
            choicePort.Add(comparisonSigns);
            choicePort.Add(contextVariables);

            return choicePort;
        }
        #endregion
    }
}
