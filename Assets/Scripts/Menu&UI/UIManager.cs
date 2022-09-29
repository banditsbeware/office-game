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
	public GameObject canvasNotifs;
	private TMP_Text canvasText;

    void Start()
    {
        Time.timeScale = 1;
        pauseObjects = GameObject.FindGameObjectsWithTag("Pause");
		notifs = GameObject.FindGameObjectsWithTag("notif");
		minigames = GameObject.FindGameObjectsWithTag("minigame");
		hide(pauseObjects);
		hide(notifs);
		// hide(minigames);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if(Time.timeScale == 1)
            {
                Time.timeScale = 0;
                show(pauseObjects);
            } else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                hide(pauseObjects);
            }
        }
    }
	// reloads current scene index (in Build Management)
    public void Reload(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
	}

	public void pauseControl(){
			if(Time.timeScale == 1)
			{
				Time.timeScale = 0;
				show(pauseObjects);
			} else if (Time.timeScale == 0){
				Time.timeScale = 1;
				hide(pauseObjects);
			}
	}

	//  shows objects with tag
	public void show(GameObject[] tagg){
		foreach(GameObject g in tagg){
			g.SetActive(true);
		}
	}
	public void show(GameObject obj){
			obj.SetActive(true);
	}

	// hides objects with tag
	public void hide(GameObject[] tagg){
		foreach(GameObject g in tagg){
			g.SetActive(false);
		}
	}
	public void hide(GameObject obj){
			obj.SetActive(false);
	}
	
	// changes text in popup notification
	public void notify(string popup){
		canvasNotifs.GetComponent<TMP_Text>().text = popup;
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
