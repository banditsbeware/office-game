using System;
using System.Collections.Generic;
using UnityEngine;

//interact bed only used on bed in apartment
public class interact_bed : interact_door
{
    private System.Random random = new System.Random();

    public Vector2 bedsidePosition;
    public Vector2 sleepPosition;
    public Vector2 interrumPosition;
    public List<string> scenes = new List<string>();
    private DreamLoader dreamer;

    private void Start() {
        dreamer = (DreamLoader) loader;
    }

    public override void Update()
    {
        if (isInteractable)
        {
            if (Input.GetKeyDown(KeyCode.E) && UIManager.gameState == UIManager.state.PLAY)
            {
                //work out functionality about deciding which dream to choose

                Meta.Global["day"] += 1;
                UIManager.Denotify();
                UIManager.gameState = UIManager.state.WINDOW;
                dreamer.LoadDream(scene, bedsidePosition, sleepPosition);
            }
        }
    }

    public override void OnTriggerEnter2D(Collider2D other)
    {
        base.OnTriggerEnter2D(other);
        
        //choose random dream each time you enter the sleep trigger box
        scene = scenes[random.Next(0, scenes.Count)];
    }
}
