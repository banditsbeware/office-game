using UnityEngine;
using UnityEngine.UIElements;

namespace SpeakEasy.Elements
{
    using Utilities;
    using Windows;

    //class parent to SingleChoice and MultiChoice nodes. Add the Dialogue Text dropdown underneath the node and the isPlayer Toggle
    public class SESpeakingNode : SENode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            DialogueText = "Dialoge text.";

            IsPlayer = isPlayer;

            base.Initialize(seGraphView, position, nodeName);
        }

        public override void Draw()
        {
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
        }
    }
}
