using SpeakEasy;
using UnityEngine;

//interact minigame only used for minigames made on a Canvas
public class cutscene : MonoBehaviour
{
    public GameObject dialogueWindow;
    public GameObject characterObject;
    [HideInInspector] public SEDialogue dialogue;
    public AK.Wwise.Bank minigameBank;
    
    public virtual void Start() 
    {
        dialogue = characterObject.transform.Find("Dialogue").GetComponent<SEDialogue>();
        GameObject.Find("Main Camera").GetComponent<FollowMan>().inCutscene = true;
    }
    public void StartCutscene()
    {
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

    public virtual void EndCutscene()
    {
        GameObject.Find("Main Camera").GetComponent<FollowMan>().inCutscene = false;
    }
    
}
