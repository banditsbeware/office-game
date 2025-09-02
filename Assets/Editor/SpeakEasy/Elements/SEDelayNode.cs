using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

namespace SpeakEasy.Elements
{
    using Enumerations;
    using Utilities;
    using Windows;
    using Data.Save;


  public class SEDelayNode : SELogicNode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName);

            NodeType = SENodeType.Delay;

            SEChoiceSaveData choiceData = new SEChoiceSaveData()
            {
                Text = "Delay Time"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            // Output Container //
            foreach(SEChoiceSaveData choice in Choices)
            {
                Port outPort = this.CreatePort();

                outPort.userData = choice;
                
                outputContainer.Add(outPort);

                TextField choiceTextField = SEElementUtility.CreateTextField(choice.Text, null, callback =>
                {
                    choice.Text = callback.newValue;
                });

                choiceTextField.AddClasses(
                    "se-node__text-field",
                    "se-node__choice-text-field",
                    "se-node__text-field__hidden"
                );

                outPort.Add(choiceTextField);
            }

            extensionContainer.Clear();

            RefreshExpandedState();
        }
    }
}
