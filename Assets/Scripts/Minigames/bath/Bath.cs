using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bath : MonoBehaviour
{
    public AK.Wwise.Bank birdBank;
    public literallyFlappyBird game;

    void OnEnable()
    {
        birdBank.Load();
        StartCoroutine(beginNoise());
        Cursor.visible = false;
    }

    IEnumerator beginNoise()
    {
        yield return new WaitForSeconds(.5f);
        AkSoundEngine.PostEvent("Play_Bath_Amb", gameObject);
        AkSoundEngine.SetState("room", "bath");
    }

    void OnDisable()
    {
        game.gameOver();
        game.ExitBath();
        AkSoundEngine.ExecuteActionOnEvent("Play_Bath_Amb", 0, gameObject, 500);
        AkSoundEngine.SetState("room", "office");
        Cursor.visible = true;
    }
}
