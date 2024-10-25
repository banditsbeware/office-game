using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bath : MonoBehaviour
{
    public literallyFlappyBird game;

    void OnEnable()
    {
        if(transform.parent.GetComponent<interact_minigame>().isInteractable) //only here to stop audio from playing when scene first loads
        {
            AkSoundEngine.PostEvent("Play_Bath_Amb", gameObject);
            AkSoundEngine.SetState("room", "bath");
        }
        Cursor.visible = false; //for the hand
    }

    void OnDisable()
    {
        game.gameOver();
        game.ExitBath();
        if(transform.parent.GetComponent<interact_minigame>().isInteractable) 
        {
            AkSoundEngine.ExecuteActionOnEvent("Play_Bath_Amb", 0, gameObject, 500);
            AkSoundEngine.SetState("room", "office");
        }
        Cursor.visible = true;
    }
}
