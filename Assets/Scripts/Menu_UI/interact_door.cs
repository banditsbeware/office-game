using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact_door : interact
{
    public LevelLoader loader;
    public string scene;

    virtual public void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (ProgressChecks.LeaveWorkCheck(scene)) // Returns true unless attempting to leave office before tasks complete
                {
                    UIManager.denoitfy();
                    loader.LoadLevel(scene);
                }
                else
                {
                    Debug.Log("Fuck you, respectfully");
                    //trigger boss stopping you at door / dialog
                    //remove interactivity of door until leave trigger
                }
            }
        }
    }
}
