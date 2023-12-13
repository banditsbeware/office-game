using UnityEngine;

//interact minigame only used for minigames made on a Canvas
public class interact_minigame : interact
{
    public GameObject theGame;
    public AK.Wwise.Bank minigameBank;
    private void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E) && UIManager.gameState == "play")
            {
                UIManager.denoitfy();
                UIManager.gameState = "window";
                GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
                UIManager.show(theGame);
            }
        }

    }

    //loads/unloads a soundbank specific to the minigame before the minigame starts
    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        if (other.tag == "Player" && minigameBank != null)
        {
            minigameBank.Load();
        }
    }

    public override void OnTriggerExit2D(Collider2D other)
    {
        base.OnTriggerExit2D(other);
        if (other.tag == "Player" && minigameBank != null)
        {
            Debug.Log("Oof");
            minigameBank.Unload();
        }
    }
    
}
