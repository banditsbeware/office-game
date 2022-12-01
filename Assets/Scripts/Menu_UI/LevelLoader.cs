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
        if (levelName == "Street")
        {
            //control which door you spawn from when level loads
            if (SceneManager.GetActiveScene().name == "Office")
            {
                MoveHim.spawnLocation.x = 0;
            }
            else
            {
                MoveHim.spawnLocation.x = 30;
            }
        }
        else
        {
            MoveHim.spawnLocation = new Vector3(0, 0, 0);
        }

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
