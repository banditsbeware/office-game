using UnityEngine;

//interact minigame only used for minigames made on a Canvas
public class interact_bed : interact_minigame
{
    public override void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E) && UIManager.gameState == "play")
            {
                Meta.Global["day"] += 1;
                UIManager.denoitfy();
                UIManager.gameState = "window";
                UIManager.show(theGame);
            }
        }

    }
}
