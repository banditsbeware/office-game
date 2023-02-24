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
    public class SELogicNode : SENode
    {
        List<string> comparisons = new List<string>(){"==", "!=", "<", ">", "<=", ">="};

        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName, isPlayer);
        }

        public override void Draw()
        {
            base.Draw();

            extensionContainer.Add(CreateCallbackFoldout());

            RefreshExpandedState();
        }

        #region Element Creation
        public Port CreateLogicPort(object userData)
        {
            Port choicePort = this.CreatePort();

            SEIfSaveData ifData = new SEIfSaveData();
            
            ifData = (SEIfSaveData) userData;
            choicePort.userData = ifData;  //storing save data about the if statement in the Port's userData

            int contextVariableIndex = meta.GetVaraibleKeys().IndexOf(ifData.contextVariableName);
            int comparisonIndex = comparisons.IndexOf(ifData.comparisonSign);

            // Port Contents //

            PopupField<string> contextVariables = SEElementUtility.CreatePopupField(meta.GetVaraibleKeys(), contextVariableIndex);
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
