using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

using UnityEngine.UIElements;

namespace SpeakEasy.Utilities
{
    using Elements;

    //Utility methods for making VisualElements in graph
    public static class SEElementUtility
    {
        public static TextField CreateTextField(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textField = new TextField()
            {
                value = value,
                label = label
            };

            if (onValueChanged != null)
            {
                textField.RegisterValueChangedCallback(onValueChanged);
            }

            return textField;
        }

        public static TextField CreateTextArea(string value = null, string label = null, EventCallback<ChangeEvent<string>> onValueChanged = null)
        {
            TextField textArea = CreateTextField(value, label, onValueChanged);

            textArea.multiline = true;

            return textArea;
        }

        public static PopupField<T> CreatePopupField<T>(List<T> choices, int startingIndex = 0)
        {
            PopupField<T> popupField = new PopupField<T>(choices, startingIndex);

            return popupField;
        }

        public static Foldout CreateFoldout(string title, bool collapsed = false)
        {
            Foldout foldout = new Foldout()
            {
                text = title,
                value = !collapsed
            };

            return foldout;
        }

        public static Button CreateButton(string text, Action onClick = null)
        {
            Button button = new Button(onClick)
            {
                text = text
            };

            return button;
        }

        public static Toggle CreateToggle(bool isOn, EventCallback<ChangeEvent<bool>> onValueChanged = null)
        {
            Toggle toggle = new Toggle();

            toggle.RegisterValueChangedCallback<bool>(onValueChanged);

            toggle.value = isOn;

            return toggle;
        }

        public static Port CreatePort(this SENode node, string portName = "", Orientation orientation = Orientation.Horizontal, Direction direction = Direction.Output, Port.Capacity capacity = Port.Capacity.Single)
        {
            Port port = node.InstantiatePort(orientation, direction, capacity, typeof(bool));

            port.portName = portName;

            return port;
        }
    }
}
