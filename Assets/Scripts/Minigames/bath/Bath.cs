using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bath : MonoBehaviour
{
    public literallyFlappyBird game;

    void OnEnable()
    {
        if(transform.parent.GetComponent<interact_minigame>().isGame) 
        {
            AkSoundEngine.PostEvent("Play_Bath_Amb", gameObject);
            AkSoundEngine.SetState("room", "bath");
        }
        Cursor.visible = false;
    }

    void OnDisable()
    {
        game.gameOver();
        game.ExitBath();
        if(transform.parent.GetComponent<interact_minigame>().isGame) 
        {
            AkSoundEngine.ExecuteActionOnEvent("Play_Bath_Amb", 0, gameObject, 500);
            AkSoundEngine.SetState("room", "office");
        }
        Cursor.visible = true;
    }
}
