using SpeakEasy;
using UnityEngine;

//interact minigame only used for minigames made on a Canvas
public class interact_dialogue : interact
{
    public GameObject dialogueWindow;
    public GameObject characterObject;
    [HideInInspector] public SEDialogue dialogue;
    public AK.Wwise.Bank minigameBank;
    
    private void Awake() 
    {
        dialogue = characterObject.transform.Find("Dialogue").GetComponent<SEDialogue>();
    }
    private void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E) && UIManager.gameState == "play")
            {
                UIManager.denoitfy();
                UIManager.gameState = "window";
                UIManager.show(dialogueWindow);
                UIManager.show(dialogue.gameObject);

                dialogue.BeginDialogue();
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
            minigameBank.Unload();
        }
    }
    
}
