using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;

namespace SpeakEasy.Data.Error
{
  using Elements;

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