using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flappyScore : MonoBehaviour
{
    public literallyFlappyBird game;
    public AK.Wwise.Event chime;
    void OnTriggerEnter2D()
    {
        game.Score();
        chime.Post(game.gameObject);
    }
}
