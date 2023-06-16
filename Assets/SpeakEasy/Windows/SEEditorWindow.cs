using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEditor.AnimatedValues;
using UnityEngine.UIElements;

namespace SpeakEasy.Windows
{
    using Utilities;
    using Elements;
    using Enumerations;
    
    //creates the window in Unity in which the magic happens
    public class SEEditorWindow : EditorWindow
    {
        private SEGraphView graphView;
        private readonly string defaultFileName = "NewDialogueGraph";
        private static TextField fileNameTextField;
        private Button saveButton;
        private Button miniMapButton;

        [MenuItem("Window/SpeakEasy")]
        public static void Open()
        {
            Texture icon = AssetDatabase.LoadAssetAtPath<Texture>("Assets/SpeakEasy/Icons/SpeakingIcon.png");

            SEEditorWindow window = GetWindow<SEEditorWindow>();

            window.titleContent.image = icon;
            window.titleContent.text = "SpeakEasy";
        }

        private void OnEnable() 
        {
            AddGraphView();
            AddToolbar();
            AddStyles();

            SEIOUtility.Initialize(graphView, graphView.name);

            // HOTKEYS //
            rootVisualElement.RegisterCallback<KeyDownEvent>(evt =>
            {
                Vector2 localMousePosition = graphView.GetLocalMousePosition(evt.originalMousePosition);

                switch (evt.keyCode)
                {
                    case KeyCode.Alpha1:
                        SESpeakingNode speakingNode = (SESpeakingNode) graphView.CreateNode(SENodeType.Speaking, localMousePosition);
                        graphView.AddElement(speakingNode);
                        break;

                    case KeyCode.Alpha2:
                        SEMultiChoiceNode multiChoiceNode = (SEMultiChoiceNode) graphView.CreateNode(SENodeType.MultiChoice, localMousePosition);
                        graphView.AddElement(multiChoiceNode);
                        break;

                    case KeyCode.Alpha3:
                        SEIfNode ifNode = (SEIfNode) graphView.CreateNode(SENodeType.If, localMousePosition);
                        graphView.AddElement(ifNode);
                        break;

                    case KeyCode.Alpha4:
                        SEDelayNode delayNode = (SEDelayNode) graphView.CreateNode(SENodeType.Delay, localMousePosition, "_delay");
                        graphView.AddElement(delayNode);
                        break;

                    case KeyCode.Alpha5:
                        SEConnectorNode connectorNode = (SEConnectorNode) graphView.CreateNode(SENodeType.Connector, localMousePosition);
                        graphView.AddElement(connectorNode);
                        break;

                    case KeyCode.Alpha9: //debugging
                        break;
                }
            });
        }

        private void OnDestroy() 
        {
            SEIOUtility.RemoveFolder("Assets/SpeakEasyData/CopyPasteBuffer");
        }

        #region Elements
        private void AddGraphView()
        {
            graphView = new SEGraphView(this);

            graphView.StretchToParentSize();
            rootVisualElement.Add(graphView); //EditorWindows have a rootVisualElement, which is the object you add UIElements to. It is essentially the parent of any visuals in the window
        }

        private void AddToolbar()
        {
            Toolbar toolbar = new Toolbar();

            fileNameTextField = SEElementUtility.CreateTextField(defaultFileName, "File Name:", callback =>
            {
                fileNameTextField.value = callback.newValue.RemoveWhitespaces().RemoveSpecialCharacters();
            });

            saveButton = SEElementUtility.CreateButton("Save", () => Save());

            Button loadButton = SEElementUtility.CreateButton("Load", () => Load());

            Button clearButton = SEElementUtility.CreateButton("Clear", () => Clear());

            Button resetButton = SEElementUtility.CreateButton("Reset", () => ResetGraph());

            miniMapButton = SEElementUtility.CreateButton("Minimap", () => ToggleMiniMap());

            toolbar.Add(fileNameTextField);
            toolbar.Add(saveButton);
            toolbar.Add(loadButton);
            toolbar.Add(clearButton);
            toolbar.Add(resetButton);
            toolbar.Add(miniMapButton);
            
            toolbar.AddStyleSheets("SpeakEasy/SEToolbarStyle.uss");

            rootVisualElement.Add(toolbar);
        }

        #region Toolbar Actions
        private void Save()
        {
            if (string.IsNullOrEmpty(fileNameTextField.value))
            {
                EditorUtility.DisplayDialog("uh oh stinky", "make a real file name!", "K");

                return;
            }
            
            SEIOUtility.Initialize(graphView, fileNameTextField.value);
            SEIOUtility.Save();
        }

        private void Clear(bool isLoading = false)
        {
            if (EditorUtility.DisplayDialog("Double Checking", "Are you sure you want to clear the dialogue graph", "go for it", "whoops sorry no"))
            {
                graphView.ClearGraph();
            }
        }

        private void ResetGraph()
        {
            Clear();

            UpdateFileName(defaultFileName);
        }

        private void Load()
        {
            string filePath = EditorUtility.OpenFilePanel("Dialogue Graphs", "Assets/SpeakEasy/Graphs", "asset");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            graphView.ClearGraph();

            SEIOUtility.Initialize(graphView, Path.GetFileNameWithoutExtension(filePath));
            SEIOUtility.Load();
        }

        private void ToggleMiniMap()
        {
            graphView.ToggleMiniMap();

            miniMapButton.ToggleInClassList("se-toolbar__button__selected");

            graphView.UpdateEdgeColors();
        }
        #endregion

        private void AddStyles()
        {
            rootVisualElement.AddStyleSheets("SpeakEasy/SEVariables.uss");  
        }

        #endregion

        #region Utilities
        public static void UpdateFileName(string newFileName)
        {
            fileNameTextField.value = newFileName;
        }

        public void EnableSaving()
        {
            saveButton.SetEnabled(true);
        }

        public void DisableSaving()
        {
            saveButton.SetEnabled(false);
        }
        #endregion
    }
}
