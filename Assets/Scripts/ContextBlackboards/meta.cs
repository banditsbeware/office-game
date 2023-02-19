using System.Collections.Generic;
using System.Linq;
using System;
using SpeakEasy.Data;

//contains saved data that transcends an entire play of the game
[System.Serializable]
public static class meta
{
    //world variables
    public static int chaos; //float from 1 to 100, scalable chaos
    public static int day; //day in game
    public static int flappyHighScore; //high score in bird game
    public static string currentScene = "Office";
    



    //for calling data in dialogues
    public delegate T GetIntDelegate<T>();
    public static SerializableDictionary<string, GetIntDelegate<object>> VariableMap = new SerializableDictionary<string, GetIntDelegate<object>>();
    private static bool listIsPopulated;

    #region Saving and Loading
    public static void ResetData()
    {
        chaos = 0;
        flappyHighScore = 0;
        currentScene = "Office"; //scene loaded on new game
    }

    //use to serialize static data into an instance to be saved in JSON
    public static SerializableMeta Serialize()
    {
        SerializableMeta data = new SerializableMeta();

        data.chaos = chaos;
        data.flappyHighScore = flappyHighScore;
        data.currentScene = currentScene;

        return data;
    }

    // import instance data from JSON into the static variables
    public static void Import(SerializableMeta data)
    {
        chaos = data.chaos;
        flappyHighScore = data.flappyHighScore;
        currentScene = data.currentScene;
    }
    #endregion

    #region SpeakEasy Utility
    //get a list of all meta variables' names
    public static List<string> getAllVariableNames()
    {
        AddVariablesToList();

        return VariableMap.Keys.ToList<string>();
    }
    
    #endregion

    #region Runtime Utility

    public static void AddVariablesToList() //populates a map of variables and calls to those variables
    {
        if (!listIsPopulated)
        {
            VariableMap.Add("chaos", () => chaos);
            VariableMap.Add("flappyHighScore", () => flappyHighScore);
            VariableMap.Add("currentScene", () => currentScene);

            listIsPopulated = true;
        }
    }

    #endregion
    public static bool IfStatement(SEIfData ifData) //compares the variable names/values held in IfData, which is held in the Node Scriptable Object
    {
        string variable = ifData.contextVariableName;
        string symbol = ifData.comparisonSign;
        string toCompare = ifData.comparisonValue;

        Type variableType = VariableMap[variable]().GetType();


        if (variableType == typeof(int))
        {
            int variableValue = (int) VariableMap[variable]();
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

        if (variableType == typeof(string))
        {
            string variableValue = (string) VariableMap[variable]();
            switch (symbol)
            {
                case "==":
                    return variableValue == toCompare;

                case "!=":
                    return variableValue != toCompare;
            }
            return false;
        }
        else if (variableType == typeof(bool))
        {
            bool variableValue = (bool) VariableMap[variable]();
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
    public int chaos;
    public int flappyHighScore;
    public string currentScene;
}
