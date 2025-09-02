using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;


namespace SpeakEasy.Elements
{
    using Utilities;
    using Windows;
    using Data.Save;
    using Enumerations;


  //class parent to SingleChoice and MultiChoice nodes. Add the Dialogue Text dropdown underneath the node and the isPlayer Toggle
  public class SESpeakingNode : SENode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            DialogueText = "Dialoge text.";

            base.Initialize(seGraphView, position, nodeName, isPlayer);

            SEChoiceSaveData choiceData = new SEChoiceSaveData()
            {
                Text = "Next Node"
            };

            NodeType = SENodeType.Speaking;

            Choices.Add(choiceData);

            IsPlayer = isPlayer;
        }

        public override void Draw()
        {
            // Base Draw
            base.Draw();

            // Title Container //
            Label toggleLabel = new Label("isPlayer:");

            toggleLabel.AddClasses("se-node__label");

            Toggle isPlayerToggle = SEElementUtility.CreateToggle(IsPlayer, callback => 
            {
                IsPlayer = callback.newValue;
            });

            Button addTimeButton = SEElementUtility.CreateButton("T", () =>
            {
                extensionContainer.Add(CreateSpeechTimeFoldout());
                RefreshExpandedState();
            });
            addTimeButton.AddToClassList("se-node__mini-button");

            isPlayerToggle.AddClasses("se-node");

            titleContainer[1].Add(addTimeButton);  //adds to button container
            titleContainer.Insert(1, toggleLabel);
            titleContainer.Insert(2, isPlayerToggle);

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
