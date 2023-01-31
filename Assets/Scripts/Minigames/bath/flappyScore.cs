using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//placed on scorebox object above bird, when it collides with a pipe, score!
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
