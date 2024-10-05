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

	// gamestates are: pause, play, window, cutscene
	public static string gameState = "play";
	public static string bufferState = null;

	// rigid body constraints are different in different scenes, this stores them temporarily when paused

	public static RigidbodyConstraints2D currentPlayerConstraints;


	// reloads current scene index (in Build Management)
	public static void Reload(){
	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public static void pauseControl()
	{
		switch (gameState)
		{
			case "play": //when esc is pressed when in standard play mode or cutscene, game will move to pause
				Time.timeScale = 0;
				show(pauseObjects);
				bufferState = "play";
				gameState = "pause";
			break;
			case "cutscene": 
				Time.timeScale = 0;
				show(pauseObjects);
				bufferState = "cutscene";
				gameState = "pause";
			break;
			case "pause": //game will revery to whatever state it was in when it was paused
				Time.timeScale = 1;
				hide(pauseObjects);
				gameState = bufferState;
				bufferState = null;
			break;
			case "window": //esc will only exit you from a minigame, not pause.
				hide(minigames);
				GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints = currentPlayerConstraints;
				gameState = "play";
			break;
		}
	}

	public static void EnterCutscene()
	{
		currentPlayerConstraints = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints;
		GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		gameState = "cutscene";
	}

	public static void ExitCutscene()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints = currentPlayerConstraints;
		gameState = "play";
	}

	public static void EnterMinigame()
	{
		currentPlayerConstraints = GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints;
		Debug.Log(currentPlayerConstraints);
		GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
		gameState = "window";
	}

	public static void ExitMinigame()
	{
		GameObject.FindGameObjectWithTag("Player").GetComponent<Rigidbody2D>().constraints = currentPlayerConstraints;
		gameState = "play";
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