using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System;
using SpeakEasy.Data;
using UnityEngine;

//contains saved data that transcends an entire play of the game
[System.Serializable]
public static class meta
{
    //world variables
    public static Dictionary<string, dynamic> Variables = new Dictionary<string, dynamic>();
    public static string currentScene = "Office";

    private static void InitializeVariables()
    {
        // Set initial values for the static variables here
        MetaSet("chaos", 0);
        MetaSet("day", 1);
        MetaSet("flappyHighScore", 0);
        MetaSet("currentScene", "Office");
    }

    public static List<string> GetVaraibleKeys()
    {
        InitializeVariables();
        return Variables.Keys.ToList<string>();
    }

    public static void MetaSet(string name, object value)
    {
        if (Variables.ContainsKey(name))
        {
            Variables[name] = value;
        }
        else
        {
            Variables.Add(name, value);
        }
    }

    public static T MetaGet<T>(string name)
    {
        if (Variables.ContainsKey(name))
        {
            return (T) Variables[name];
        }
        else
        {
            return default (T);
        }
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
        foreach (KeyValuePair<string, dynamic> pair in Variables)
        {
            if (pair.Value is int)
            {
                data.metaInts.Add(pair.Key, pair.Value);
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
            MetaSet(datum.Key, datum.Value);
        }
        foreach (KeyValuePair<string, string> datum in data.metaStrings)
        {
            MetaSet(datum.Key, datum.Value);
        }
        foreach (KeyValuePair<string, bool> datum in data.metaBools)
        {
            MetaSet(datum.Key, datum.Value);
        }
    }
    #endregion

    public static bool IfStatement(SEIfData ifData) //compares the variable names/values held in IfData, which is held in the Node Scriptable Object
    {
        string variable = ifData.contextVariableName;
        string symbol = ifData.comparisonSign;
        string toCompare = ifData.comparisonValue;

        if (Variables[variable] is int)
        {
            int variableValue = Variables[variable];
            int toCompareValue = int.Parse(toCompare);
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

        if (Variables[variable] is string)
        {
            string variableValue = Variables[variable];
            switch (symbol)
            {
                case "==":
                    return variableValue == toCompare;

                case "!=":
                    return variableValue != toCompare;
            }
            return false;
        }
        else if (Variables[variable] is bool)
        {
            bool variableValue = Variables[variable];
            bool toCompareValue = bool.Parse(toCompare);
            switch (symbol)
            {
                case "==":
                    return variable == toCompare;

                case "!=":
                    return variable != toCompare;
            }
            return false;
        }

        return false;
    }
}

public class SerializableMeta
{
    public SerializableDictionary<string, int> metaInts = new SerializableDictionary<string, int>();
    public SerializableDictionary<string, string> metaStrings = new SerializableDictionary<string, string>();
    public SerializableDictionary<string, bool> metaBools = new SerializableDictionary<string, bool>();

    public List<SerializableDictionary> dictionaries = new List<SerializableDictionary>();

    public SerializableMeta()
    {
        dictionaries.Add(metaInts);
        dictionaries.Add(metaStrings);
        dictionaries.Add(metaBools);
    }
}
