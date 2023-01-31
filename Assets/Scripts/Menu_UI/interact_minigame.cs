using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interact minigame only used on minigames on a Canvas
public class interact_minigame : interact
{
    public GameObject theGame;
    public AK.Wwise.Bank minigameBank;
    private void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIMan.denoitfy();
                UIManager.gameState = "window";
                UIManager.show(theGame);
            }
        }

    }

    //loads/unloads a soundbank specific to the minigame before the minigame starts
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == "Player")
        {
            minigameBank.Load();
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.tag == "Player")
        {
            minigameBank.Unload();
        }
    }
    
}
