using UnityEngine;

namespace SpeakEasy.Elements
{
    using Enumerations;
    using Windows;

    //node to be used to end the dialogue, may be used anywhere on graph
    public class SEExitNode : SELogicNode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName);

            NodeType = SENodeType.Exit;
        }

        public override void Draw()
        {
            base.Draw(); //the single input port is initialized in the base class

            outputContainer.Clear();
        }
    }
}
