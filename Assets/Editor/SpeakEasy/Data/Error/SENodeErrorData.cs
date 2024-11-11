using System.Collections.Generic;

namespace SpeakEasy.Data.Error
{
    using Elements;

    //error data about nodes
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