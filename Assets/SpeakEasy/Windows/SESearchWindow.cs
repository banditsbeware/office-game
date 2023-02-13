using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace SpeakEasy.Windows
{
    using Enumerations;
    using Elements;
    public class SESearchWindow : ScriptableObject, ISearchWindowProvider
    {
        private SEGraphView graphView;
        private Texture2D indentation;

        public void Initialize(SEGraphView seGraphView)
        {
            graphView = seGraphView;

            indentation = new Texture2D(1, 1);
            indentation.SetPixel(0, 0, Color.clear);
            indentation.Apply();
        }
        public List<SearchTreeEntry> CreateSearchTree(SearchWindowContext context)
        {
            List<SearchTreeEntry> searchTreeeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Single Choice", indentation))
                {
                    level = 2,
                    userData = SENodeType.SingleChoice
                },
                new SearchTreeEntry(new GUIContent("Multi Choice", indentation))
                {
                    level = 2,
                    userData = SENodeType.MultiChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentation))
                {
                    level = 2,
                    userData = new Group()
                }
            };

            return searchTreeeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch(SearchTreeEntry.userData)
            {
                case SENodeType.SingleChoice:
                {
                    SESingleChoiceNode singleChoiceNode = (SESingleChoiceNode) graphView.CreateNode(SENodeType.SingleChoice, localMousePosition);
                    graphView.AddElement(singleChoiceNode);
                    return true;
                }
                case SENodeType.MultiChoice:
                {
                    SEMultiChoiceNode multiChoiceNode = (SEMultiChoiceNode) graphView.CreateNode(SENodeType.MultiChoice, localMousePosition);
                    graphView.AddElement(multiChoiceNode);
                    return true;
                }
                case Group _:
                {
                    graphView.CreateGroup("DialogueGroup", localMousePosition);
                    return true;
                }
                default:
                {
                    return false;
                }
            }
        }
    }
}
