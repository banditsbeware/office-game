using System.Collections.Generic;

namespace SpeakEasy.Data.Error
{
    using Elements;
    // error data about groups
    public class SEGroupErrorData
    {
        public SEErrorData ErrorData { get; set; }
        public List<SEGroup> Groups { get; set; }

        public SEGroupErrorData()
        {
            ErrorData = new SEErrorData();
            Groups = new List<SEGroup>();
        }
    }
}