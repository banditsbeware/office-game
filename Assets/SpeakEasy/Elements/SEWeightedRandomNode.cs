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
    public class SEWeightedRandomNode : SELogicNode
    {
        public override void Initialize(SEGraphView seGraphView, Vector2 position, string nodeName, bool isPlayer = false)
        {
            base.Initialize(seGraphView, position, nodeName, isPlayer);

            NodeType = SENodeType.WeightedRandom;

            SEChoiceSaveData weightData = new SEChoiceSaveData()
            {
                Text = "weight"
            };

            Choices.Add(weightData);
        }

        public override void Draw()
        {
            base.Draw();

            // Main Container (above inputs/outputs) //
            Button addPossibilityButton = SEElementUtility.CreateButton("Add Possibility", () =>
            {
                SEChoiceSaveData weightData = new SEChoiceSaveData()
                {
                    Text = "weight"
                };

                Choices.Add(weightData);

                Port randomPort = CreateChoicePort(weightData);

                outputContainer.Add(randomPort);
            });

            addPossibilityButton.AddToClassList("se-node__button");

            mainContainer.Insert(1, addPossibilityButton);
            
            // Output Container //
            foreach(SEChoiceSaveData possibility in Choices)
            {
                Port randomPort = CreateChoicePort(possibility);

                outputContainer.Add(randomPort);
            }

            RefreshExpandedState();
        }
    }
}
