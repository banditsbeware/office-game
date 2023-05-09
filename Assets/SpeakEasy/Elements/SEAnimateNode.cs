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

    //for this node to work, the animator must be placed on the parent of the dialogue object
    public class SEAnimateNode : SENode
    {

        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            DialogueText = "trigger name";

            base.Initialize(seGraphView, position, nodeName, isPlayer);

            SEChoiceSaveData choiceData = new SEChoiceSaveData()
            {
                Text = "Next Node"
            };
            Choices.Add(choiceData);

            NodeType = SENodeType.Animate;
        }

        public override void Draw()
        {
            // Base Draw
            base.Draw();

            // Title Container //
            Button addTimeButton = SEElementUtility.CreateButton("T", () =>
            {
                extensionContainer.Add(CreateSpeechTimeFoldout());
                RefreshExpandedState();
            });
            addTimeButton.AddToClassList("se-node__mini-button");

            titleContainer[1].Add(addTimeButton);  //adds to button container

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
