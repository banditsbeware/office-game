using SpeakEasy;
using UnityEngine;

//game progress script
public static class ProgressChecks
{
    public static bool LeaveWorkCheck(string sceneName)
    {
      if (sceneName != "Street" || Meta.Global["currentScene"] != "Office") //Only apply if attempting to leave Office
      {
          return true;
      }
      
      if (Meta.Daily["workComplete"] == true)
      {
        Meta.SetValue("afterWork", true, Meta.Daily);
        return true;
      }
      
      return false;
    }
}
