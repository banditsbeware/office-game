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
  public class SEIfElseIfNode : SELogicNode
    {

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

            NodeType = SENodeType.IfElseIf;

            IfStatements.Add(ifData);
            IfStatements.Add(elseData);
        }

        public override void Draw()
        {
            base.Draw();

            Port elsePort = this.CreatePort("else:");

            elsePort.userData = IfStatements[IfStatements.Count - 1];
            
            // Output Container //
            foreach(SEIfSaveData ifData in IfStatements)
            {
                Port ifPort = CreateElseIfPort(ifData);

                outputContainer.Add(ifPort);
            }

            outputContainer.RemoveAt(IfStatements.Count - 1);
            
            outputContainer.Add(elsePort);

            //Main Container
            //one button instantiates an if data with isMetaVariableComparison as true
            Button insertVariableCompareButton = SEElementUtility.CreateButton("Add Variable Comparison", () =>
            {
                int statementNumber = IfStatements.Count - 1;

                SEIfSaveData ifData = new SEIfSaveData()
                {
                    contextVariableName = "chaos",
                    comparisonSign = "==",
                    comparisonValue = "1",
                    isMetaVariableComparison = true
                };

                IfStatements.Insert(statementNumber, ifData);

                Port choicePort = CreateElseIfPort(ifData);

                outputContainer.Insert(statementNumber, choicePort);
            });
            Button insertValueCompareButton = SEElementUtility.CreateButton("Add Value Comparison", () =>
            {
                int statementNumber = IfStatements.Count - 1;

                SEIfSaveData ifData = new SEIfSaveData()
                {
                    contextVariableName = "chaos",
                    comparisonSign = "==",
                    comparisonValue = "1",
                    isMetaVariableComparison = false
                };

                IfStatements.Insert(statementNumber, ifData);

                Port choicePort = CreateElseIfPort(ifData);

                outputContainer.Insert(statementNumber, choicePort);
            });

            insertVariableCompareButton.AddToClassList("se-node__button");
            insertValueCompareButton.AddToClassList("se-node__button");

            mainContainer.Insert(1, insertVariableCompareButton);
            mainContainer.Insert(2, insertValueCompareButton);
        
            RefreshExpandedState();
        }
    }
}
