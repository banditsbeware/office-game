using System.Collections.Generic;

namespace SpeakEasy.Data.Error
{
    using Elements;

    //contains a list of nodes that have the same title, a new instance is created for each unique title
    public class SENodeErrorData
    {
        public SEErrorData ErrorData { get; set; }
        public List<SENode> Nodes { get; set; }

        public SENodeErrorData()
        {
            ErrorData = new SEErrorData();
            Nodes = new List<SENode>();
        }
    }
}