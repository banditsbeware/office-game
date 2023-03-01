using System;
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

  //node to be used to begin the dialogue tree from a runtime event (opening a minigame)
  //should be only one per graph
  public class SEConnectorNode : SENode
    {
      
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName);

            NodeName = ID;

            NodeType = SENodeType.Connector;

            SEChoiceSaveData choiceData = new SEChoiceSaveData()
            {
                Text = "out"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            Port inPort = this.CreatePort("", Orientation.Horizontal, Direction.Input, Port.Capacity.Multi);

            Port outPort = this.CreatePort("", Orientation.Horizontal, Direction.Output, Port.Capacity.Single);
            outPort.userData = Choices[0];
            
            inputContainer.Add(inPort);
            outputContainer.Add(outPort);

            titleContainer.RemoveFromHierarchy();

            RefreshExpandedState();
        }
     }
}
