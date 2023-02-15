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

  public class SEEntryNode : SENode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName);

            NodeName = "_entry";

            NodeType = SENodeType.Entry;

            SEChoiceSaveData choiceData = new SEChoiceSaveData()
            {
                Text = "First Node"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            // Input Container //
            inputContainer.Clear();

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
