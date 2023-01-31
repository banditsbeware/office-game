using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class interact_door : interact
{
    public LevelLoader loader;
    public string scene;

    private void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                UIMan.denoitfy();
                loader.LoadLevel(scene);
            }
        }
    }
}
