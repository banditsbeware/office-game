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
                UIManager.Denotify();
                loader.LoadLevel(scene);
            }
        }
    }
}
