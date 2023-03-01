using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

using static SpeakEasy.Enumerations.MetaVariable;

public class LevelLoader : MonoBehaviour
{
    public Animator fadeTransition;
    public float transitionTime = 1f;
    private Transform door;  //door you will be exiting from

    void Start()
    {
        Meta.Variables["currentScene"] = SceneManager.GetActiveScene().name;
    }

    public void LoadLevel(string levelName)
    {
        MoveHim.doorToExit = "door_" + SceneManager.GetActiveScene().name;
        StartCoroutine(LoadScene(levelName));
    }

    //fade in/out
    IEnumerator LoadScene(string levelName)
    {
        fadeTransition.SetTrigger("Start");

        AkSoundEngine.PostEvent("Fade_All", gameObject);

        if(Meta.Variables["currentScene"] != "MainMenu") 
        {
            Time.timeScale = 1;
			UIManager.hide(UIManager.pauseObjects);
			UIManager.gameState = "play";
        }

        yield return new WaitForSeconds(transitionTime);

        Meta.Variables["currentScene"] = levelName;

        SceneManager.LoadScene(levelName);
    }
}
