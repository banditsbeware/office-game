using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelLoader : MonoBehaviour
{
    public Animator fadeTransition;
    public float transitionTime = 1f;
    internal Transform door;  //door you will be exiting from
    public TMP_Text debugText;

    void Start()
    {
        Meta.Global["currentScene"] = SceneManager.GetActiveScene().name;
    }

    public void LoadLevel(string levelName)
    {
        MoveHim.doorToExit = "door_" + SceneManager.GetActiveScene().name;
        StartCoroutine(LoadNextScene(levelName));
    }

    //fade in/out
    IEnumerator LoadNextScene(string levelName)
    {
        fadeTransition.SetTrigger("Start");

        AkSoundEngine.PostEvent("Fade_All", gameObject);

        if(Meta.Global["currentScene"] != "MainMenu") 
        {
            Time.timeScale = 1;
			UIManager.hide(UIManager.pauseObjects);
			UIManager.gameState = "play";
        }

        yield return new WaitForSeconds(transitionTime);

        Meta.Global["currentScene"] = levelName;

        SceneManager.LoadScene(levelName);
    }
}
