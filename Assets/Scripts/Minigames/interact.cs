using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact : MonoBehaviour
{
    public UIManager UIMan;
    public GameObject notification;
    [System.NonSerialized] public bool isGame = false;
    
    [SerializeField] private string eMessage;

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
}