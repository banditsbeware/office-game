using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigame : MonoBehaviour
{
    private bool isGame = false;
    
    public GameObject UI;
    private UIManager UIMan;
    public GameObject notification;
    public GameObject theMinigame;
    private void Start()
    {
        UIMan = UI.GetComponent<UIManager>();
    }
    private void Update()
    {
        if (isGame)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIMan.denoitfy();
                
                startCoolerGame();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") 
        {
            //changes popup text and shows popup notification in UIManager
            UIMan.notify("Harold [E]");

            isGame = true;
        }
    }
    
    void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") 
        {
            //hides popup notification in UIManager
            UIMan.denoitfy();

            isGame = false;
        }
    }
// begins cooler mini-game
    public void startCoolerGame()
    {
        UIMan.show(theMinigame);
    }
}
