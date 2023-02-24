using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator fadeTransition;
    public float transitionTime = 1f;
    private Transform door;  //door you will be exiting from

    void Start()
    {
        meta.currentScene = SceneManager.GetActiveScene().name;
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

        if(meta.currentScene != "MainMenu") 
        {
            Time.timeScale = 1;
			UIManager.hide(UIManager.pauseObjects);
			UIManager.gameState = "play";
        }

        yield return new WaitForSeconds(transitionTime);

        meta.currentScene = levelName;

        SceneManager.LoadScene(levelName);
    }
}
