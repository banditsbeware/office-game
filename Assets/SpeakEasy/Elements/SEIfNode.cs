using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using System.Linq;


namespace SpeakEasy.Elements
{
    using Data.Save;
    using Enumerations;
    using Windows;
    using Utilities;

  //node used for testing values in meta. changes dialogue based on game state
  public class SEIfNode : SELogicNode
    {
        List<string> comparisons = new List<string>(){"==", "!=", "<", ">", "<=", ">="};

        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName, isPlayer);

            SEIfSaveData ifData = new SEIfSaveData()
            {
                contextVariableName = "chaos",
                comparisonSign = "==",
                comparisonValue = "1",
                isMetaVariableComparison = false
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

            //Main Container
            //button toggles whether you're comparing to a user-defined value or a second variable
            Button toggleValueButton = SEElementUtility.CreateButton("Toggle Value Field", () =>
            {
                IfStatements[0].isMetaVariableComparison = !IfStatements[0].isMetaVariableComparison;
                
                ifPort.Remove(ifPort.Children().ElementAt(2));

                if (IfStatements[0].isMetaVariableComparison)
                {
                    //index in list of variables
                    //if the text field contents doesn't exist in the list of variables, default to first in list
                    int comparisonVariableIndex = Meta.GetVaraibleKeys().IndexOf(IfStatements[0].comparisonValue);
                    if (comparisonVariableIndex == -1) comparisonVariableIndex = 0;

                    PopupField<string> comparisonVariablePopup = SEElementUtility.CreatePopupField(Meta.GetVaraibleKeys(), comparisonVariableIndex);
                    comparisonVariablePopup.RegisterValueChangedCallback(evt => 
                    {
                        IfStatements[0].comparisonValue = evt.newValue;
                    });
                    comparisonVariablePopup.contentContainer.AddToClassList("se-node__context-popup-field");

                    ifPort.Insert(2, comparisonVariablePopup);
                    
                }
                else
                {
                    TextField choiceTextField = SEElementUtility.CreateTextField(IfStatements[0].comparisonValue, null, callback =>
                    {
                        IfStatements[0].comparisonValue = callback.newValue;
                    });
                    choiceTextField.AddClasses(
                        "se-node__text-field",
                        "se-node__choice-text-field",
                        "se-node__text-field__hidden"
                    );

                    ifPort.Insert(2, choiceTextField);
                }
            });
            toggleValueButton.AddToClassList("se-node__button");

            mainContainer.Insert(1, toggleValueButton);
        
            RefreshExpandedState();
        }
    }
}
