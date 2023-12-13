using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public static class UIManager
{
	public static UIManagerInstance instance;

  [HideInInspector] public static GameObject[] pauseObjects; //Objects in Pause menu
	[HideInInspector] public static GameObject[] notifs; //any UI popups not part of pause menu

	//minigames are all made in UI, I'm so sorry
	[HideInInspector] public static GameObject[] minigames; 

	// gamestates are: pause, play, window
	public static string gameState = "play";

	// reloads current scene index (in Build Management)
	public static void Reload(){
	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void pauseControl()
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
			GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None | RigidbodyConstraints2D.FreezeRotation;
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
	public static void notify(string popup){
		instance.canvasText.text = popup;
		show(notifs);
	}

	public static void denoitfy()
	{
		hide(notifs);
	}

	public static Vector3 mouseLocation()
    {
			Vector3 screenPoint = Input.mousePosition;
			screenPoint.z = -Camera.main.transform.position.z; //distance of the plane from the camera
			return Camera.main.ScreenToWorldPoint(screenPoint); 
    }
}