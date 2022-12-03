using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator fadeTransition;
    public float transitionTime = 1f;
    private Transform door;  //door you will be exiting from
    public void LoadLevel(string levelName)
    {
        MoveHim.doorToExit = "door_" + SceneManager.GetActiveScene().name;
        StartCoroutine(LoadScene(levelName));
    }

    //fade in/out
    IEnumerator LoadScene(string levelName)
    {
        fadeTransition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }

}
