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

	public enum state {
		PAUSE,
		PLAY,
		WINDOW
	}

	// Global game state
	public static state gameState = state.PLAY;

	// reloads current scene index (in Build Management)
	public static void Reload(){
	SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	// PauseControl is called from UIManagerInstance to handle game state
	// logic when the player presses the escape key.
	public static void PauseControl()
	{
		// Escape pressed while in PLAY state
		if (gameState == state.PLAY)
		{
			Time.timeScale = 0;
			Show(pauseObjects);
			gameState = state.PAUSE;
		} 
		// Escape pressed while in PAUSE state
		else if (gameState == state.PAUSE) 
		{
			Time.timeScale = 1;
			Hide(pauseObjects);
			gameState = state.PLAY;
		}
		// Escape pressed while in WINDOW state (minigames, ...?)
		else if (gameState == state.WINDOW) 
		{
			Hide(minigames);
			gameState = state.PLAY;
		}
	}

	// Shows a list of GameObjects
	public static void Show(GameObject[] objList)
	{
		foreach(GameObject g in objList) Show(g);
	}

	// Shows a GameObject
	public static void Show(GameObject obj)
	{
		obj.SetActive(true);
	}

	// Hides a list of GameObjects
	public static void Hide(GameObject[] objList)
	{
		foreach(GameObject g in objList) Hide(g);
	}

	// Hides a GameObject
	public static void Hide(GameObject obj)
	{
		obj.SetActive(false);
	}
	
	// Changes text in popup notification and shows all notifs
	public static void Notify(string popup)
	{
		instance.canvasText.text = popup;
		Show(notifs);
	}

	// Hide all notifs
	public static void Denotify()
	{
		Hide(notifs);
	}

	public static Vector3 mouseLocation()
    {
		Vector3 screenPoint = Input.mousePosition;
		screenPoint.z = -Camera.main.transform.position.z; //distance of the plane from the camera
		return Camera.main.ScreenToWorldPoint(screenPoint); 
    }
}