using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManager : MonoBehaviour
{
  [HideInInspector] public GameObject[] pauseObjects; //Objects in Pause menu
	[HideInInspector] public GameObject[] notifs; //any UI popups not part of pause menu

	//minigames are all made in UI, I'm so sorry
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
		if (gameState == "play") //when esc is it while playing
		{
			Time.timeScale = 0;
			show(pauseObjects);
			gameState = "pause";
		} 
		else if (gameState == "pause") //when resume or esc are hit in pause menu
		{
			Time.timeScale = 1;
			hide(pauseObjects);
			gameState = "play";
		}
		else if (gameState == "window") // exit from minigame
		{
			hide(minigames);
			gameState = "play";
		}
	}

	//  shows objects with tag
	public static void show(GameObject[] tagg){
		foreach(GameObject g in tagg)
		{
			show(g);
		}
	}

	public static void show(GameObject obj){
			obj.SetActive(true);
	}

	// hides objects with tag
	public static void hide(GameObject[] tagg){
		foreach(GameObject g in tagg)
		{
			hide(g);
		}
	}

	public static void hide(GameObject obj){
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

	public static Vector3 mouseLocation()
    {
			Vector3 screenPoint = Input.mousePosition;
			screenPoint.z = 10; //distance of the plane from the camera
			return Camera.main.ScreenToWorldPoint(screenPoint); 
    }
}
