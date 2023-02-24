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

		UIManager.hide(UIManager.pauseObjects);
		UIManager.hide(UIManager.notifs);
		UIManager.hide(UIManager.minigames);
	}

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Escape))
		{
				UIManager.pauseControl();
		}
	}
}
