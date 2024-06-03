using SpeakEasy;
using UnityEngine;

//interact minigame only used for minigames made on a Canvas
public class cutscene : MonoBehaviour
{
    public GameObject dialogueWindow;
    public GameObject characterObject;
    [HideInInspector] public SEDialogue dialogue;
    public AK.Wwise.Bank minigameBank;
    
    private void Start() 
    {
        dialogue = characterObject.transform.Find("Dialogue").GetComponent<SEDialogue>();
    }
    public void StartCutscene()
    {
        Debug.Log(dialogue.gameObject.name);
        UIManager.denoitfy();
        UIManager.EnterCutscene();
        UIManager.show(dialogueWindow);
        UIManager.show(dialogue.gameObject);

        minigameBank.Load();
        dialogue.BeginDialogue();
    }


    public void UnloadBank()
    {
        minigameBank.Unload();
    }
    
}
