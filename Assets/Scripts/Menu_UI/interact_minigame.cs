using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact_minigame : interact
{
    public GameObject theGame;
    private void Update()
    {
        if (isGame)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIMan.denoitfy();
                UIManager.gameState = "window";
                UIMan.show(theGame);
            }
        }

    }
}
