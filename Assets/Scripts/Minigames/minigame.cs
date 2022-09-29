using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class minigame : MonoBehaviour
{
    [SerializeField] private UIManager UIMan;
    public GameObject notification;
    public GameObject theMinigame;
    private bool isGame = false;
    
    [SerializeField] private string eMessage;

    private void Start()
    {
    }

    private void Update()
    {
        if (isGame)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIMan.denoitfy();
                
                startGame();
            }
        }
    }
    void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") 
        {
            //changes popup text and shows popup notification in UIManager
            UIMan.notify(eMessage);
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
    public void startGame()
    {
        UIMan.gameState = "window";
        UIMan.show(theMinigame);
    }
}
