using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipeCollider : MonoBehaviour
{
    
    public literallyFlappyBird flapGame;
    public AK.Wwise.Event impact;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (flapGame.inGame)
        {
            impact.Post(flapGame.gameObject);
            flapGame.gameOver();
        }
    }
}
