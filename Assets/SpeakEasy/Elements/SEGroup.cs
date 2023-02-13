using System;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace SpeakEasy.Elements
{
    public class SEGroup : Group
    {
        public string ID;
        public string oldTitle;
        private Color defaultBorderColor;
        private float defaultBorderWidth;

        public SEGroup(string groupTitle, Vector2 position)
        {
            ID = Guid.NewGuid().ToString();
            title = groupTitle;
            oldTitle = groupTitle;
            
            SetPosition(new Rect(position, Vector2.zero));

            defaultBorderColor = contentContainer.style.borderBottomColor.value;
            defaultBorderWidth = contentContainer.style.borderBottomWidth.value;
        }

        public void SetErrorColor(Color color)
        {
            contentContainer.style.borderBottomColor = color;
            contentContainer.style.borderBottomWidth = 2f;
        }

        public void ResetColor()
        {
            contentContainer.style.borderBottomColor = defaultBorderColor;
            contentContainer.style.borderBottomWidth = defaultBorderWidth;
        }

    }
}
