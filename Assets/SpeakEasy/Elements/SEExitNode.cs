using UnityEngine;

namespace SpeakEasy.Elements
{
    using Enumerations;
    using Windows;

    //node to be used to end the dialogue, may be used anywhere on graph
    public class SEExitNode : SENode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName);

            NodeName = "_exit";

            NodeType = SENodeType.Exit;
        }

        public override void Draw()
        {
            base.Draw(); //the single input port is initialized in the base class

            outputContainer.Clear();
        }
    }
}
