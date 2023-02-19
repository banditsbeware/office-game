using UnityEngine;

namespace SpeakEasy.Data.Error
{
    //contains information about duplicate errors, held in lists in SEGraphView
    public class SEErrorData
    {
        //color of the nodes/groups with the same name in the graph
        public Color Color { get; set; }

        public SEErrorData()
        {
            GenerateRandomColor();
        }

        private void GenerateRandomColor()
        {
            Color = new Color32(
                (byte) Random.Range(65, 256),
                (byte) Random.Range(50, 176),
                (byte) Random.Range(50, 176),
                255
            );
        }
    }
}
