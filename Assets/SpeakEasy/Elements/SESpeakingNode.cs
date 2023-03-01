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
            base.Initialize(seGraphView, position, nodeName, isPlayer);

            SEChoiceSaveData choiceData = new SEChoiceSaveData()
            {
                Text = "Next Node"
            };

            NodeType = SENodeType.Speaking;

            Choices.Add(choiceData);

            DialogueText = "Dialoge text.";

            IsPlayer = isPlayer;
        }

        public override void Draw()
        {
            // Extensions Container //
            VisualElement customDataContainer = new VisualElement();

            customDataContainer.AddToClassList("se-node__custom-data-container");

            Foldout textFoldout = SEElementUtility.CreateFoldout("Dialogue Text");

            TextField textTextField = SEElementUtility.CreateTextArea(DialogueText, null, callback => DialogueText = callback.newValue);

            textTextField.AddClasses(
                "se-node__text-field",
                "se-node__quote-text-field"
            );

            textFoldout.Add(textTextField);
            customDataContainer.Add(textFoldout);
            extensionContainer.Add(customDataContainer);

            textFoldout.value = false;

            // Base Draw
            base.Draw();

            // Title Container //
            Label toggleLabel = new Label("isPlayer:");

            toggleLabel.AddClasses("se-node__label");

            Toggle isPlayerToggle = SEElementUtility.CreateToggle(IsPlayer, callback => 
            {
                IsPlayer = callback.newValue;
            });

            isPlayerToggle.AddClasses("se-node");

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
