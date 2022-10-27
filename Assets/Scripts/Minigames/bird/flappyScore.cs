using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flappyScore : MonoBehaviour
{
    public literallyFlappyBird game;
    void OnTriggerEnter2D()
    {
        game.Score();
    }
}
