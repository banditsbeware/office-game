using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class UIManagerInstance : MonoBehaviour
{
	[SerializeField] public TMP_Text canvasText;

	void Awake()
	{
		UIManager.instance = this;

		Time.timeScale = 1;
		UIManager.pauseObjects = GameObject.FindGameObjectsWithTag("Pause");
		UIManager.notifs = GameObject.FindGameObjectsWithTag("notif");
		UIManager.minigames = GameObject.FindGameObjectsWithTag("minigame");

		UIManager.Hide(UIManager.pauseObjects);
		UIManager.Hide(UIManager.notifs);
		UIManager.Hide(UIManager.minigames);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			UIManager.PauseControl();
		}
	}

//	public void PauseControl()
//	{
//		UIManager.PauseControl();
//	}
}
