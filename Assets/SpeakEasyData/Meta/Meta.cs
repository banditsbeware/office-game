using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using SpeakEasy.Data;
using UnityEngine;
using SpeakEasy.Enumerations;

//contains saved data that transcends an entire play of the game
[System.Serializable]
public static class Meta
{
    public static bool DebuggingMode = true;


    //world variables
    public static Dictionary<string, dynamic> Global = new Dictionary<string, dynamic>();
    public static Dictionary<string, dynamic> Daily = new Dictionary<string, dynamic>();
    public static Dictionary<string, dynamic> Yesterdaily = new Dictionary<string, dynamic>();

    private static void InitializeVariables()
    {
        // Set initial values for the static variables here
        SetValue("chaos", 0, Global);
        SetValue("day", 1, Global);
        SetValue("eggs", 0, Global);
        SetValue("alcohol", 0, Global);
        SetValue("cigs", 0, Global);
        SetValue("flappyHighScore", 0, Global);
        SetValue("currentScene", "Office", Global);

        SetDaily();
        SetYesterday();
    }

    public static void SetDaily()
    {
        SetValue("afterWork", false, Daily);
        SetValue("waRepeatedChoice", 0, Daily);
        SetValue("bVisits", 0, Daily);
        SetValue("bEggAttempt", 0, Daily);
        SetValue("bAlcoholAttempt", 0, Daily);
        SetValue("bCigsAttempt", 0, Daily);
        SetValue("bPissedOffCosta", false, Daily);
        SetValue("alVisits", 0, Daily);
        SetValue("alGaveAlcohol", false, Daily);
        SetValue("alGaveCigs", false, Daily);
        SetValue("dJaxVisits", 0, Daily);
        SetValue("dPatVisits", 0, Daily);
        SetValue("dOrderedItem", 0, Daily);
        SetValue("dChosenItem", "Hot Dog", Daily);
        SetValue("dTotal", 0f, Daily);

    }

    public static void SetYesterday()
    {
        foreach (string key in Daily.Keys.ToList<string>())
        {
            SetValue("y_" + key, Daily[key], Yesterdaily);

        }
    }

    public static void DayReset()
    {
        SetYesterday();
        SetDaily();
    }

    //only used when editing dialogue graphs
    public static List<string> GetVaraibleKeys()
    {
        InitializeVariables();

        List<string> names = new List<string>();

        names.AddRange(Global.Keys.ToList<string>());
        names.AddRange(Daily.Keys.ToList<string>());
        names.AddRange(Yesterdaily.Keys.ToList<string>());

        foreach (string name in names)
        {
            name.RemoveWhitespaces();
        }

        return names;
    }

    public static void SetValue(string name, object value, Dictionary<string, dynamic> blackboard)
    {
        if (blackboard.ContainsKey(name))
        {
            if (blackboard[name] is string) blackboard[name] = value;
            if (blackboard[name] is int) blackboard[name] = int.Parse(value.ToString());
            if (blackboard[name] is float) blackboard[name] = float.Parse(value.ToString());
            if (blackboard[name] is bool) blackboard[name] = bool.Parse(value.ToString());
        }
        else
        {
            blackboard.Add(name, value);
        }
    }

    public static T GetValue<T>(string name)
    {
        Dictionary<string, dynamic> blackboard = BlackboardThatContains(name);

        if (blackboard != null)
        {
            return (T) blackboard[name];
        }
        
        return default (T);
        
    }

    #region Saving and Loading
    public static void ResetData()
    {
        InitializeVariables();
    }

    //use to serialize static data into an instance to be saved in JSON
    public static SerializableMeta Serialize()
    {
        SerializableMeta data = new SerializableMeta();
        foreach (KeyValuePair<string, dynamic> pair in Global)
        {
            if (pair.Value is int)
            {
                data.metaInts.Add(pair.Key, pair.Value);
            }
            if (pair.Value is float)
            {
                data.metaFloats.Add(pair.Key, pair.Value);
            }
            if (pair.Value is string)
            {
                data.metaStrings.Add(pair.Key, pair.Value);
            }
            if (pair.Value is bool)
            {
                data.metaBools.Add(pair.Key, pair.Value);
            }
        }
        
        return data;
    }

    // import instance data from JSON into the static variables
    public static void Import(SerializableMeta data)
    {
        foreach (KeyValuePair<string, int> datum in data.metaInts)
        {
            SetValue(datum.Key, datum.Value, Global);
        }
        foreach (KeyValuePair<string, float> datum in data.metaFloats)
        {
            SetValue(datum.Key, datum.Value, Global);
        }
        foreach (KeyValuePair<string, string> datum in data.metaStrings)
        {
            SetValue(datum.Key, datum.Value, Global);
        }
        foreach (KeyValuePair<string, bool> datum in data.metaBools)
        {
            SetValue(datum.Key, datum.Value, Global);
        }
    }
    #endregion

    //compares the variable names/values held in IfData, which is held in the Node Scriptable Object
    public static bool isIfStatementTrue(SEIfData ifData) 
    {
        string variable = ifData.contextVariableName;
        string symbol = ifData.comparisonSign;
        string toCompare = ifData.comparisonValue;

        Dictionary<string, dynamic> blackboard = BlackboardThatContains(variable);

        if (blackboard[variable] is int)
        {
            int variableValue = blackboard[variable];
            int toCompareValue = 0;
            
            //sets comparison value either to the contents of the data, or the variable value
            if (ifData.isMetaVariableComparison)
            {
                toCompareValue = blackboard[toCompare];
            }
            else
            {
                toCompareValue = int.Parse(toCompare);
            }
            
            switch (symbol)
            {
                case "==":
                    return variableValue == toCompareValue;

                case "!=":
                    return variableValue != toCompareValue;

                case ">":
                    return variableValue > toCompareValue;

                case "<":  
                    return variableValue < toCompareValue;

                case ">=":
                    return variableValue >= toCompareValue;

                case "<=":
                    return variableValue <= toCompareValue;
            }
            return false;
        }

        if (blackboard[variable] is float)
        {
            float variableValue = blackboard[variable];
            float toCompareValue = 0f;
            
            //sets comparison value either to the contents of the data, or the variable value
            if (ifData.isMetaVariableComparison)
            {
                toCompareValue = blackboard[toCompare];
            }
            else
            {
                toCompareValue = float.Parse(toCompare);
            }
            
            switch (symbol)
            {
                case "==":
                    return variableValue == toCompareValue;

                case "!=":
                    return variableValue != toCompareValue;

                case ">":
                    return variableValue > toCompareValue;

                case "<":  
                    return variableValue < toCompareValue;

                case ">=":
                    return variableValue >= toCompareValue;

                case "<=":
                    return variableValue <= toCompareValue;
            }
            return false;
        }

        if (blackboard[variable] is string)
        {
            string variableValue = blackboard[variable];
            string toCompareValue = "";

            if (ifData.isMetaVariableComparison)
            {
                toCompareValue = blackboard[toCompare];
            }
            else
            {
                toCompareValue = toCompare;
            }

            switch (symbol)
            {
                case "==":
                    return variableValue == toCompareValue;

                case "!=":
                    return variableValue != toCompareValue;
            }
            return false;
        }
        else if (blackboard[variable] is bool)
        {
            bool variableValue = blackboard[variable];
            bool toCompareValue = false;

            if (ifData.isMetaVariableComparison)
            {
                toCompareValue = blackboard[toCompare];
            }
            else
            {
                toCompareValue = bool.Parse(toCompare);
            }

            switch (symbol)
            {
                case "==":
                    return variableValue == toCompareValue;

                case "!=":
                    return variableValue != toCompareValue;
            }
        }

        return false;
    }

    #region Utility

    public static Dictionary<string, dynamic> BlackboardThatContains(string variableName)
    {
        if (Daily.ContainsKey(variableName))
        {
            return Daily;
        }
        if (Global.ContainsKey(variableName))
        {
            return Global;
        }

        return null;
        
    }

    #endregion
}

public class SerializableMeta
{
    public SerializableDictionary<string, int> metaInts = new SerializableDictionary<string, int>();
    public SerializableDictionary<string, float> metaFloats = new SerializableDictionary<string, float>();
    public SerializableDictionary<string, string> metaStrings = new SerializableDictionary<string, string>();
    public SerializableDictionary<string, bool> metaBools = new SerializableDictionary<string, bool>();

    public List<SerializableDictionary> dictionaries = new List<SerializableDictionary>();

    public SerializableMeta()
    {
        dictionaries.Add(metaInts);
        dictionaries.Add(metaFloats);
        dictionaries.Add(metaStrings);
        dictionaries.Add(metaBools);
    }
}
