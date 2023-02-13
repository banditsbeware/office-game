using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace SpeakEasy.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;

    public class SESingleChoiceNode : SENode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName)
        {
            base.Initialize(seGraphView, position, nodeName);

            SEChoiceSaveData choiceData = new SEChoiceSaveData()
            {
                Text = "Next Dialogue"
            };

            NodeType = SENodeType.SingleChoice;

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();
            
            // Output Container //
            foreach(SEChoiceSaveData choice in Choices)
            {
                Port outPort = this.CreatePort(choice.Text);

                outPort.userData = choice;
                
                outputContainer.Add(outPort);
            }

            RefreshExpandedState();
        }
    }
}
