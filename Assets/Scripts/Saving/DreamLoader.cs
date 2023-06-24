using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DreamLoader : LevelLoader
{
    public Rigidbody2D rigidBed;
    public Rigidbody2D rigidWalls;

    public void LoadDream(string levelName, Vector2 bedside, Vector2 inBed)
    {
        MoveHim.doorToExit = "door_origin";
        StartCoroutine(LoadDreamScene(levelName, bedside, inBed));
    }

    //getting into bed animation
    IEnumerator LoadDreamScene(string levelName, Vector2 bedside, Vector2 inBed)
    {
        MoveHim player = GameObject.FindGameObjectWithTag("Player").GetComponent<MoveHim>();
        rigidBed.simulated = false;
        rigidWalls.simulated = false;
        player.AnimateMovement(bedside, 200);
        
        while (player.isAnimating)
        {
            yield return null;
        }

        yield return new WaitForSeconds(1f);

        player.AnimateMovement(inBed, 150);
        
        while (player.isAnimating)
        {
            yield return null;
        }

        //animation
        player.Sleep();

        yield return new WaitForSeconds(1f);

        fadeTransition.speed = .3f;
        fadeTransition.SetTrigger("Start");

        AkSoundEngine.PostEvent("Fade_All", gameObject);

        Time.timeScale = 1;
        UIManager.Hide(UIManager.pauseObjects);

        yield return new WaitForSeconds(transitionTime);

        Meta.Global["currentScene"] = levelName;
        UIManager.gameState = UIManager.state.PLAY;

        SceneManager.LoadScene(levelName);
    }
}
