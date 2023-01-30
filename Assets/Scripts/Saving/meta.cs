using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//contains saved data that transcends an entire play of the game
[System.Serializable]
public class meta 
{
    public float chaos;
    public int flappyHighScore;

    public meta()
    {
        chaos = 0;
        flappyHighScore = 0;
    }
}
