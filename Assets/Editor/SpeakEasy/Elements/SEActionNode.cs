using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;


namespace SpeakEasy.Elements
{
    using Data.Save;
    using Enumerations;
    using Windows;
    using Utilities;

    //used to call a function using Monobehavior.Invoke()
    //waits for a specific action (within the perscribed function) to continue to the next dialogue
    //for this node to work, the function Invoked must end with SEDialogue.BeginNode()
    public class SEActionNode : SENode
    {

        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            DialogueText = "function to invoke";

            base.Initialize(seGraphView, position, nodeName, isPlayer);

            SEChoiceSaveData choiceData = new SEChoiceSaveData()
            {
                Text = "Next Node"
            };
            Choices.Add(choiceData);

            NodeType = SENodeType.Action;
        }

        public override void Draw()
        {
            // Base Draw
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

        #region Element Creation
        #endregion
    }
}
