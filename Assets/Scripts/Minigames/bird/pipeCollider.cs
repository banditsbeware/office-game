using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeCollider : MonoBehaviour
{
    public literallyFlappyBird flapGame;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (flapGame.inGame)
        {
            flapGame.gameOver();
        }
    }
}
