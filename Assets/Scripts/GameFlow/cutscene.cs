using SpeakEasy;
using UnityEngine;

//interact minigame only used for minigames made on a Canvas
public class cutscene : MonoBehaviour
{
    public GameObject dialogueWindow;
    internal GameObject playerObject;
    public GameObject dialogueObject;
    [HideInInspector] public SEDialogue dialogue;
    public AK.Wwise.Bank minigameBank;
    internal Animator cutsceneAnimation;
    internal Camera mainCamera;
    internal bool endFlag = false;
    
    public virtual void Start() 
    {
        dialogue = dialogueObject.GetComponent<SEDialogue>();
        cutsceneAnimation = gameObject.GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamera.GetComponent<FollowMan>().inCutscene = true;
        playerObject = GameObject.FindGameObjectWithTag("Player");
    }
    public virtual void StartCutscene()
    {
        UIManager.denoitfy();
        UIManager.EnterCutscene();
        UIManager.show(dialogueWindow);

        minigameBank.Load();
        dialogue.BeginDialogue();
    }


    public void UnloadBank()
    {
        minigameBank.Unload();
    }

    public virtual void EndCutscene()
    {
        endFlag = true;
    }
    
    public virtual void CutsceneFinished()
    {
        UIManager.ExitCutscene();
        UIManager.hide(dialogueWindow);
        GameObject.Find("Main Camera").GetComponent<FollowMan>().inCutscene = false;
        cutsceneAnimation.enabled = false;   
    }
}
