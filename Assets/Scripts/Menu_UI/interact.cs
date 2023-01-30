using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//interact_door and interact_minigame inherit this class
public class interact : MonoBehaviour
{
    public UIManager UIMan;   
    [System.NonSerialized] public bool isGame = false;
    
    [SerializeField] private string eMessage;

    virtual public void OnTriggerEnter2D(Collider2D other) {
        if (other.tag == "Player") 
        {
            //changes popup text and shows popup notification in UIManager
            UIMan.notify(eMessage);
            isGame = true;
        }
    }
    
    virtual public void OnTriggerExit2D(Collider2D other) {
        if (other.tag == "Player") 
        {
            //hides popup notification in UIManager
            UIMan.denoitfy();
            isGame = false;
        }
    }
}
