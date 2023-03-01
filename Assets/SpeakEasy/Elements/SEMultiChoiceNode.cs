using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;

namespace SpeakEasy.Elements
{
    using Data.Save;
    using Enumerations;
    using Utilities;
    using Windows;
  
    //node for choosing from a selection of dialogue choices
    public class SEMultiChoiceNode : SENode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName, isPlayer);

            NodeType = SENodeType.MultiChoice;

            SEChoiceSaveData choiceData = new SEChoiceSaveData()
            {
                Text = "new choice"
            };

            Choices.Add(choiceData);
        }

        public override void Draw()
        {
            base.Draw();

            // Main Container (above inputs/outputs) //
            Button addChoiceButton = SEElementUtility.CreateButton("Add Port", () =>
            {
                SEChoiceSaveData choiceData = new SEChoiceSaveData()
                {
                    Text = "new choice"
                };

                Choices.Add(choiceData);

                Port choicePort = CreateChoicePort(choiceData);

                outputContainer.Add(choicePort);
            });

            addChoiceButton.AddToClassList("se-node__button");

            mainContainer.Insert(1, addChoiceButton);
            
            // Output Container //
            foreach(SEChoiceSaveData choice in Choices)
            {
                Port choicePort = CreateChoicePort(choice);

                outputContainer.Add(choicePort);
            }

            RefreshExpandedState();
        }
    }
}
