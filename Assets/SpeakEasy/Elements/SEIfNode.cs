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
    }
}
