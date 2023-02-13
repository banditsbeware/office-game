using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//contains saved data that transcends an entire play of the game
[System.Serializable]
public static class meta
{
    public static int chaos {get; private set;} //float from 1 to 100, scalable chaos
    public static int flappyHighScore; //high score in bird game
    public static string currentScene; 


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
}

public class SerializableMeta
{
    public int chaos;
    public int flappyHighScore;
    public string currentScene;
}
