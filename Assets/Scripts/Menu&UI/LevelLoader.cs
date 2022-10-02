using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator fadeTransition;
    public float transitionTime = 1f;
    
    public void LoadLevel(string levelName)
    {
        StartCoroutine(LoadScene(levelName));
    }

    IEnumerator LoadScene(string levelName)
    {
        fadeTransition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelName);
    }

}
