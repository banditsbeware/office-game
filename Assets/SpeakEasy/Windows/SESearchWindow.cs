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
            List<SearchTreeEntry> searchTreeEntries = new List<SearchTreeEntry>()
            {
                new SearchTreeGroupEntry(new GUIContent("Create Element")),
                new SearchTreeGroupEntry(new GUIContent("Dialogue Node"), 1),
                new SearchTreeEntry(new GUIContent("Speaking", indentation))
                {
                    level = 2,
                    userData = SENodeType.Speaking
                },
                new SearchTreeEntry(new GUIContent("Multi Choice", indentation))
                {
                    level = 2,
                    userData = SENodeType.MultiChoice
                },
                new SearchTreeGroupEntry(new GUIContent("Logic Node"), 1),
                new SearchTreeEntry(new GUIContent("If", indentation))
                {
                    level = 2,
                    userData = SENodeType.If
                },
                new SearchTreeEntry(new GUIContent("If Else If", indentation))
                {
                    level = 2,
                    userData = SENodeType.IfElseIf
                },
                new SearchTreeEntry(new GUIContent("WeightedRandom", indentation))
                {
                    level = 2,
                    userData = SENodeType.WeightedRandom
                },
                new SearchTreeEntry(new GUIContent("Entry", indentation))
                {
                    level = 2,
                    userData = SENodeType.Entry
                },
                new SearchTreeEntry(new GUIContent("Exit", indentation))
                {
                    level = 2,
                    userData = SENodeType.Exit
                },
                new SearchTreeEntry(new GUIContent("Delay", indentation))
                {
                    level = 2,
                    userData = SENodeType.Delay
                },
                new SearchTreeGroupEntry(new GUIContent("Cosmetic"), 1),
                new SearchTreeEntry(new GUIContent("Connector", indentation))
                {
                    level = 2,
                    userData = SENodeType.Connector
                },

                new SearchTreeGroupEntry(new GUIContent("Dialogue Group"), 1),
                new SearchTreeEntry(new GUIContent("Single Group", indentation))
                {
                    level = 2,
                    userData = new Group()
                }
            };

            return searchTreeEntries;
        }

        public bool OnSelectEntry(SearchTreeEntry SearchTreeEntry, SearchWindowContext context)
        {
            Vector2 localMousePosition = graphView.GetLocalMousePosition(context.screenMousePosition, true);

            switch(SearchTreeEntry.userData)
            {
                case SENodeType.Speaking:
                {
                    SESpeakingNode speakingNode = (SESpeakingNode) graphView.CreateNode(SENodeType.Speaking, localMousePosition);
                    graphView.AddElement(speakingNode);
                    return true;
                }
                case SENodeType.MultiChoice:
                {
                    SEMultiChoiceNode multiChoiceNode = (SEMultiChoiceNode) graphView.CreateNode(SENodeType.MultiChoice, localMousePosition);
                    graphView.AddElement(multiChoiceNode);
                    return true;
                }
                case SENodeType.If:
                {
                    SEIfNode ifNode = (SEIfNode) graphView.CreateNode(SENodeType.If, localMousePosition);
                    graphView.AddElement(ifNode);
                    return true;
                }
                case SENodeType.IfElseIf:
                {
                    SEIfElseIfNode ifNode = (SEIfElseIfNode) graphView.CreateNode(SENodeType.IfElseIf, localMousePosition);
                    graphView.AddElement(ifNode);
                    return true;
                }
                case SENodeType.Entry:
                {
                    SEEntryNode entryNode = (SEEntryNode) graphView.CreateNode(SENodeType.Entry, localMousePosition, "_entry");
                    graphView.AddElement(entryNode);
                    return true;
                }
                case SENodeType.Exit:
                {
                    SEExitNode exitNode = (SEExitNode) graphView.CreateNode(SENodeType.Exit, localMousePosition, "_exit");
                    graphView.AddElement(exitNode);
                    return true;
                }
                case SENodeType.WeightedRandom:
                {
                    SEWeightedRandomNode weightedNode = (SEWeightedRandomNode) graphView.CreateNode(SENodeType.WeightedRandom, localMousePosition);
                    graphView.AddElement(weightedNode);
                    return true;
                }
                case SENodeType.Delay:
                {
                    SEDelayNode delayNode = (SEDelayNode) graphView.CreateNode(SENodeType.Delay, localMousePosition, "_delay");
                    graphView.AddElement(delayNode);
                    return true;
                }
                case SENodeType.Connector:
                {
                    SEConnectorNode connectorNode = (SEConnectorNode) graphView.CreateNode(SENodeType.Connector, localMousePosition);
                    graphView.AddElement(connectorNode);
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
