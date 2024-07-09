using SpeakEasy;
using UnityEngine;

//interact minigame only used for minigames made on a Canvas
public class cutscene : MonoBehaviour
{
    public GameObject dialogueWindow;
    public GameObject characterObject;
    [HideInInspector] public SEDialogue dialogue;
    public AK.Wwise.Bank minigameBank;
    public Animator cutsceneAnimation;
    internal Camera mainCamera;
    internal bool endFlag = false;
    
    public virtual void Start() 
    {
        dialogue = characterObject.transform.Find("Dialogue").GetComponent<SEDialogue>();
        cutsceneAnimation = gameObject.GetComponent<Animator>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        mainCamera.GetComponent<FollowMan>().inCutscene = true;
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
        endFlag = true;
    }
    
    public virtual void CutsceneFinished()
    {
        UIManager.ExitCutscene();
        GameObject.Find("Main Camera").GetComponent<FollowMan>().inCutscene = false;
        cutsceneAnimation.enabled = false;   
    }
}
