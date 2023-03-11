using UnityEditor.Experimental.GraphView;
using UnityEngine.UIElements;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace SpeakEasy.Windows
{
    using Elements;
    using Enumerations;
    using Utilities;
    using Data.Error;
    using Data.Save;

    //creates the graph portion of the editor window, where all the nodes are
    public class SEGraphView : GraphView
    {
        private SEEditorWindow editorWindow;
        private SESearchWindow searchWindow;

        private MiniMap miniMap;

        private SerializableDictionary<string, SENodeErrorData> ungroupedNodes;
        private SerializableDictionary<Group, SerializableDictionary<string, SENodeErrorData>> groupedNodes; //list of groups, each containing a list of nodes' error data
        private SerializableDictionary<string, SEGroupErrorData> groups;

        private int nameErrors;
        public int NameErrors
        {
            get
            {
                return nameErrors;
            }
            set
            {
                nameErrors = value;
                if (nameErrors == 0)
                {
                    editorWindow.EnableSaving();
                }
                if (nameErrors == 1)
                {
                    editorWindow.DisableSaving();
                }
            }
        }

        public SEGraphView(SEEditorWindow seEditorWindow)
        {
            editorWindow = seEditorWindow;

            ungroupedNodes = new SerializableDictionary<string, SENodeErrorData>();
            groupedNodes = new SerializableDictionary<Group, SerializableDictionary<string, SENodeErrorData>>();
            groups = new SerializableDictionary<string, SEGroupErrorData>();

            AddManipulators();
            AddSearchWindow();
            AddMinimap();
            AddGridBackground();

            OnElementsDeleted();  //overriding methods from the inherited Element and Group classes
            OnGroupElementsAdded();
            OnGroupElementsRemoved();
            OnGroupRenamed();
            OnGraphViewChanged();
            OnSerializeGraphElements();
            OnUnserializeAndPaste();
            
            AddStyles();
            AddMiniMapStyles();
        }

        #region Overridden Methods
        public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter) 
        {
            List<Port> compatiblePorts = new List<Port>();

            //dont connect with own node's ports or any port of the same type
            ports.ForEach(port => 
            {
                if (startPort.node == port.node || startPort.direction == port.direction)
                {
                    return;
                }

                compatiblePorts.Add(port);
            });

            return compatiblePorts;
        }
        #endregion

        #region Manipulator Creation
        private void AddManipulators()
        {
            SetupZoom(ContentZoomer.DefaultMinScale, ContentZoomer.DefaultMaxScale);

            this.AddManipulator(new SelectionDragger());
            this.AddManipulator(new RectangleSelector());
            this.AddManipulator(new ContentDragger());

            this.AddManipulator(CreateNodeContextualMenu(SENodeType.Speaking, "Add Speaking Node"));
            this.AddManipulator(CreateNodeContextualMenu(SENodeType.MultiChoice,  "Add Multi Choice Node"));
            this.AddManipulator(CreateNodeContextualMenu(SENodeType.If, "Add If Node"));
            this.AddManipulator(CreateNodeContextualMenu(SENodeType.Connector, "Add Connector"));
            
            this.AddManipulator(CreateGroupContextualMenu());
        }

        private IManipulator CreateNodeContextualMenu(SENodeType dialogueType, string actionTitle)
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
                menuEvent => menuEvent.menu.AppendAction(actionTitle, actionEvent => AddElement(CreateNode(dialogueType, GetLocalMousePosition(actionEvent.eventInfo.localMousePosition))))
            );

            return contextualMenuManipulator;
        }
        private IManipulator CreateGroupContextualMenu()
        {
            ContextualMenuManipulator contextualMenuManipulator = new ContextualMenuManipulator(
            menuEvent => menuEvent.menu.AppendAction("Add Group", actionEvent => CreateGroup("DialogueGroup", GetLocalMousePosition(actionEvent.eventInfo.localMousePosition)))
            );

            return contextualMenuManipulator;
        }
        #endregion

        #region Elements
        public SENode CreateNode(SENodeType dialogueType, Vector2 position, string nodeName = "Dialogue Node", bool isPlayer = false, bool shouldDraw = true)
        {
            Type nodeType = Type.GetType($"SpeakEasy.Elements.SE{dialogueType}Node");
            SENode node = (SENode) Activator.CreateInstance(nodeType);

            node.Initialize(this, position, nodeName, isPlayer);  //sets default text and values

            if (shouldDraw)
            {
                node.Draw();  //sets up all containers in the node
            }

            LogUngroupedNode(node);

            return node;
        }

        public SEGroup CreateGroup(string title, Vector2 localMousePosition)
        {
            SEGroup group = new SEGroup(title, localMousePosition);
            LogGroup(group);
            AddElement(group);

            group.SetPosition(new Rect(localMousePosition, Vector2.zero));

            foreach (GraphElement selectedElement in selection)
            {
                if (!(selectedElement is SENode))
                {
                    continue;
                }

                SENode node = (SENode) selectedElement;

                group.AddElement(node);
            }

            return group;

        }

        private void AddSearchWindow()
        {
            if(searchWindow == null)
            {
                searchWindow = ScriptableObject.CreateInstance<SESearchWindow>();
                searchWindow.Initialize(this);
            }

            nodeCreationRequest = context => SearchWindow.Open(new SearchWindowContext(context.screenMousePosition), searchWindow);
        }

        private void AddGridBackground()
        {
            GridBackground gridBackground = new GridBackground();
            gridBackground.StretchToParentSize();
            Insert(0, gridBackground);
        }

        private void AddMinimap()
        {
            miniMap = new MiniMap()
            {
                anchored = true
            };

            miniMap.SetPosition(new Rect(15, 50, 200, 180));
            Add(miniMap);

            miniMap.visible = false;
        }

        private void AddStyles()
        {
            this.AddStyleSheets(
                "SpeakEasy/SEVariables.uss",
                "SpeakEasy/SEGraphViewStyle.uss",
                "SpeakEasy/SENodeStyle.uss"
            );
        }

        private void AddMiniMapStyles()
        {
            StyleColor backgroundColor = new StyleColor(new Color32(29, 29, 30, 255));
            StyleColor borderColor = new StyleColor(new Color32(51, 51, 51, 255));

            miniMap.style.backgroundColor = backgroundColor;
            miniMap.style.borderTopColor = borderColor;
            miniMap.style.borderRightColor = borderColor;
            miniMap.style.borderBottomColor = borderColor;
            miniMap.style.borderLeftColor = borderColor;
        }

        #region Callbacks
        private void OnElementsDeleted()
        {
            deleteSelection = (operationName, askUser) =>
            {
                Type groupType = typeof(SEGroup);
                Type edgeType = typeof(Edge);

                List<SEGroup> groupsToDelete = new List<SEGroup>();
                List<SENode> nodesToDelete = new List<SENode>();
                List<Edge> edgesToDelete = new List<Edge>();

                foreach (GraphElement element in selection)
                {
                    if (element is SENode node)
                    {
                        nodesToDelete.Add(node);
                        continue;
                    }

                    if (element.GetType() == edgeType)
                    {
                        edgesToDelete.Add((Edge) element);
                        continue;
                    }

                    if (element.GetType() == groupType)
                    {
                        groupsToDelete.Add((SEGroup) element);
                        continue;
                    }
                }

                foreach (SEGroup group in groupsToDelete)
                {
                    List<SENode> groupNodes = new List<SENode>();

                    foreach (GraphElement groupElement in group.containedElements)
                    {
                        if (!(groupElement is SENode))
                        {
                            continue;
                        }

                        groupNodes.Add((SENode) groupElement);
                    }

                    group.RemoveElements(groupNodes);

                    UnlogGroup(group);

                    RemoveElement(group);
                }

                DeleteElements(edgesToDelete);

                foreach (SENode node in nodesToDelete)
                {
                    if (node.Group != null)
                    {
                        node.Group.RemoveElement(node);
                    }

                    UnlogUngroupedNode(node);

                    node.DisconnectAllPorts();

                    RemoveElement(node);

                    continue;
                }

                
            };
        }

        private void OnGroupElementsAdded()
        {
            elementsAddedToGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is SENode))
                    {
                        continue;
                    }

                    SENode node = (SENode) element;

                    if(ungroupedNodes.ContainsKey(node.NodeName)) 
                    {
                        UnlogUngroupedNode(node);
                    }
                    LogGroupedNode(node, (SEGroup) group);
                }
            };
        }

        private void OnGroupElementsRemoved()
        {
            elementsRemovedFromGroup = (group, elements) =>
            {
                foreach (GraphElement element in elements)
                {
                    if (!(element is SENode))
                    {
                        continue;
                    }

                    SENode node = (SENode) element;

                    // Remove node from its group
                    UnlogGroupedNode(node, (SEGroup) group);
                    LogUngroupedNode(node);
                }
            };
        }
        
        private void OnGroupRenamed()
        {
            groupTitleChanged = (group, newTitle) =>
            {
                SEGroup seGroup = (SEGroup) group;

                seGroup.title = newTitle.RemoveWhitespaces().RemoveSpecialCharacters();

                if (string.IsNullOrEmpty(seGroup.title))
                {
                    if (!string.IsNullOrEmpty(seGroup.oldTitle))
                    {

                        NameErrors++;
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(seGroup.oldTitle))
                    {
                        NameErrors--;
                    }
                }

                UnlogGroup(seGroup);

                seGroup.oldTitle = seGroup.title;

                LogGroup(seGroup);
            };
        }

        private void OnGraphViewChanged()
        {
            graphViewChanged = (changes) =>
            {
                if (changes.edgesToCreate != null)
                {
                    foreach (Edge edge in changes.edgesToCreate)
                    {
                        SENode nextNode = (SENode) edge.input.node;

                        SEChoiceSaveData choiceData = (SEChoiceSaveData) edge.output.userData;

                        choiceData.NodeID = nextNode.ID;

                        if (edge.input.node.GetPosition().x < edge.output.node.GetPosition().x)
                        {
                            edge.input.portColor = new Color32(100, 100, 100, 100);
                            edge.output.portColor = new Color32(100, 100, 100, 100);
                        }
                    }
                }

                if (changes.elementsToRemove != null)
                {
                    Type edgeType = typeof(Edge);

                    foreach (GraphElement element in changes.elementsToRemove)
                    {
                        if (element.GetType() != edgeType)
                        {
                            continue;
                        }

                        Edge edge = (Edge) element;

                        if (edge.input.node.GetPosition().x < edge.output.node.GetPosition().x)
                        {
                            edge.input.portColor = edge.defaultColor;
                            edge.output.portColor = edge.defaultColor;
                        }

                        SEChoiceSaveData choiceData = (SEChoiceSaveData) edge.output.userData;

                        choiceData.NodeID = "";
                    }

                }

                return changes;
            };
        }
        
        private void OnSerializeGraphElements()
        { 
            serializeGraphElements = (selection) =>
            {
                SEIOUtility.Copy(selection);
                
                return "Selection Data";
            };
            
        }

        private void OnUnserializeAndPaste()
        {
            unserializeAndPaste = (operationName, data) =>
            {
                SEIOUtility.Paste();
            };
        }
        
        #endregion

        #region Repeated Elements
        public void LogUngroupedNode(SENode node)
        {
            string nodeName = node.NodeName.ToLower();

            if (!ungroupedNodes.ContainsKey(nodeName))
            {
                SENodeErrorData nodeErrorData = new SENodeErrorData();
                nodeErrorData.Nodes.Add(node);

                ungroupedNodes.Add(nodeName, nodeErrorData);

                return;
            }

            List<SENode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;
            ungroupedNodesList.Add(node);

            Color errorColor = ungroupedNodes[nodeName].ErrorData.Color;
            node.SetErrorColor(errorColor);

            if (ungroupedNodesList.Count == 2)
            {
                ++NameErrors;

                ungroupedNodesList[0].SetErrorColor(errorColor);
            }
        }

        public void UnlogUngroupedNode(SENode node)
        {
            string nodeName = node.NodeName.ToLower();

            

            ungroupedNodes[nodeName].Nodes.Remove(node);

            List<SENode> ungroupedNodesList = ungroupedNodes[nodeName].Nodes;

            

            node.ResetColor();

            if (ungroupedNodesList.Count == 1)
            {
                --NameErrors;

                ungroupedNodesList[0].ResetColor();
                
                return;
            }

            if (ungroupedNodesList.Count == 0)
            {
                ungroupedNodes.Remove(nodeName);
            }
        }

        public void LogGroupedNode(SENode node, SEGroup group)
        {
            string nodeName = node.NodeName.ToLower();

            node.Group = group;

            if (!groupedNodes.ContainsKey(group))
            {
                groupedNodes.Add(group, new SerializableDictionary<string, SENodeErrorData>());
            }

            if (!groupedNodes[group].ContainsKey(nodeName))
            {
                SENodeErrorData nodeErrorData = new SENodeErrorData();
                nodeErrorData.Nodes.Add(node);

                groupedNodes[group].Add(nodeName, nodeErrorData);

                return;
            }

            List<SENode> groupedNodesList = groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Add(node);

            Color errorColor = groupedNodes[group][nodeName].ErrorData.Color;
            node.SetErrorColor(errorColor);

            if (groupedNodesList.Count == 2)
            {
                ++NameErrors;

                groupedNodesList[0].SetErrorColor(errorColor);
            }
        }

        public void UnlogGroupedNode(SENode node, SEGroup group)
        {
            string nodeName = node.NodeName.ToLower();

            node.Group = null;

            List<SENode> groupedNodesList = groupedNodes[group][nodeName].Nodes;
            groupedNodesList.Remove(node);

            node.ResetColor();

            if (groupedNodesList.Count == 1)
            {
                --NameErrors;

                groupedNodesList[0].ResetColor();

                return;
            }

            if (groupedNodesList.Count == 0)
            {
                groupedNodes[group].Remove(nodeName);

                if (groupedNodes[group].Count == 0)
                {
                    groupedNodes.Remove(group);
                }
            }
        }

        private void LogGroup(SEGroup group)
        {
            string groupName = group.title.ToLower();

            if (!groups.ContainsKey(groupName))
            {
                SEGroupErrorData groupErrorData = new SEGroupErrorData();

                groupErrorData.Groups.Add(group);

                groups.Add(groupName, groupErrorData);

                return;
            }

            List<SEGroup> groupsList = groups[groupName].Groups;
            groupsList.Add(group);

            Color errorColor = groups[groupName].ErrorData.Color;
            group.SetErrorColor(errorColor);

            if (groupsList.Count == 2)
            {
                ++NameErrors;

                groupsList[0].SetErrorColor(errorColor);
            }
        }

        private void UnlogGroup(SEGroup group)
        {
            string oldGroupName = group.oldTitle.ToLower();

            List<SEGroup> groupsList = groups[oldGroupName].Groups;

            groupsList.Remove(group);
            group.ResetColor();

            if (groupsList.Count == 1)
            {
                --NameErrors;

                groupsList[0].ResetColor();

                return;
            }

            if (groupsList.Count == 0)
            {
                groups.Remove(oldGroupName);
            }
        }
        #endregion

        #endregion

        #region Utilities
        public Vector2 GetLocalMousePosition(Vector2 mousePosition, bool isSearchWindow = false)
        {
            Vector2 worldMousePosition = mousePosition;

            if (isSearchWindow)
            {
                worldMousePosition -= editorWindow.position.position;
            }

            Vector2 localMousePosition = contentViewContainer.WorldToLocal(worldMousePosition);

            return localMousePosition;
        }

        public void ClearGraph()
        {
            graphElements.ForEach(graphElement => RemoveElement(graphElement));

            groups.Clear();
            groupedNodes.Clear();
            ungroupedNodes.Clear();

            nameErrors = 0;
        }

        public void ToggleMiniMap()
        {
            miniMap.visible = !miniMap.visible;
        }

        public void UpdateEdgeColors()
        {
            foreach (GraphElement element in graphElements)
            {
                if (element is Edge) 
                {
                    Edge edge = (Edge) element;

                    if (edge.input.node.GetPosition().x < edge.output.node.GetPosition().x)
                    {
                        edge.input.portColor = new Color32(100, 100, 100, 100);
                        edge.output.portColor = new Color32(100, 100, 100, 100);
                    }
                }
            }
        }
        #endregion
    }
}

