using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
    [HideInInspector] public GameObject[] pauseObjects;
	[HideInInspector] public GameObject[] notifs;
	[HideInInspector] public GameObject[] minigames;
	[SerializeField] private TMP_Text canvasText;


	// gamestates are: pause, play, window
	public static string gameState = "play";

    void Awake()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("Pause");
		notifs = GameObject.FindGameObjectsWithTag("notif");
		minigames = GameObject.FindGameObjectsWithTag("minigame");

		hide(pauseObjects);
		hide(notifs);
		hide(minigames);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            pauseControl();
        }
    }
	// reloads current scene index (in Build Management)
    public void Reload(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void pauseControl()
	{
		if (gameState == "play")
		{
			Time.timeScale = 0;
			show(pauseObjects);
			gameState = "pause";
		} 
		else if (gameState == "pause")
		{
			Time.timeScale = 1;
			hide(pauseObjects);
			gameState = "play";
		}
		else if (gameState == "window")
		{
			hide(minigames);
			gameState = "play";
		}
	}

	//  shows objects with tag
	public void show(GameObject[] tagg){
		foreach(GameObject g in tagg){
			show(g);
		}
	}
	public void show(GameObject obj){
			obj.SetActive(true);
	}

	// hides objects with tag
	public void hide(GameObject[] tagg){
		foreach(GameObject g in tagg){
			hide(g);
		}
	}
	public void hide(GameObject obj){
			obj.SetActive(false);
	}
	
	// changes text in popup notification
	public void notify(string popup){
		canvasText.text = popup;
		show(notifs);
	}

	public void denoitfy()
	{
		hide(notifs);
	}


	// //loads inputted level
	// public void LoadLevel(string level){
	// 	Application.LoadLevel(level);
	// }
}
